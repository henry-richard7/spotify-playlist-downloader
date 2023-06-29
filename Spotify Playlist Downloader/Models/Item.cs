using Newtonsoft.Json;
using System;

namespace Spotify_Playlist_Downloader.Models
{
    public class Item
    {
        public DateTime added_at { get; set; }
        public AddedBy added_by { get; set; }
        public bool is_local { get; set; }
        public object primary_color { get; set; }
        public Track track { get; set; }
        public VideoThumbnail video_thumbnail { get; set; }
        [JsonIgnore]
        public DownloadStatus DownloadStatus { get; set; } = DownloadStatus.Unknown;
    }
}
