using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;

namespace TS3MusicBot
{
    public class autoDJ
    {

        public bool autoplay = false;
        private Process autoplayProcess;

        public bool Auto()
        {
            if (autoplay)
            {
                stopAuto();
                return false;
            }
            autoplay = true;
            using (WebClient WC = new WebClient())
            {
                WC.DownloadFile("http://stream.nightcoreradio.com:9040/main_stream.m3u", "playlist.m3u");
            }
            autoplayProcess = Process.Start("playlist.m3u");
            return true;
            
        }

        private void stopAuto()
        {
            autoplay = false;
            Process[] MP = Process.GetProcessesByName("wmplayer");
            MP[0].Kill();
        }

    }
}
