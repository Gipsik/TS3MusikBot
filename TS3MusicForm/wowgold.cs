using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TS3MusicBot
{
    class wowgold
    {
        public string getWoWGold() // per request, it now gets the current wow gold price.
        {
            try
            {
                using (WebClient WC = new WebClient()) // new webclient because i'm lazy and can't be bothered using httprequests ect
                {
                    string gold = WC.DownloadString("https://wowtoken.info/"); // download the page
                    gold = gold.Substring(gold.IndexOf("EU-buy"), 100); // shrink the string to 100 characters
                    gold = gold.Substring(gold.IndexOf('>') + 1, gold.IndexOf('<') - gold.IndexOf('>') - 1); // get the innerHTML with my super awkward code
                    return gold; // return the string
                }
            }
            catch (Exception)
            {
                return "N/A";
            }
        }
    }
}
