using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Commands.Actors
{
    class Markov : CommandActor
    {
        //private static Pattern COMMAND_PATTERN = Pattern.compile("(\\S+):?\\s*(\\S+)?(?: (\\S+))?", Pattern.CASE_INSENSITIVE);

        public const int REPLY_RATE = 1;
        public const int REPLY_NICK = 100;
        public UserAccess GetRequiredAccess()
        {
            return UserAccess.
                ANYONE;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
