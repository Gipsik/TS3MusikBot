using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS3MusicBot
{
    public class showHelp
    {

        public List<string> howToUse()
        {
            List<string> list = new List<string> { };

            list.Add("How to use Jack's Music Bot!");
            list.Add("'!play [song youtube url]' to request a song!");
            list.Add("'!skip' to skip the current song!");
            list.Add("'!song' to get the current song that's playing!");
            list.Add("'!auto' to auto play music (NIGHTCORE!)");
            list.Add("'!repeat' to toggle repeat for the current song that's playing!");
            list.Add("!gold to get the current price of 1 WoW token");
            list.Add("'!say [text here]' to make the bot speak!");
            list.Add("'!playlist' [playlist ID] to add a full playlist!");
            return list;
        }
    }
}
