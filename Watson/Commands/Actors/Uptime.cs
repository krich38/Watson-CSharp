using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson.Commands.Actors
{
    class Uptime : CommandActor
    {
        public static DateTime BEGIN = DateTime.Now;
        public UserAccess GetRequiredAccess()
        {
            return UserAccess.
                ANYONE;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            var delta = DateTime.Now - BEGIN;
            var seconds = delta.Seconds.ToString("n0");
            var minutes = Math.Floor(delta.TotalMinutes).ToString("n0");
            var hours = Math.Floor(delta.TotalHours).ToString("n0");
            var days = Math.Floor(delta.TotalDays).ToString("n0");
            string reply = "";
            if (!seconds.Equals("0"))
            {
                if (minutes.Equals("1"))
                {
                    reply = seconds + " second.";
                }
                else
                {
                    reply = seconds + " seconds.";
                }
            }

            if (!minutes.Equals("0"))
            {
                if (minutes.Equals("1"))
                {
                    reply = minutes + " minute, " + reply;
                }
                else
                {
                    reply = minutes + " minutes, " + reply;
                }
            }

            if (!hours.Equals("0"))
            {
                if (hours.Equals("1"))
                {
                    reply = hours + " hour, " + reply;
                }
                else
                {
                    reply = hours + " hours, " + reply;
                }
            }

            if (!days.Equals("0"))
            {
                if (days.Equals("1"))
                {
                    reply = days + " day, " + reply;
                }
                else
                {
                    reply = days + " days, " + reply;
                }
            }
            msg.SendChat("Current uptime: " + reply);
        }
    }
}
