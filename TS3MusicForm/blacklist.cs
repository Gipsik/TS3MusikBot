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
            string blockedVideos = string.Join(",", File.ReadAllLines("blacklist.txt")).ToLower();
            if (blockedVideos.Contains(songURL.Trim().ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool addToBlacklist(string user, string getCommand)
        {
            try
            {
                string URL = getCommand.ToLower().Replace("!blacklist", "").Trim();
                File.AppendAllText("blacklist.txt", URL);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            
        }

        public static bool removeFromBlacklist(string user, string getCommand)
        {
            try
            {
                string URL = getCommand.ToLower().Replace("!blacklist", "").Trim();
                string[] blacklistedURLs = File.ReadAllLines("blacklist.txt");
                blacklistedURLs = blacklistedURLs.Where(url => !url.Contains(URL)).ToArray();
                File.WriteAllLines("blacklist.txt", blacklistedURLs);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
