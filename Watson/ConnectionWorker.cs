using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;
using Watson.Message;
using Watson.Message.Handler;

namespace Watson
{
    class ConnectionWorker
    {
        private TcpClient connection;
        private Stream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private Program program;
        public const string IRC_PATTERN = "^(?:[:](\\S+) )?(\\S+)(?: (?!:)(.+?))?(?: [:](.*))?$";
        public Regex REGEX = new Regex(IRC_PATTERN);
        private IRCServer server;
        
        private bool Working
        {
            get; set;
        }

        public void Write(string text)
        {
            writer.Write(text);

        }

        public void WriteAndFlush(string text)
        {
            writer.Write(text);
            Flush();
        }

        public void Flush()
        {
            writer.Flush();
        }


        public void Process()
        {
            while (Working)
            {
                string line = reader.ReadLine();
                if (line == null)
                {
                    server.Dispose();
                }
                if (REGEX.IsMatch(line))
                {
                    Match match = REGEX.Match(line);
                    IncomingMessage msg = new IncomingMessage(server, line, match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value);

                    foreach (MessageListener ml in Protocol.LISTENERS)
                    {
                        if (ml.ShouldHandle(msg))
                        {
                            ml.Handle(msg);
                        }
                    }
                    Console.Write(msg.Raw + "\n");
                }
                else
                {
                    Console.Write("UNMATCHED\n");
                }
            }
        }

        public void Stop()
        {
            Working = false;
            stream.Dispose();
            connection.Close();
        }

        public ConnectionWorker(IRCServer server)
        {
            this.server = server;
            program = Program.GetInstance();
            server.SetWorker(this);

            connection = new TcpClient(server.IP, server.PORT);
            stream = connection.GetStream();
            if (server.SSL)
            {
                
                stream = Protocol.Secure(server, stream);
            }
            this.reader = new StreamReader(stream);
            this.writer = new StreamWriter(stream);


            while (!stream.CanWrite) ;
            Console.WriteLine("LAL: " + server.Nick);
            this.writer.Write("NICK " + server.Nick + "\r\n");
            this.writer.Write("USER " + server.RealName + " 0 * :" + server.RealName + "\r\n");
            this.writer.Flush();
            Working = true;
        }



    }
}
