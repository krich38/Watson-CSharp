namespace Watson.Commands.Actors
{
    class ChangeNick : CommandActor
    {
        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if (msg.HasMessage())
            {
                string nick = msg.Message.Split(' ')[0];
                server.Nick = nick;
            }
        }

        public UserAccess GetRequiredAccess()
        {
            return UserAccess.HALF_USER;
        }
    }
}
