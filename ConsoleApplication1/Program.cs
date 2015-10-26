using System.IO;
using System.Threading;
using System.Xml.Linq;
using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    class Program
    {
        private static volatile Program INSTANCE;
        private List<IRCServer> toConnect;

        static void Main(string[] args)
        {
            Program program = new Program();
            if (program.Load())
            {
                INSTANCE = program;


                program.ConnectAll();
            }

        }

        public bool Load()
        {
            string[] files = Directory.GetFiles("servers/");
            toConnect = new List<IRCServer>(files.Length);
            foreach (string file in files)
            {
                XDocument xd = XDocument.Load(file);
                Console.Write("Loading " + file + "\n");

                XElement el = xd.Root.Element("connection");
                string ip = el.Attribute("ip").Value;
                int port = int.Parse(el.Attribute("port").Value);

                string nick = el.Element("name").Value;
                string pass = el.Element("pass").Value;
                string altnick = el.Element("altnick").Value;
                string realname = el.Element("realname").Value;
                bool ssl = bool.Parse(el.Element("ssl").Value);
                List<IRCChannel> channels = new List<IRCChannel>();
                foreach (XElement ch in el.Elements("channels"))
                {
                    XElement x = ch.Element("channel");
                    string channel = x.Attribute("name").Value;
                    bool reconnect = bool.Parse(x.Attribute("reconnect").Value);
                    IRCChannel chan = new IRCChannel(channel, reconnect);
                    channels.Add(chan);

                }
                IRCServer server = new IRCServer(ip, port, channels, ssl);
                toConnect.Add(server);

            }
            Console.WriteLine("Loaded " + toConnect.Count);
            return true;
        }
        private void ConnectAll()
        {
            foreach (IRCServer server in toConnect)
            {
                ConnectionWorker worker = new ConnectionWorker(server);
                Thread t = new Thread(worker.Process);
                t.Start();
                Console.WriteLine("Starting " + server.getIp());
            }
        }

        public bool IsRunning()
        {
            return true;
        }

        public static Program GetInstance()
        {

            return INSTANCE;

        }

    }
}
