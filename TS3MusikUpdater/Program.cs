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
            string randomFolderName = new Random().Next(1, 9999).ToString();
            string directory = imLocatedAt + @"\" + randomFolderName + @"\TS3MusikBot-master\TS3MusicForm\bin\Debug\";
            

            Process[] processes = Process.GetProcessesByName("TS3 Music Bot");
            foreach(Process process in processes)
            {
                process.Kill();
            }

            try
            {
                using (WebClient WC = new WebClient())
                {
                    WC.DownloadFile("https://github.com/JackRyder/TS3MusikBot/archive/master.zip", imLocatedAt + @"\new.zip");
                }

                ZipFile.ExtractToDirectory(imLocatedAt + @"\new.zip", imLocatedAt + @"\" + randomFolderName);
                File.Delete(imLocatedAt + @"\new.zip");
                foreach (string fileName in Directory.GetFiles(directory))
                {
                    int lastSlash = fileName.LastIndexOf('\\');
                    string safeFileName = fileName.Remove(0, lastSlash + 1);

                    if (File.Exists(imLocatedAt + @"\" + safeFileName))
                    {
                        File.Delete(imLocatedAt + @"\" + safeFileName);
                    }

                    File.Move(fileName, imLocatedAt + @"\" + safeFileName);
                }
                Directory.Delete(imLocatedAt + @"\" + randomFolderName,true);
            }
            catch (Exception)
            {
                Directory.Delete(imLocatedAt + @"\" + randomFolderName, true);
            }
            finally
            {
                Process.Start("TS3 Music Bot.exe");
            }
            
            
        }
    }
}
