using System;

namespace Watson.Message.Handler
{
    class ProtocolHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            IRCServer server = msg.Server;
            string ourNick = server.Nick;

            if (msg.Command.Equals("NICK"))
            {
                string whoChanging = msg.Sender;
                string changingTo = msg.Target;
                if (whoChanging.Equals(ourNick))
                {
                    server.Nick = changingTo;
                    server.SetAttemptNickChange(false);
                }
                else
                {
                    // update user nick list
                }
            }
            else if (msg.Command.Equals("ERROR"))
            {
                server.Dispose();
            }
            else
            {
                string raw = msg.Raw;
                switch (msg.Command)
                {
                    // nick change
                    case "433":
                        if (server.Connected)
                        {
                            // are we attempting a nick name change?
                            if (server.IsAttemptNickChange())
                            {

                                server.Nick = server.LastNick;
                                server.SetAttemptNickChange(false);
                            }


                        }
                        else
                        {
                            string nick;
                            if (server.Nick.Equals(server.AltNick))
                            {
                                nick = server.AltNick + 1;
                            }
                            else
                            {
                                nick = server.AltNick;
                            }
                            server.Nick = nick;

                        }
                        break;
                }
            }
        }

        public bool ShouldHandle(IncomingMessage msg)
        {

            return msg.Command.Equals("433") || msg.Command.Equals("353") || msg.Command.Equals("NICK") || msg.Command.Equals("ERROR");
        }
    }
}
