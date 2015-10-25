using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Say : CommandActor
    {
        public UserAccess GetRequiredAccess()
        {
            return UserAccess.FULL_USER;
        }

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
        }
    }
}
