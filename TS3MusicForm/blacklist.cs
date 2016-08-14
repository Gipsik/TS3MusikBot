using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TS3MusicBot
{
    class blacklist
    {
        public static bool checkBlacklist(string songURL)
        {
            if(!File.Exists("blacklist.txt"))
            {
                File.Create("blacklist.txt");
            }
            string blockedVideos = string.Join(",",File.ReadAllLines("blacklist.txt"));
            if (blockedVideos.Contains(songURL.Trim()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static void addToBlacklist(string user, string getCommand)
        {
            if(user != "someone")
            {
                // can possibly do something here for validation.
            }
            string URL = getCommand.ToLower().Replace("!blacklist","").Trim();
            File.AppendText("blacklist.txt").WriteLine(URL);
        }
    }
}
