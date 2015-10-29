namespace Watson.Commands.Actors
{
    class ChannelChange : CommandActor
    {
        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if (msg.HasMessage())
            {
                string channel = msg.Message;
                string partMessage = "Bye!";
                if (command.Equals("part"))
                {
                    string[] parts;
                    if (msg.Message.Contains(" "))
                    {
                        parts = msg.Message.Split(new char[] { ' ' }, 2);
                    }
                    else
                    {
                        parts = new string[] { msg.Message };
                    }

                    if (parts.Length > 1)
                    {
                        if (parts[0].StartsWith("#"))
                        {
                            channel = parts[0];
                            partMessage = parts[1];
                        }
                        else
                        {
                            partMessage = msg.Message;
                        }

                    }
                    server.PartChannel(channel, partMessage);
                }
                else
                {
                    server.JoinChannel(msg.Message);
                }
            }
            else
            {
                msg.SendChat(Help());
            }
        }

        public UserAccess GetRequiredAccess()
        {
            return UserAccess.HALF_USER;
        }

        public string Help()
        {
            return "usage: [part|join #channel <part message here>";
        }
    }
}
