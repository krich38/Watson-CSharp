using System;

namespace Watson.Message.Handler
{
    class ProtocolHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            IRCServer server = msg.GetServer();
            string ourNick = server.Nick;

            if (msg.GetCommand().Equals("NICK"))
            {
                string whoChanging = msg.GetNick();
                string changingTo = msg.GetTarget();
                if (whoChanging.Equals(ourNick))
                {
                    server.Nick = changingTo;
                    server.SetAttemptNickChange(false);
                }
                else
                {
                    // update user nick list
                }
            } else if(msg.GetCommand().Equals("ERROR"))
            {
                server.Dispose();
            }
            else
            {
                string raw = msg.GetRaw();
                switch (msg.GetCommand())
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
                            if(server.Nick.Equals(server.AltNick))
                            {
                                nick = server.AltNick + 1;
                            } else
                            {
                                nick = server.AltNick;
                            }
                            server.Nick = nick; 
                            server.Write("NICK " + nick);
                            server.Flush();
                           
                        }
                        break;



                }
            }
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            
            return msg.GetCommand().Equals("433") || msg.GetCommand().Equals("353") || msg.GetCommand().Equals("NICK") || msg.GetCommand().Equals("ERROR");
        }
    }
}
