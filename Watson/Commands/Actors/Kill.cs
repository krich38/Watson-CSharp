namespace Watson.Commands.Actors
{
    class Kill : CommandActor
    {
        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if (msg.HasMessage())
            {
                server.Write("QUIT :" + msg.Message);
            }
            else
            {
                server.Write("QUIT :Bye!");
            }
            server.Flush();
            server.Dispose();
        }

        public UserAccess GetRequiredAccess()
        {
            return UserAccess.FULL_USER;
        }

        public string Help()
        {
            return "usage: kill <quit message here>";
        }
    }
}
