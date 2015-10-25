using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Settings
    {
        private String IP;
        private int PORT;
        public Settings(string IP, int PORT)
        {
            this.IP = IP;
            this.PORT = PORT;
        }

        public String getIp()
        {
            return this.IP;
        }

        public int getPort()
        {
            return this.PORT;
        }
    }
}
