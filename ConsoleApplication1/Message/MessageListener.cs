namespace ConsoleApplication1
{
    interface MessageListener
    {
        void Handle(IncomingMessage msg);
        bool ShouldHandle(IncomingMessage msg);
    }
}
