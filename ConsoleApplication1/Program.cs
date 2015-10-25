using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        private static volatile Program INSTANCE;


        static void Main(string[] args)
        {Program program = new Program();
            INSTANCE = program; 
            
            Settings server = new Settings("irc.freenode.net", 6667);
            program.Connect(server);
        }

        private void Connect(Settings settings)
        {
            ConnectionWorker worker = new ConnectionWorker(settings.getIp(), settings.getPort());
            Thread t = new Thread(worker.Process);
            t.Start();
        }

        internal bool IsRunning()
        {
            return true;
        }

        public static Program GetInstance()
        {
           
                return INSTANCE;
            
        }

    }
}
