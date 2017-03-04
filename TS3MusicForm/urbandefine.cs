using System.Net;
using Newtonsoft.Json;

namespace TS3MusicBot
{
    public class urbandefine
    {
        public static string Response(string term)
        {
            string url = "https://mashape-community-urban-dictionary.p.mashape.com/define?term=" + term;
            string definition = "";
            using (WebClient WC = new WebClient())
            {
                WC.Headers.Add("X-Mashape-Key", "3vwh66sb7amshoNfkbaIPvfk0i5cp1fhatdjsnNUOqOGy4ENmS");
                WC.Headers.Add(HttpRequestHeader.Accept, "text/plain");
                string response = "";
                response = WC.DownloadString(url);
                Newtonsoft.Json.Linq.JArray JObj = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(response).Value<Newtonsoft.Json.Linq.JArray>("list");
                if(JObj.Count > 0)
                {
                    definition = JObj[0].Value<string>("definition");
                }
                else
                {
                    definition = "I do not know what \"" + term + "\" is.";
                }
                
            }
            return definition;
        }
    }
}
