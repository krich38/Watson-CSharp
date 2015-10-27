using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Watson.Message.Handler
{
    class KickHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            IRCServer server = msg.GetServer();
            string target = msg.GetTarget();


            if (server.GetChannel(target).Reconnect)
            {
                Timer t = new Timer((obj) =>
                            {
                                server.Write("JOIN " + target);
                                server.Flush();
                            }, null, 5000, 0);
            }
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            string nick = msg.GetTargetParams();
            if (nick == null)
            {
                return false;
            }

            if (msg.GetCommand().Equals("KICK") && nick.Equals(msg.GetServer().Nick))
            {
                return true;
            }
            return false;
        }
    }
}
