using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Watson.Commands.Actors
{
    class UrbanDictionary : CommandActor
    {
        private const string URBAN_URL = "http://api.urbandictionary.com/v0/define?term=";
        private const int TIMEOUT = 10 * 60 * 1000; // length between queries in MS
        private const string LASTRESULT_PREFIX = "urban.lastresult.";
        private const string LASTQUERY_PREFIX = "urban.lastquery.";


        public UserAccess GetRequiredAccess()
        {
            return UserAccess.FULL_USER;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            if (!msg.HasMessage())
            {
                msg.SendChat(Help());
                return;
            }

            string term = msg.Message;
            long lastquery;
            try
            {
                lastquery = long.Parse(Database.GetKeyValue(msg.Server, LASTQUERY_PREFIX + term));
            }
            catch (Exception e)
            {
                lastquery = -1;
            }

            if (lastquery > 0 && Environment.TickCount - lastquery < TIMEOUT)
            {
                string lastdata = Database.GetKeyValue(msg.Server, LASTRESULT_PREFIX + term);
                if (lastdata != null && !lastdata.Equals(""))
                {
                    msg.SendChat(msg.Sender + ": " + lastdata);
                }
                else
                {
                    msg.SendChat(string.Format("{0}: There was an error last time retrieving the definition for {1}, please try again in {2} minute(s)", msg.Sender, term,
                            TimeSpan.FromMilliseconds(TIMEOUT - (Environment.TickCount - lastquery)).Minutes));
                }
            }
            else
            {
                string term_encoded;
                try
                {
                    term_encoded = Uri.EscapeUriString(term);
                }
                catch (Exception ex)
                {
                    msg.SendChat(msg.Sender + ": Error parsing location," + msg.Message);
                    return;
                }
                Database.SetKeyValue(msg.Server, LASTQUERY_PREFIX + term, Environment.TickCount);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URBAN_URL + term_encoded);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string responseContent;
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    responseContent = streamReader.ReadToEnd();
                }

                JObject json = JObject.Parse(responseContent);
                StringBuilder definition = new StringBuilder("Urban definition of `" + term + "`: ");

                try
                {
                    definition.Append(json["list"][0]["definition"].ToString());
                }
                catch (Exception e)
                {
                    msg.SendChat(msg.Sender + ": Error parsing urban data for, " + msg.Message);
                    return;
                }

                string formatted = definition.ToString();
                Database.SetKeyValue(msg.Server, LASTRESULT_PREFIX + term, formatted);
                msg.SendChat(msg.Sender + ": " + formatted);
            }
        }

        public string Help()
        {
            return "usage: urban [term]";
        }

        public UrbanDictionary()
        {
            Timer t = new Timer((obj) =>
            {
                Database.ExecuteUpdate("delete from keyvalues where key like (select 'urban.%.' || substr(key, 17) from keyvalues " +
                            "where key like '" + LASTQUERY_PREFIX + ".%' " +
                            "and value < " + (Environment.TickCount - TIMEOUT) + ")");

            }, null, 0, TIMEOUT * 2);
        }
    }
}
