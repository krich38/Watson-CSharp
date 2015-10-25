﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string GetSource()
        {
            return source;
        }
    }
}
