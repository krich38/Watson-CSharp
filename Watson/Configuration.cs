using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Watson
{
    class Configuration
    {
        public static List<IRCServer> LoadServers()
        {
            string[] files = Directory.GetFiles("servers/");
            if (files.Length > 0)
            {
                List<IRCServer> servers = new List<IRCServer>(files.Length);
                foreach (string file in files)
                {
                    XDocument xd = XDocument.Load(file);
                    Console.Write("Loading " + file + "\n");

                    XElement el = xd.Element("connection");
                    string ip = el.Attribute("ip").Value;
                    int port = int.Parse(el.Attribute("port").Value);

                    string nick = el.Element("name").Value;
                    string pass = el.Element("pass").Value;
                    string altnick = el.Element("altnick").Value;
                    string realname = el.Element("realname").Value;
                    bool loggingRaw = bool.Parse(el.Element("lograw").Value);
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
                    IRCServer server = new IRCServer(file, ip, port, channels, ssl, nick, pass, altnick, realname, serverUsers, loggingRaw);
                    servers.Add(server);
                }
                Console.WriteLine("Loaded " + servers.Count + " servers");
                return servers;
            }
            else
            {
                return null;
            }
        }

        public static bool Save(IRCServer server)
        {
            XDocument xd = new XDocument();
            XElement connection = new XElement("connection", new XAttribute("ip", server.IP), new XAttribute("port", server.PORT));
            connection.Add(new XElement("name", server.Nick));
            connection.Add(new XElement("pass", server.Pass));
            connection.Add(new XElement("altnick", server.AltNick));
            connection.Add(new XElement("realname", server.RealName));
            connection.Add(new XElement("ssl", server.SSL));
            connection.Add(new XElement("lograw", server.LoggingRaw));
            xd.Add(connection);
            XElement channels = new XElement("channels");
            foreach (IRCChannel chan in server.GetChannels())
            {
                XElement channel = new XElement("channel");
                channel.Add(new XElement("name", chan.Channel)); channel.Add(new XElement("reconnect", chan.Reconnect));
                channels.Add(channel);
            }
            connection.Add(channels);

            XElement users = new XElement("users");

            int index = 0;
            foreach (KeyValuePair<string, UserAccess> access in server.Users)
            {
                XElement user = new XElement("user");
                if (index++ == server.Users.Count)
                {
                    break;
                }
                user.Add(new XElement("host", access.Key));
                user.Add(new XElement("access", UserAccessAttr.GetByValue(access.Value)));
                users.Add(user);
            }
            connection.Add(users);
            xd.Save(server.File);
            return true;

        }
    }


}
