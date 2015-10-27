namespace Watson
{
    class ConnectedHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            IRCServer server = msg.GetServer();
            if (msg.GetCommand().Equals("001"))
            {
                foreach (IRCChannel chan in server.GetChannels())
                {
                    server.Write("JOIN " + chan.Channel);
                }

                server.Write("PRIVMSG NICKSERV :IDENTIFY " + server.Pass);
                server.Flush();
                server.Connected = true;
            }
            else if (msg.GetRaw().Contains("No more connections allowed from your host via this connect class"))
            {
                // todo: set reconnect off here
            }
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            //Console.Write(msg.GetCommand());
            return msg.GetCommand().Equals("001") || msg.GetRaw().Contains("No more connections allowed from your host via this connect class");
        }
    }

}
