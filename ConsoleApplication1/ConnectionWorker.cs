using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Security.Authentication;
using ConsoleApplication1.Message;
using ConsoleApplication1.Message.Handler;

namespace ConsoleApplication1
{
    class ConnectionWorker
    {
        private string IP;
        private int PORT;
        private TcpClient connection;
        private Stream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private Program program;
        public const string IRC_PATTERN = "^(?:[:](\\S+) )?(\\S+)(?: (?!:)(.+?))?(?: [:](.*))?$";
        public Regex REGEX = new Regex(IRC_PATTERN);

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

        private List<MessageListener> LISTENERS = new List<MessageListener>();
        private IRCServer server;
        private X509Certificate _SslClientCertificate;

        public void Process()
        {
            Console.WriteLine("PROCESS");
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                if (REGEX.IsMatch(line))
                {
                    Match match = REGEX.Match(line);
                    IncomingMessage msg = new IncomingMessage(server, line, match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value);

                    foreach (MessageListener ml in LISTENERS)
                    {
                        if (ml.ShouldHandle(msg))
                        {
                            ml.Handle(msg);
                        }
                    }
                    Console.Write(msg.GetRaw() + "\n");
                }
                else
                {
                    Console.Write("UNMATCHED\n");
                }


            }
        }

        public ConnectionWorker(IRCServer server)
        {
            this.IP = server.getIp();
            this.PORT = server.getPort();
            this.server = server;
            this.program = Program.GetInstance();
            server.SetWorker(this);

            LISTENERS.Add(new ConnectedHandler());
            LISTENERS.Add(new PingHandler());
            LISTENERS.Add(new CommandListener());
            LISTENERS.Add(new MarkovListener());
            LISTENERS.Add(new ProtocolHandler());

            this.connection = new TcpClient(IP, PORT);
            this.stream = connection.GetStream();
            if (server.IsSSL())
            {
                RemoteCertificateValidationCallback certValidation;
                if (_ValidateServerCertificate)
                {
                    certValidation = ServicePointManager.ServerCertificateValidationCallback;
                    if (certValidation == null)
                    {
                        certValidation = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        {
                            if (sslPolicyErrors == SslPolicyErrors.None)
                            {
                                return true;
                            }
                            return false;
                        };
                    }
                }
                else
                {
                    certValidation = delegate { return true; };
                }
                RemoteCertificateValidationCallback certValidationWithIrcAsSender = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return certValidation(this, certificate, chain, sslPolicyErrors);
                    };
                LocalCertificateSelectionCallback selectionCallback = delegate (object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
                {
                    if (localCertificates == null || localCertificates.Count == 0)
                    {
                        return null;
                    }
                    return localCertificates[0];
                };
                SslStream sslStream = new SslStream(stream, false,
                                                    certValidationWithIrcAsSender,
                                                    selectionCallback);

                if (_SslClientCertificate != null)
                {
                    var certs = new X509Certificate2Collection();
                    certs.Add(_SslClientCertificate);
                    sslStream.AuthenticateAsClient(IP, certs,
                                                   SslProtocols.Default,
                                                   false);
                }
                else
                {
                    sslStream.AuthenticateAsClient(IP);
                }

                stream = sslStream;
            }
            this.reader = new StreamReader(stream);
            this.writer = new StreamWriter(stream);


            while (!stream.CanWrite) ;
            this.writer.Write("NICK " + server.GetNick() + "\r\n");
            this.writer.Write("USER " + server.GetRealName() + " 0 * :bigfat\r\n");
            this.writer.Flush();

        }



        private bool _ValidateServerCertificate;
        public X509Certificate SslClientCertificate
        {
            get
            {
                return _SslClientCertificate;
            }
            set
            {
                _SslClientCertificate = value;
            }
        }
    }
}
