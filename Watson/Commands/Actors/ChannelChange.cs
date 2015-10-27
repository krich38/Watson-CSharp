namespace Watson.Commands.Actors
{
    class ChannelChange : CommandActor
    {
        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if (msg.HasMessage())
            {
                string channel = msg.GetMessage();
                string partMessage = "Bye!";
                if (command.Equals("part"))
                {
                    string[] parts;
                    if (msg.GetMessage().Contains(" "))
                    {
                        parts = msg.GetMessage().Split(new char[] { ' ' }, 2);
                    }
                    else
                    {
                        parts = new string[] { msg.GetMessage() };
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
                            partMessage = msg.GetMessage();
                        }

                    }
                    server.PartChannel(channel, partMessage);
                }
                else
                {
                    server.JoinChannel(msg.GetMessage());
                }
            }
            else
            {
                msg.SendChat("Incorrect syntax! Usage: join #example       OR     part #example <message here>");
            }
        }

        public UserAccess GetRequiredAccess()
        {
            return UserAccess.HALF_USER;
        }
    }
}
