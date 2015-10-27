namespace Watson
{
    interface MessageListener
    {
        void Handle(IncomingMessage msg);
        bool ShouldHandle(IncomingMessage msg);
    }
}
