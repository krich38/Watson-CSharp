using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class IRCServer
    {
        private String IP;
        private int PORT;
        private string[] channels;
        private ConnectionWorker worker;

        public IRCServer(string IP, int PORT, string[] channels)
        {
            this.IP = IP;
            this.PORT = PORT;
            this.channels = channels;
        }

        public void Write(string text)
        {
            if(!text.EndsWith("\r\n"))
            {
                text = text + "\r\n";
            }
            worker.Write(text);
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
    }
}
