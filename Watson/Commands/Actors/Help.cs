namespace Watson.Commands.Actors
{
    class Help : CommandActor
    {

        public UserAccess GetRequiredAccess()
        {
            return UserAccess.ANYONE;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            CommandManager commands = CommandManager.GetInstance();
            if (!msg.HasMessage())
            {

                msg.SendChat("Available commands: " + commands.AvailableCommands());
                return;
            }

            string cmd = msg.Message.ToLower();
            if (!commands.HasCommand(cmd))
            {
                msg.SendChat("No help for command: " + cmd);
            }
            else
            {
                string help = commands.GetCommand(cmd).Help();
                if (help == null || help.Equals(""))
                {
                    msg.SendChat("No help available for: " + cmd);
                }
                else
                {
                    msg.SendChat("Help for " + cmd + ": " + help);
                }
            }

        }

        string CommandActor.Help()
        {
            return "you're an idiot";
        }
    }
}
