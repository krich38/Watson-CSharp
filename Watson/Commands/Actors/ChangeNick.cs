using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson.Commands.Actors
{
    class ChangeNick : CommandActor
    {


        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if(msg.HasMessage())
            {
                string nick = msg.GetMessage().Split(' ')[0];
                server.SetAttemptNickChange(true);
                server.Write("NICK " + nick);
                server.Flush();
                
            }
        }        public UserAccess GetRequiredAccess()
        {
            return UserAccess.HALF_USER;
        }
    }
}
