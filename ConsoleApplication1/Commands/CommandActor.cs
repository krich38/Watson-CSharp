using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    interface CommandActor
    {
        UserAccess GetRequiredAccess();
        void HandleCommand(IRCServer server, string command, IncomingMessage msg);
    }
}
