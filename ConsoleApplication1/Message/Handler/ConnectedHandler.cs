namespace ConsoleApplication1
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
                    server.Write("JOIN " + chan.GetName());
                }
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
