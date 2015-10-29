namespace Watson
{
    interface CommandActor
    {
        UserAccess GetRequiredAccess();
        void HandleCommand(IRCServer server, string command, IncomingMessage msg);
        string Help();
    }
}
