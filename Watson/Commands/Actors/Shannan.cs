using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson.Commands.Actors
{
    class Shannan : CommandActor
    {
        private string[] sayings = new string[]
        {
            "I want to meet Shannan, will she join the chat and talk to me?",
            
        };
        public UserAccess GetRequiredAccess()
        {
            return UserAccess.ANYONE;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            int i = new Random().Next(0,sayings.Length-1);
            string saying = sayings[i];
            msg.SendChat(saying);
        }

        public string Help()
        {
            throw new NotImplementedException();
        }
    }
}
