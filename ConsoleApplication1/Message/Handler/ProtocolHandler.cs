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
                    msg.GetServer().SetNick(changingTo);
                }
                else
                {
                    // update user nick list
                }
            }
            else
            {
                switch (msg.GetCommand())
                {
                    // nick change
                    case "433":
                        // are we attempting a nick name change?
                        if (server.IsAttemptNickChange())
                        {
                            server.SetNick(server.GetLastNick());
                            server.SetAttemptNickChange(false);
                        }
                        else
                        {
                            // someone else is changing their nick
                        }
                        break;
                        // names
                    case "353":
                        try {
                            //Console.WriteLine(msg.GetRaw());
                            string chan = msg.GetRaw().Split(' ')[4];
                            
                            string[] users = msg.GetMessage().Split(' ');
                            IRCChannel channel = server.GetChannel(chan);
                            foreach (string user in users)
                            {


                                switch (IRCChannel.GetChannelAccessByCode(user[0])) {
                                    case IRCChannel.ChannelAccess.REGULAR_USER:
                                        channel.UpdateUsers(user, 0);
                                        break;
                                    case IRCChannel.ChannelAccess.VOICE:
                                        channel.UpdateUsers(user.Substring(1), 1);
                                        break;
                                    case IRCChannel.ChannelAccess.HALF_OPERATOR:
                                        channel.UpdateUsers(user.Substring(1), 2);
                                        break;
                                    case IRCChannel.ChannelAccess.OPERATOR:
                                        channel.UpdateUsers(user.Substring(1), 3);
                                        break;
                                    case IRCChannel.ChannelAccess.ADMIN:
                                        channel.UpdateUsers(user.Substring(1), 4);
                                        break;
                                    case IRCChannel.ChannelAccess.OWNER:
                                        channel.UpdateUsers(user.Substring(1), 5);
                                        break;

                                }
                            }
                            Console.WriteLine(string.Join(";", channel.GetUsers()));
                        } catch(Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;

                }
            }

        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            
            return msg.GetCommand().Equals("433") || msg.GetCommand().Equals("353") || msg.GetCommand().Equals("NICK");
        }
    }
}
