using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace TS3MusikUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            string imLocatedAt = Environment.CurrentDirectory;

            Process[] P = Process.GetProcessesByName("TS3MusicForm");
            if(P.Count() != 0)
            {
                P.First().Kill();
            }
            

            using (WebClient WC = new WebClient())
            {
                WC.DownloadFile("https://github.com/JackRyder/TS3MusikBot/archive/master.zip",imLocatedAt + @"\new.zip");
            }

            ZipFile.ExtractToDirectory(imLocatedAt + @"\new.zip", imLocatedAt);
            File.Delete(imLocatedAt + @"\new.zip");
            Directory.Move(imLocatedAt + @"\TS3MusikBot-master\TS3MusicForm",) // early commit do not worry
            
        }
    }
}
