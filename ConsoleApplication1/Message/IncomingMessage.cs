namespace ConsoleApplication1
{
    class IncomingMessage

    {
        private string raw;

        private string source;

        private string command;

        private string target;

        private string message;
        private string targetParams;
        private IRCServer server;

        public IncomingMessage(IRCServer server, string raw, string source, string command, string target, string message)
        {
            //Console.Write("CMD: " + command + ", target: " + target + ", MSG: " + message + ", source: " + source + "\n");
            this.server = server;
            this.raw = raw;
            this.source = source;
            this.command = command;
            if (target != null)
            {
                string[] dummy = target.Split(' ');

                this.target = dummy[0];
                this.targetParams = (dummy.Length == 2 ? dummy[1] : null);
            }
            else
            {
                this.target = this.targetParams = null;
            }
            if (message == null)
            {
                this.message = "";
            }
            else
            {
                this.message = message.Trim();
            }
        }

        public string GetCommand()
        {
            return command;
        }

        public string GetRaw()
        {
            return raw;
        }

        public IRCServer GetServer()
        {
            return server;
        }

        public string GetMessage()
        {
            return message;
        }

        public string GetTarget()
        {
            return target;
        }

        public string GetSource()
        {
            return source;
        }

        public bool HasMessage()
        {
            return message != null && message.Length > 0;
        }

        public bool IsDestChannel()
        {
            return GetCommand().Equals("PRIVMSG") && GetTarget().StartsWith("#");
        }

        public bool IsDestMe()
        {
            return GetCommand().Equals("PRIVMSG") && GetTarget().ToLower().Equals(server.GetNick().ToLower());
        }

        public void SendChat(string text)
        {
            GetServer().SendMessage(GetTarget(), text);
        }
    }
}
