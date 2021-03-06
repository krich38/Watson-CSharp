﻿using System;

namespace Watson.Message
{
    class LoginListener : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            string[] parts = msg.Message.Split(' ');
            if (parts.Length == 4)
            {
                string text = msg.Message.Substring(msg.Message.IndexOf(parts[1]));
                parts = text.Split(' ');
                string user = parts[1];
                string pass = parts[2];
                UserAccess found = Database.AuthenticateUser(user, pass);
                if (found != UserAccess.ANYONE)
                {
                    //save here
                    msg.Server.Users[msg.Host] = found;
                    msg.SendChat("Thank you for authenticating, you have been granted " + found + " access.");
                    Configuration.Save(msg.Server);
                }
            }
            else
            {
                msg.SendChat("To login: login <username> <password>");
            }
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            if (msg.IsDestMe() && msg.Message.StartsWith(msg.Server.Nick))
            {
                string[] parts = msg.Message.Split(' ');

                if (parts[1].Equals("login", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

            }
            return false;
        }
    }
}
