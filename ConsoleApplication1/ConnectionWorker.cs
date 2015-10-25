using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace ConsoleApplication1
{
    class ConnectionWorker {
        private string IP;
        private int PORT;
        private TcpClient connection;
        private Stream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private Program program;
        public const string IRC_PATTERN = "^(?:[:](\\S+) )?(\\S+)(?: (?!:)(.+?))?(?: [:](.*))?$";
        public Regex REGEX = new Regex(IRC_PATTERN);
        //private const List<MessageListener> LISTENERS

        public void Process()
        {

            string line = null;
            while((line = reader.ReadLine())!= null)
            {
                

                if(REGEX.IsMatch(line))
                {
                    Match match = REGEX.Match(line);
                    //Console.Write(line + "\n");
                    IncomingMessage msg = new IncomingMessage(line, match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
                    Console.Write(msg.GetRaw() + "\n");
                } else
                {
                    Console.Write("UNMATCHED LIKE SHIT\n");
                }


            }
        }

        public ConnectionWorker(String IP, int PORT)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.program = Program.GetInstance();

            this.connection = new TcpClient(IP, PORT);
            this.stream = connection.GetStream();
            this.reader = new StreamReader(stream);
            this.writer = new StreamWriter(stream);

            while (!stream.CanWrite) ;
            this.writer.Write("NICK Watson\r\n");
            this.writer.Write("USER watttoo 0 * :bigfat\r\n");
            this.writer.Flush();
        }
    }
}
