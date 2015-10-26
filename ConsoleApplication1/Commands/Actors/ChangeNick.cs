using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Commands.Actors
{
    class ChangeNick : CommandActor
    {
        public UserAccess GetRequiredAccess()
        {
            return UserAccess.FULL_USER;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if(msg.HasMessage())
            {
                string nick = msg.GetMessage().Split(' ')[0];
                server.SetAttemptNickChange(true);
                server.Write("NICK " + nick);
                server.Flush();
                
            }
        }
    }
}
