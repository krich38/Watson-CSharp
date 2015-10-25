namespace ConsoleApplication1
{
    class PingHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            msg.GetServer().Write("PONG " + msg.GetMessage());
            msg.GetServer().Flush();
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            return msg.GetCommand().Equals("PING");
        }
    }
}
