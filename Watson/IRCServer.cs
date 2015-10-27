using System;
using System.Collections.Generic;

namespace Watson
{
    class IRCServer
    {
        public string IP
        {
            get; private set;
        }
        public int PORT
        {
            get; private set;
        }
        private string _Nick;
        public string Nick
        {
            get { return _Nick; }
            set
            {
                if(Connected)
                {
                    _Nick = value;
                    SetAttemptNickChange(true);
                    Write("NICK " + _Nick);
                    Flush();
                }
            }
        }
        public string AltNick
        {
            get; private set;
        }

        public string RealName
        {
            get; private set;
        }

        public string LastNick
        {
            get; private set;
        }
        public string Pass
        {
            get; private set;
        }
        public bool SSL
        {
            get; private set;
        }
        public bool Connected
        {
            get; set;
        }
        private List<IRCChannel> channels;
        private ConnectionWorker worker;

        private bool attemptingNick;



        public IRCServer(string IP, int PORT, List<IRCChannel> channels)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.channels = channels;
        }

        public IRCServer(string IP, int PORT, List<IRCChannel> channels, bool SSL, string Nick, string Pass, string AltNick, string RealName)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.channels = channels;
            this.SSL = SSL;
            this.Nick = Nick;
            this.Pass = Pass;
            this.AltNick = AltNick;
            this.RealName = RealName;

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


        public IRCChannel GetChannel(string name)
        {
            foreach (IRCChannel chan in channels)
            {
                if (chan.Channel.Equals(name))
                {
                    return chan;
                }
            }

            return null;
        }

        public void PartChannel(string chan, string msg)
        {
            channels.Remove(GetChannel(chan));
            Write("PART " + chan + " :" + msg);
            Flush();
        }

        public void JoinChannel(string chan)
        {
            Write("JOIN " + chan);
            Flush();
            channels.Add(new IRCChannel(chan, true));
        }

        public List<IRCChannel> GetChannels()
        {
            return channels;
        }

        public void SetWorker(ConnectionWorker worker)
        {
            this.worker = worker;
        }

      
        public void Dispose()
        {
            // do all parts etc here
            worker.Stop();
            Program.GetInstance().OnDispose(this);
        }

        public void SetAttemptNickChange(bool attemptingNick)
        {
            this.attemptingNick = attemptingNick;

            if (attemptingNick)
            {
                LastNick = Nick;
            }
        }

        public bool IsAttemptNickChange()
        {
            return attemptingNick;
        }

        override
        public string ToString()
        {
            return IP + ":" + PORT;
        }
    }

    class IRCChannel
    {
        public string Channel {
            get; private set;
        }
        public bool Reconnect
        {
            get; set;
        }

        public IRCChannel(string Channel, bool Reconnect)
        {
            this.Channel = Channel;
            this.Reconnect = Reconnect;
        }
    }
}
