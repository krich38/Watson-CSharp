using System.Text.RegularExpressions;

namespace Watson.Commands.Actors
{
    class Markov : CommandActor
    {
        private const string COMMAND_PATTERN = "(\\S+):?\\s*(\\S+)?(?: (\\S+))?";
        public const int REPLY_RATE = 1;
        public const int REPLY_NICK = 100;
        public Regex COMMAND_REGEX = new Regex(COMMAND_PATTERN);

        public UserAccess GetRequiredAccess()
        {
            return UserAccess.ANYONE;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage message)
        {
            if (!message.HasMessage())
            {
                // bad syntax
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
                        message.SendChat("Need context");
                    }

                    string markov = MarkovDatabaseAdapter.MarkovFind(m.Groups[2].Value, m.Groups[3].Value);
                    if (markov == null || markov.Equals(""))
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
