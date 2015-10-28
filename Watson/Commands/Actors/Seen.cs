using System;

namespace Watson.Commands.Actors
{
    class Seen : CommandActor
    {
        public UserAccess GetRequiredAccess()
        {
            return UserAccess.ANYONE;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if (!msg.HasMessage())
            {
                // oh no
                return;
            }
            string nick = msg.Message.ToLower().Split(new char[] { ' ' }, 2)[0];
            string lastSeenEvent = Database.GetKeyValue(server, "lastseen.event." + nick);
            if (lastSeenEvent == null)
            {
                msg.SendChat("Have not seen " + nick);
            }
            else
            {
                TimeSpan t = TimeSpan.FromMilliseconds(Environment.TickCount - long.Parse(Database.GetKeyValue(msg.Server, "lastseen.time." + nick)));
                string answer = string.Format("{0:D2} hours, {1:D2} minutes, {2:D2} seconds",
                                        t.Hours,
                                        t.Minutes,
                                        t.Seconds);
                msg.SendChat(string.Format("Last seen {0}: {1} ago, {2}", nick, answer, lastSeenEvent));
            }
        }
    }
}
