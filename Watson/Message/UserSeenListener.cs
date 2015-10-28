using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson.Message
{
    class UserSeenListener : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            
        }

        public bool ShouldHandle(IncomingMessage message)
        {
            return (message.Command.Equals("PRIVMSG") || message.Command.Equals("JOIN") || message.Command.Equals("PART") || message.Command.Equals("QUIT") || message.Command.Equals("NICK"));
           
        }
    }
}
