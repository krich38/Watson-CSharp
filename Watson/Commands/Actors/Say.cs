namespace Watson
{
    class Say : CommandActor
    {
        public void HandleCommand(IRCServer server, string command, IncomingMessage message)
        {
            if (message.HasMessage())
            {
                string msg = message.Message;
                string target = message.Target;
                string[] parts = message.Message.Split(new char[] { ' ' }, 2);
                if (parts[0].StartsWith("{") && parts[0].EndsWith("}") && parts.Length > 1)
                {
                    target = parts[0].Replace("{", "").Replace("}", "");
                    msg = parts[1];
                }

                server.SendMessage(target, msg);
            }
        }

        public UserAccess GetRequiredAccess()
        {
            return UserAccess.HALF_USER;
        }
    }
}
