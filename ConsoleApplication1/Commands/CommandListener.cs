using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class CommandListener : MessageListener
    {
        private CommandManager commands;
        private const string USER_PATTERN = "^Watso[:,]? .+";
        private const string COMMAND_PATTERN = "(\\S+?)(?:[,:]? (.+))?";
        public Regex REGEX = new Regex(USER_PATTERN);
        public Regex COMMAND_REGEX = new Regex(COMMAND_PATTERN);
        public void Handle(IncomingMessage msg)
        {
            string[] parts = msg.GetMessage().Split(new char[] { ' ' }, 3);
            string command = parts[1];
            CommandActor cmd = commands.GetCommand(command);
            IncomingMessage newMessage;
            if (parts.Length > 2)
            {
                newMessage = new IncomingMessage(msg.GetServer(), msg.GetRaw(), msg.GetSource(), msg.GetCommand(), msg.GetTarget(), parts[2]);
            }
            else
            {
                newMessage = new IncomingMessage(msg.GetServer(), msg.GetRaw(), msg.GetSource(), msg.GetCommand(), msg.GetTarget(), null);
            }
            cmd.HandleCommand(msg.GetServer(), command, newMessage);
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            
            if ((msg.IsDestChannel() && REGEX.IsMatch(msg.GetMessage())) || msg.IsDestMe())
            {
                string text = msg.GetMessage().Substring(msg.GetMessage().IndexOf(' ') + 1);
                if (COMMAND_REGEX.IsMatch(text))
                {
                    string command = text.Split(' ')[0];
                    if (commands.HasCommand(command))
                    {
                        CommandActor commandActor = commands.GetCommand(command);
  
                            string host = msg.GetSource().Split('@')[1];

                                    return true;
                                
                
                    }
                }
            }
            return false;
        }

        public CommandListener()
        {
            commands = CommandManager.getInstance();
        }
    }
}
