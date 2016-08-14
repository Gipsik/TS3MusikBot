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
            List<string> blacklistedVideos = new List<string> { };
            if(!File.Exists("blacklist.txt"))
            {
                File.Create("blacklist.txt");
            }
            blacklistedVideos.AddRange(File.ReadAllLines("blacklist.txt"));
            if (blacklistedVideos.Contains(songURL.Trim()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
