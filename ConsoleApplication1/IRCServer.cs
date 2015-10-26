using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    class IRCServer
    {
        private string IP;
        private string nick, pass,altnick,realname, lastnick;
        private int PORT;
        private List<IRCChannel> channels;
        private ConnectionWorker worker;
        private bool ssl;
        private bool attemptingNick;

        public IRCServer(string IP, int PORT, List<IRCChannel> channels)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.channels = channels;
        }

        public IRCServer(string IP, int PORT, List<IRCChannel> channels, bool ssl, string nick, string pass, string altnick, string realname)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.channels = channels;
            this.ssl = ssl;
            this.nick = nick;
            this.pass = pass;
            this.altnick = altnick;
            this.realname = realname;

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

        public string getIp()
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

        public string GetNick()
        {
            return nick;
        }

        public string GetRealName()
        {
            return realname;
        }

        public void SetNick(string nick)
        {
            this.nick = nick;
        }

        public void SetAttemptNickChange(bool attemptingNick)
        {
            this.attemptingNick = attemptingNick;

            if(attemptingNick)
            {
                lastnick = nick;
            }
        }

        public string GetLastNick()
        {
            return lastnick;
        }

        public bool IsAttemptNickChange()
        {
            return attemptingNick;
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
