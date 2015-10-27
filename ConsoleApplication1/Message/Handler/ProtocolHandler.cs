using System;

namespace ConsoleApplication1.Message.Handler
{
    class ProtocolHandler : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            IRCServer server = msg.GetServer();
            string ourNick = server.GetNick();

            if (msg.GetCommand().Equals("NICK"))
            {
                string whoChanging = msg.GetNick();
                string changingTo = msg.GetTarget();
                if (whoChanging.Equals(ourNick))
                {
                    server.SetNick(changingTo);
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

                                server.SetNick(server.GetLastNick());
                                server.SetAttemptNickChange(false);
                            }


                        }
                        else
                        {
                            string nick;
                            if(server.GetNick().Equals(server.GetAltNick()))
                            {
                                nick = server.GetAltNick() + 1;
                            } else
                            {
                                nick = server.GetAltNick();
                            }
                            server.SetNick(nick);
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
