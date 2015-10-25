using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class ConnectedHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            Console.Write("Connected doe");
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            return msg.GetCommand().Equals("001") || msg.GetRaw().Contains("No more connections allowed from your host via this connect class");
        }
    }

}
