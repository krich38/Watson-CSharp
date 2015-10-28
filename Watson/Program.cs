﻿using System.IO;
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

                XElement cc = el.Element("channels");
                foreach (XElement dd in cc.Elements())
                {
                    string channel = dd.Element("name").Value;
                    bool reconnect = bool.Parse(dd.Element("reconnect").Value);
                    IRCChannel chan = new IRCChannel(channel, reconnect);
                    channels.Add(chan);
                }

                XElement users = el.Element("users");
                Dictionary<string, UserAccess> serverUsers = new Dictionary<string, UserAccess>();
                foreach (XElement user in users.Elements())
                {
                    string host = user.Element("host").Value;
                    string access = user.Element("access").Value;
                    UserAccess ua = UserAccessAttr.GetByAccess(int.Parse(access));
                    serverUsers.Add(host, ua);
                }
                IRCServer server = new IRCServer(ip, port, channels, ssl, nick, pass, altnick, realname, serverUsers);
                toConnect.Add(server);
            }
            Console.WriteLine("Loaded " + toConnect.Count + " servers");
            return Database.EstablishConnection();
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
