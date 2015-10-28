using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Watson.Commands.Actors
{
    class Weather : CommandActor
    {
        private const int TIMEOUT = 10 * 60 * 1000;
        private const string WEATHER_URL = "http://api.openweathermap.org/data/2.5/find?mode=json&units=metric&appid=53fa79458caa8d9eb2b2abb261003a19&q=";

        public UserAccess GetRequiredAccess()
        {
            return UserAccess.ANYONE;
        }

        public void HandleCommand(IRCServer server, string command, IncomingMessage msg)
        {
            try
            {
                string location;
                if (!msg.HasMessage())
                {
                    location = Database.GetKeyValue(msg.Server, "weather.lastlocation." + msg.Sender);
                    if (location == null)
                    {
                        msg.SendChat(msg.Sender + ": No previous location stored");
                        return;
                    }
                }
                else
                {
                    location = msg.Message;
                    Database.SetKeyValue(msg.Server, "weather.lastlocation." + msg.Sender, location);
                }

                long lastquery = -1;
                try
                {
                    lastquery = long.Parse(Database.GetKeyValue(msg.Server, "weather.lastquery." + location));
                }
                catch (Exception e)
                {
                    lastquery = -1;
                }

                if (lastquery > 0 && Environment.TickCount - lastquery < TIMEOUT)
                {
                    string lastdata = Database.GetKeyValue(msg.Server, "weather.lastdata." + location);
                    if (lastdata != null && !lastdata.Equals(""))
                    {
                        msg.SendChat(msg.Sender + ": " + lastdata);
                    }
                    else
                    {
                        // oh no
                    }
                }
                else
                {
                    string location_encoded = Uri.EscapeUriString(location);
                    Database.SetKeyValue(msg.Server, "weather.lastquery." + location, Environment.TickCount);
                    Console.WriteLine(WEATHER_URL + location_encoded);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WEATHER_URL + location_encoded);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string responseContent;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = streamReader.ReadToEnd();
                    }

                    JObject json = JObject.Parse(responseContent);

                    Database.SetKeyValue(msg.Server, "weather.lastquery." + location, Environment.TickCount);

                    JToken list = json["list"][0];
                    StringBuilder w = new StringBuilder("Weather for " + list["name"].ToString() + ", " + list["sys"]["country"].ToString() + ", ");

                    JToken weather = list["weather"][0];
                    string description = weather["description"].ToString();
                    if (description != null && !description.Equals(""))
                    {
                        w.Append(description).Append(", ");
                    }

                    JToken main = list["main"]; JToken wind = list["wind"];
                    w.Append(String.Format("Temp {0}c (min {1}c/max {2}c), {3}% Humidity, {4} hPa, {5}% Cloudy, Wind Speed {6}m/s",
                        main["temp"].ToString(),
                        main["temp_min"].ToString(),
                        main["temp_max"].ToString(),
                        main["humidity"].ToString(),
                        main["pressure"].ToString(),
                        list["clouds"]["all"].ToString(),
                        wind["speed"].ToString()));


                    JToken windGust = wind["gust"];
                    if (windGust != null)
                    {
                        w.Append(string.Format(" (gusting {0}m/s)", windGust.ToString()));
                    }

                    string formatted = w.ToString();
                    Database.SetKeyValue(msg.Server, "weather.lastdata." + location, formatted);
                    msg.SendChat(msg.Sender + ": " + formatted);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }

        public Weather()
        {
            Timer t = new Timer((obj) =>
            {
                Database.ExecuteUpdate("delete from keyvalues where key like (select 'weather.%.' || substr(key, 19) from keyvalues where key like 'weather.lastquery.%' and value < " + (Environment.TickCount - TIMEOUT) + ")");
            }, null, TIMEOUT * 2, Timeout.Infinite);
        }
    }
}
