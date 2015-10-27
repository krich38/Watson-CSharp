using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson
{
    class Say : CommandActor
    {


        public void HandleCommand(IRCServer server, string command, IncomingMessage message)
        {
            if (message.HasMessage())
            {
                string msg = message.GetMessage();
                string target = message.GetTarget();
                string[] parts = message.GetMessage().Split(new char[] { ' ' }, 2);
                if (parts[0].StartsWith("{") && parts[0].EndsWith("}") && parts.Length > 1)
                {
                    target = parts[0].Replace("{", "").Replace("}", "");
                    msg = parts[1];
                }
                
                server.SendMessage(target, msg);
            }
        }        public UserAccess GetRequiredAccess()
        {
            return UserAccess.HALF_USER;
        }
    }
}
