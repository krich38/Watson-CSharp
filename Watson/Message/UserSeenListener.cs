using System;

namespace Watson.Message
{
    class UserSeenListener : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            string lastSeenEvent = null;

            if (msg.Command.Equals("PRIVMSG"))
            {
                lastSeenEvent = "saying: " + msg.Message;
            }
            else if (msg.Command.Equals("JOIN"))
            {
                lastSeenEvent = "joining " + msg.Message;
            }
            else if (msg.Command.Equals("PART"))
            {
                lastSeenEvent = "leaving " + msg.TargetParams + ": " + msg.Message;
            }
            else if (msg.Command.Equals("QUIT"))
            {
                lastSeenEvent = "quitting: " + msg.Message;
            }
            else if (msg.Command.Equals("NICK"))
            {
                lastSeenEvent = "changing nick to: " + msg.TargetParams;
            }
            if (lastSeenEvent != null)
            {
                string nick = msg.Sender.ToLower();
                Database.SetKeyValue(msg.Server, "lastseen.time." + nick, Environment.TickCount);
                Database.SetKeyValue(msg.Server, "lastseen.event." + nick, lastSeenEvent);
            }
        }

        public bool ShouldHandle(IncomingMessage message)
        {
            return (message.Command.Equals("PRIVMSG") || message.Command.Equals("JOIN") || message.Command.Equals("PART") || message.Command.Equals("QUIT") || message.Command.Equals("NICK"));

        }
    }
}
