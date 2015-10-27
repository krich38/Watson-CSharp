using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1.Commands.Actors
{
    class Markov : CommandActor
    {
        //private static Pattern COMMAND_PATTERN = Pattern.compile("(\\S+):?\\s*(\\S+)?(?: (\\S+))?", Pattern.CASE_INSENSITIVE);

        public const int REPLY_RATE = 1;
        public const int REPLY_NICK = 100;
        public Regex COMMAND_REGEX = new Regex("(\\S+):?\\s*(\\S+)?(?: (\\S+))?");
        public UserAccess GetRequiredAccess()
        {
            return UserAccess.
                ANYONE;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage message)
        {
            if (!message.HasMessage())
            {
                //message.sendChatf(getHelp());
                return;
            }


            if (COMMAND_REGEX.IsMatch(message.GetMessage()))
            {

                Match m = COMMAND_REGEX.Match(message.GetMessage());
                string cmd = m.Groups[1].Value;

                if (cmd.Equals("about"))
                {
                    if (m.Groups[2].Value == null || m.Groups[2].Value.Equals(""))
                    {
                        //message.sendChat("Need context");
                    }


                    String markov = MarkovDatabaseAdapter.MarkovFind(m.Groups[2].Value, m.Groups[3].Value);
                    if (markov == null)
                    {
                        message.SendChat("I can't :(");
                    }
                    else
                    {
                        message.SendChat(markov);

                    }

                }

            }
        }
    }
}
