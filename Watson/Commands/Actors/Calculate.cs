using NCalc;

namespace Watson.Commands.Actors
{
    class Calculate : CommandActor
    {
        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if (msg.HasMessage())
            {
                Expression e = new Expression(msg.GetMessage());
                msg.SendChat(e.Evaluate().ToString());
            }
        }

        public UserAccess GetRequiredAccess()
        {
            return UserAccess.ANYONE;
        }
    }
}
