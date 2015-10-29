using Watson.Commands.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Watson
{
    class CommandManager
    {
        private Dictionary<string, CommandActor> COMMANDS = new Dictionary<string, CommandActor>();
        public static Random RANDOM = new Random();
        private static CommandManager INSTANCE;

        public CommandManager()
        {
            COMMANDS.Add("say", new Say());
            COMMANDS.Add("chat", new Markov());
            COMMANDS.Add("nick", new ChangeNick());
            COMMANDS.Add("kill", new Kill());
            COMMANDS.Add("uptime", new Uptime());
            COMMANDS.Add("part", new ChannelChange());
            COMMANDS.Add("join", new ChannelChange());
            COMMANDS.Add("calculate", new Calculate());
            COMMANDS.Add("weather", new Weather());
            COMMANDS.Add("seen", new Seen());
            COMMANDS.Add("help", new Help());

        }

        public static CommandManager GetInstance()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new CommandManager();
            }
            return INSTANCE;
        }

        public bool HasCommand(string command)
        {
            return COMMANDS.ContainsKey(command);
        }

        public CommandActor GetCommand(string command)
        {
            return COMMANDS[command];
        }

        public string AvailableCommands()
        {
            StringBuilder cmds = new StringBuilder();
            foreach(string s in COMMANDS.Keys)
            {
                if(cmds.Length >0)
                {
                    cmds.Append(", ");
                }
                cmds.Append(s.ToLower());
            }
            return cmds.ToString();
        }
    }
}

