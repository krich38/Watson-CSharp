using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watson.Message
{
    class LoginListener : MessageListener
    {
        public void Handle(IncomingMessage msg)
        {
            string[] parts = msg.GetMessage().Split(' ');
            Console.WriteLine(msg.GetMessage());
            if (parts.Length == 4)
            {
                string text = msg.GetMessage().Substring(msg.GetMessage().IndexOf(parts[1]));
                parts = text.Split(' ');
                string user = parts[1];
                string pass = parts[2];
                UserAccess found = Database.AuthenticateUser(user, pass);
                if (found != UserAccess.ANYONE)
                {
                    //msg.GetServer().getUserProperties().getUsers().put(msg.getHostName(), found);
                    //Watson.getInstance().save();
                    msg.SendChat("Thank you for authenticating, you have been granted " + found + " access.");

                }

            }
            else
            {
                msg.SendChat("To login: login <username> <password>");
            }
        }

        public bool ShouldHandle(IncomingMessage msg)
        {
            if (msg.IsDestMe() && msg.GetMessage().StartsWith(msg.GetServer().Nick))
            {
                string[] parts = msg.GetMessage().Split(' ');

                if (parts[1].Equals("login", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

            }
            return false;
        }
    }
}
