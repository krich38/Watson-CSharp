using System;

namespace Watson
{
    class IncomingMessage
    {
        public string Raw
        {
            get; private set;
        }
        public string Source
        {
            get; private set;
        }
        public string Command
        {
            get; private set;
        }

        public string Target
        {
            get; private set;
        }
        public string Message
        {
            get; private set;
        }
        public string TargetParams
        {
            get; private set;
        }
        public string Sender
        {
            get; private set;
        }
        public string Host
        {
            get; private set;
        }
        public IRCServer Server
        {
            get; private set;
        }

        public IncomingMessage(IRCServer Server, string Raw, string Source, string Command, string Target, string Message)
        {
            this.Server = Server;
            this.Raw = Raw;
            this.Source = Source;
            this.Command = Command;
            if (Command.Equals("PRIVMSG") || Command.Equals("NICK"))
            {
                Sender = Source.Substring(0, Source.IndexOf("!"));
                Host = Source.Substring(Source.IndexOf("@") + 1);
            }
            if (Target != null)
            {
                string[] dummy = Target.Split(' ');

                this.Target = dummy[0];
                TargetParams = (dummy.Length == 2 ? dummy[1] : null);
            }
            else
            {
                this.Target = TargetParams = null;
            }
            if (Message == null)
            {
                this.Message = "";
            }
            else
            {
                this.Message = Message.Trim();
            }
        }

        public bool HasMessage()
        {
            return Message != null && Message.Length > 0;
        }

        public bool IsDestChannel()
        {
            return Command.Equals("PRIVMSG") && Target.StartsWith("#");
        }

        public bool IsDestMe()
        {
            return Command.Equals("PRIVMSG") && Target.Equals(Server.Nick, StringComparison.OrdinalIgnoreCase);
        }

        private string GetSendDest()
        {
            if (IsDestChannel())
            {
                return Target;
            }
            else
            {
                return Sender;
            }
        }

        public void SendChat(string text)
        {
            Server.SendMessage(GetSendDest(), text);
        }
    }
}
