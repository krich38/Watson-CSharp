using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Commands.Actors
{
    class Calculate : CommandActor
    {


        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if(msg.HasMessage())
            {
                Expression e = new Expression(msg.GetMessage());
                msg.SendChat(e.Evaluate().ToString());
            }
        }        public UserAccess GetRequiredAccess()
        {
            return UserAccess.ANYONE;
        }
    }
}
