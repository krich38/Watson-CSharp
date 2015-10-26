namespace ConsoleApplication1.Message.Handler
{
    class ProtocolHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            IRCServer server = msg.GetServer();
            string ourNick = server.GetNick();

            if (msg.GetCommand().Equals("NICK"))
            {
                string whoChanging = msg.GetNick();
                string changingTo = msg.GetTarget();
                if (whoChanging.Equals(ourNick))
                {
                    msg.GetServer().SetNick(changingTo);
                }
                else
                {
                    // update user nick list
                }
            }
            else
            {
                switch (msg.GetCommand())
                {
                    // nick change
                    case "433":
                        // are we attempting a nick name change?
                        if (server.IsAttemptNickChange())
                        {
                            server.SetNick(server.GetLastNick());
                            server.SetAttemptNickChange(false);
                        }
                        else
                        {
                            // someone else is changing their nick
                        }
                        break;
                }
            }

        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            return msg.GetCommand().Equals("433") || msg.GetCommand().Equals("NICK");
        }
    }
}
