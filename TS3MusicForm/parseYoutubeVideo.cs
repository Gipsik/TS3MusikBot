using System;
using System.Collections.Specialized;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace TS3MusicBot
{
    // Thanks to github.com/JackChappell for this class :)

    class parseYoutubeVideo
    {
        private static readonly Regex YouTubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");

        public static string[] GetValues(string youtubeUrl)
        {
            string id = YouTubeVideoRegex.Match(youtubeUrl).Groups[1].Value;
            string title = null, url = null;

            using (var client = new HttpClient())
            {
                string queryString = client.GetAsync(string.Format(
                    "http://www.youtube.com/get_video_info?&video_id={0}&asv=3&el=detailpage&hl=en_US", id))
                    .Result.Content.ReadAsStringAsync().Result;

                NameValueCollection collection = HttpUtility.ParseQueryString(queryString);

                if (collection["status"] != "fail")
                {
                    title = collection["title"];

                    foreach (string format in collection["url_encoded_fmt_stream_map"].Split(','))
                    {
                        collection = HttpUtility.ParseQueryString(format);

                        if (collection["type"].Split(';')[0] == "video/mp4")
                        {
                            url = string.Format("http://redirector.googlevideo.com/{0}", HttpUtility.UrlDecode(collection["url"])
                                                    .Split(new string[] { ".googlevideo.com/" }, StringSplitOptions.None)[1]);
                            break;
                        }
                    }
                }
            }
            if(isValid(url))
            {
                return new string[] { title, url };
            }
            else
            {
                return new string[] { title, null };
            }
            
        }

        // Created validator, as seen below :)
        public static bool isValid(string checkURL)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(checkURL);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}