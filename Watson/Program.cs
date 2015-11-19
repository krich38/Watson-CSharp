using System.IO;
using System.Threading;
using System.Xml.Linq;
using System;
using System.Collections.Generic;

namespace Watson
{
    class Program
    {
        private static object obj = new object();
        private static Program _instance;
        public static Program INSTANCE
        {
            get
            {
                lock (obj)
                {
                    if (_instance == null)
                    {
                        _instance = new Program();
                    }
                    return _instance;
                }
            }
        }


        private List<IRCServer> toConnect;
        private List<IRCServer> connected;

        static void Main(string[] args)
        {
            Program program = INSTANCE;
            if (program.Load())
            {
                program.ConnectAll();
            }
        }

        private bool Load()
        {
            List<IRCServer> toConnect = Configuration.LoadServers();
            if (toConnect != null)
            {
                this.toConnect = toConnect;
                return Database.EstablishConnection();
            }
            return false;

        }



        private void ConnectAll()
        {
            connected = new List<IRCServer>();

            foreach (IRCServer server in toConnect)
            {
                ConnectionWorker worker = new ConnectionWorker(server);
                Thread t = new Thread(worker.Process);
                t.Start();
            }
        }

        public bool IsRunning()
        {
            return true;
        }

        public void OnDispose(IRCServer server)
        {
            connected.Remove(server);
        }

        public void OnConnected(IRCServer server)
        {
            connected.Add(server);
            toConnect.Remove(server);
            Console.WriteLine("Established connection to " + server.ToString());
        }
    }
}
