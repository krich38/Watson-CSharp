using System.Text.RegularExpressions;
using System.Threading;

namespace Watson.Message
{
    class SedListener : MessageListener
    {
        private const string SED_PATTERN = "^s/(.*?)/(.*?)/(g?)";
        private const string LASTMESSAGE_PREFIX = "sed.lastmessage.";
        private const int TIMEOUT = 60 * 60 * 1000;

        private Regex SED_REGEX = new Regex(SED_PATTERN);
        public void Handle(IncomingMessage msg)
        {
            if (!msg.IsDestChannel() || !msg.HasMessage())
            {
                return;
            }
            if (SED_REGEX.IsMatch(msg.Message))
            {
                string lastmessage = Database.GetKeyValue(msg.Server, LASTMESSAGE_PREFIX + msg.Sender);
                if (lastmessage == null || lastmessage.Equals(""))
                {
                    // oh no
                    return;
                }

                string reply;
                Match m = SED_REGEX.Match(msg.Message);
                if (m.Groups[3] == null || m.Groups[3].Value.Equals(""))
                {
                    reply = ReplaceFirst(lastmessage, m.Groups[1].Value, m.Groups[2].Value);
                }
                else if (m.Groups[3] != null && m.Groups[3].Value.Equals("g"))
                {
                    reply = lastmessage.Replace(m.Groups[1].Value, m.Groups[2].Value);
                }
                else
                {
                    msg.SendChat(msg.Sender + ": You did something wrong...");
                    return;
                }
                msg.SendChat(msg.Sender + " meant: " + reply);
            }
            else
            {
                Database.SetKeyValue(msg.Server, LASTMESSAGE_PREFIX + msg.Sender, msg.Message);
            }
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            return msg.Command.Equals("PRIVMSG");
        }

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public SedListener()
        {
            Timer t = new Timer((obj) =>
            {
                Database.ExecuteUpdate("delete from keyvalues where key like '" + LASTMESSAGE_PREFIX + "%'");
            }, null, 0, TIMEOUT);
        }
    }
}
