using System.IO;
using System.Threading;
using System.Xml.Linq;
using System;
using System.Collections.Generic;

namespace Watson
{
    class Program
    {
        private static volatile Program INSTANCE;

        private List<IRCServer> toConnect;
        private List<IRCServer> connected;

        static void Main(string[] args)
        {
            Program program = new Program();
            if (program.Load())
            {
                INSTANCE = program;
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
                Console.WriteLine("Starting " + server.IP);
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
