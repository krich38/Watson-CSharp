using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Commands.Actors
{
    class Kill : CommandActor
    {
        public UserAccess GetRequiredAccess()
        {
            return UserAccess.ANYONE;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if(msg.HasMessage())
            {
                server.Write("QUIT :" + msg.GetMessage());
            } else
            {
                server.Write("QUIT :Bye!");
            }
            server.Flush();

            server.Dispose();

        }
    }
}
