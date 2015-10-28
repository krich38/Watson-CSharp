namespace Watson
{
    class PingHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            msg.Server.Write("PONG " + msg.Message);
            msg.Server.Flush();
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            return msg.Command.Equals("PING");
        }
    }
}
