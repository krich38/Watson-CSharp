using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    class IRCServer
    {
        private String IP;
        private int PORT;
        private List<IRCChannel> channels;
        private ConnectionWorker worker;
        private bool ssl;

        public IRCServer(string IP, int PORT, List<IRCChannel> channels)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.channels = channels;
        }

        public IRCServer(string IP, int PORT, List<IRCChannel> channels, bool ssl)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.channels = channels;
            this.ssl = ssl;

        }

        public void Write(string text)
        {

            if (!text.EndsWith("\r\n"))
            {
                text = text + "\r\n";
            }
            worker.Write(text);
        }

        public void SendMessage(string target, string msg)
        {
            Write("PRIVMSG " + target + " :" + msg);
            Flush();
        }

        public void Flush()
        {
            worker.Flush();
        }

        public String getIp()
        {
            return this.IP;
        }

        public int getPort()
        {
            return this.PORT;
        }

        public List<IRCChannel> GetChannels()
        {
            return channels;
        }

        public void SetWorker(ConnectionWorker worker)
        {
            this.worker = worker;
        }

        public bool IsSSL()
        {
            return ssl;
        }
    }

    class IRCChannel
    {
        private string channel;
        private bool reconnect;

        public IRCChannel(string channel, bool reconnect)
        {
            this.channel = channel;
            this.reconnect = reconnect;
        }

        public string GetName()
        {
            return channel;
        }
    }
}
