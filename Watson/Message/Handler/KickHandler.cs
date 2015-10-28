using System.Threading;

namespace Watson.Message.Handler
{
    class KickHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            IRCServer server = msg.Server;
            string target = msg.Target;

            if (server.GetChannel(target).Reconnect)
            {
                Timer t = new Timer((obj) =>
                            {
                                server.Write("JOIN " + target);
                                server.Flush();
                            }, null, 5000, 0);
            }
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            string nick = msg.TargetParams;
            if (nick == null)
            {
                return false;
            }

            if (msg.Command.Equals("KICK") && nick.Equals(msg.Server.Nick))
            {
                return true;
            }
            return false;
        }
    }
}
