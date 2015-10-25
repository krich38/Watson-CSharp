using System;

namespace ConsoleApplication1
{
    class IRCServer
    {
        private String IP;
        private int PORT;
        private string[] channels;
        private ConnectionWorker worker;
        private bool ssl;

        public IRCServer(string IP, int PORT, string[] channels)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.channels = channels;
        }

        public IRCServer(string IP, int PORT, string[] channels, bool ssl)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.channels = channels;
            this.ssl = ssl;

        }

        public void Write(string text)
        {
            
            if(!text.EndsWith("\r\n"))
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

        public string[] GetChannels()
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
}
