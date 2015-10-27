using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Commands.Actors
{
    class ChannelChange : CommandActor
    {


        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            try
            {
                if (msg.HasMessage())
                {
                    string channel = msg.GetMessage();
                    string partMessage = "Bye!";
                    if (command.Equals("part"))
                    {

                        Console.WriteLine(msg.GetMessage());
                        string[] parts;
                        if (msg.GetMessage().Contains(" "))
                        {
                            parts = msg.GetMessage().Split(new char[] { ' ' }, 2);
                        }
                        else
                        {
                            parts = new string[] { msg.GetMessage() };
                        }

                        if (parts.Length > 1)
                        {
                            if (parts[0].StartsWith("#"))
                            {
                                channel = parts[0];
                                partMessage = parts[1];
                            }
                            else
                            {
                                partMessage = msg.GetMessage();
                            }

                        }
                        server.PartChannel(channel, partMessage);
                    }
                    else
                    {  
                        server.JoinChannel(msg.GetMessage());
                      }
                }
                else
                {

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }        public UserAccess GetRequiredAccess()
        {
            return UserAccess.HALF_USER;
        }
    }
}
