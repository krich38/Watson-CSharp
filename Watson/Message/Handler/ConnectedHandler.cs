namespace Watson
{
    class ConnectedHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            IRCServer server = msg.Server;
            if (msg.Command.Equals("001"))
            {
                foreach (IRCChannel chan in server.GetChannels())
                {
                    server.Write("JOIN " + chan.Channel);
                }

                server.Write("PRIVMSG NICKSERV :IDENTIFY " + server.Pass);
                server.Flush();
                server.Connected = true;
            }
            else if (msg.Raw.Contains("No more connections allowed from your host via this connect class"))
            {
                server.Dispose();
            }
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            return msg.Command.Equals("001") || msg.Raw.Contains("No more connections allowed from your host via this connect class");
        }
    }

}
