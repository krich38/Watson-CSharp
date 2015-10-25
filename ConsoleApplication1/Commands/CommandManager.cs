﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class CommandManager
    {
        private Dictionary<string, CommandActor> COMMANDS = new Dictionary<string, CommandActor>();
        private static CommandManager INSTANCE;
        public CommandManager()
        {
            COMMANDS.Add("say", new Say());

            }

        public static CommandManager getInstance()
        {
            if(INSTANCE == null)
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
    }
}

