using System.Collections.Generic;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace YoutubePlaylists
{
    public class playlist
    {
        /// <summary>
        /// API key required for Youtube Data V3.
        /// </summary>
        private string API_KEY = "";

        /// <summary>
        /// Initiates the object with your API Key.
        /// </summary>
        /// <param name="APIKey"></param>
        public playlist(string APIKey)
        {
            API_KEY = APIKey;
        }

        /// <summary>
        /// Returns all videos inside a playlist
        /// </summary>
        /// <param name="playlistID"> The playlist URL</param>
        /// <param name="type"> If you don't know what this is, leave it as it is </param>
        /// <returns> string array of all the video IDs inside the playlist</returns>
        public string[] getVideos(string playlistID, string type = "contentDetails")
        {
            if (string.IsNullOrWhiteSpace(API_KEY))
            {
                return new string[] { "You don't have access to use this feature." };
            }
            BaseClientService.Initializer apiConnection = new BaseClientService.Initializer();
            apiConnection.ApiKey = API_KEY;
            List<string> IDs = new List<string> { };


            var youtubeService = new YouTubeService(apiConnection);

            string nextPageToken = "";
            while (nextPageToken != null)
            {
                var getPlaylistVideos = youtubeService.PlaylistItems.List(type);
                getPlaylistVideos.PlaylistId = playlistID;
                getPlaylistVideos.MaxResults = 50;
                getPlaylistVideos.PageToken = nextPageToken;
                var getVideos = getPlaylistVideos.Execute();

                foreach (var video in getVideos.Items)
                {
                    IDs.Add(video.ContentDetails.VideoId);
                }
                nextPageToken = getVideos.NextPageToken;
            }
            return IDs.ToArray();
        }
    }
}
