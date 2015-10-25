namespace ConsoleApplication1
{
    class ConnectedHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            if (msg.GetCommand().Equals("001"))
            {
                foreach (string chan in msg.GetServer().GetChannels())
                {
                    msg.GetServer().Write("JOIN " + chan);
                }
                msg.GetServer().Flush();
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
