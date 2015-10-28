using System.Text.RegularExpressions;

namespace Watson
{
    class CommandListener : MessageListener
    {
        private const string COMMAND_PATTERN = "(\\S+?)(?:[,:]? (.+))?";

        private CommandManager commands;
        public Regex COMMAND_REGEX = new Regex(COMMAND_PATTERN);

        public void Handle(IncomingMessage msg)
        {
            string[] parts = msg.Message.Split(new char[] { ' ' }, 3);
            string command = parts[1];
            CommandActor cmd = commands.GetCommand(command);
            IncomingMessage newMessage;
            if (parts.Length > 2)
            {
                newMessage = new IncomingMessage(msg.Server, msg.Raw, msg.Source, msg.Command, msg.Target, parts[2]);
            }
            else
            {
                newMessage = new IncomingMessage(msg.Server, msg.Raw, msg.Source, msg.Command, msg.Target, null);
            }
            cmd.HandleCommand(msg.Server, command, newMessage);
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            Regex REGEX = new Regex("^" + msg.Server.Nick + "[:,]? .+");
            if ((msg.IsDestChannel() && REGEX.IsMatch(msg.Message)) || msg.IsDestMe())
            {
                string text = msg.Message.Substring(msg.Message.IndexOf(' ') + 1);
                if (COMMAND_REGEX.IsMatch(text))
                {
                    string command = text.Split(' ')[0].ToLower();
                    if (commands.HasCommand(command))
                    {
                        CommandActor commandActor = commands.GetCommand(command);
                        if (commandActor.GetRequiredAccess() != UserAccess.ANYONE)
                        {
                            string host = msg.Source.Split('@')[1];
                            UserAccess userRights = msg.Server.GetUserAccess(host);
                            if (UserAccessAttr.HasRequiredAccess(userRights, commandActor.GetRequiredAccess()))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
        }

        public CommandListener()
        {
            commands = CommandManager.getInstance();
        }
    }
}
