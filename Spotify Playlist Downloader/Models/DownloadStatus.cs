using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Playlist_Downloader.Models
{
    /// <summary>
    /// Represents the download status of the item
    /// </summary>
    public enum DownloadStatus
    {
        Unknown,
        Downloaded,
        Skipped
    }
}
