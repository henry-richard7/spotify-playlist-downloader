using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spotify_Playlist_Downloader.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Spotify_Playlist_Downloader
{

    /// <summary>
    /// Class for handling the downloads
    /// </summary>
    public class DownloadHelper
    {
        public string PlayListId { get; set; }
        public List<Item> PlayListItems { get; private set; }
        public ImageList PlayListItemsImageList { get; private set; }
        /// <summary>
        /// Number of songs downloaded in last run
        /// </summary>
        public int Downloaded
        {
            get { return PlayListItems.Count(i => i.DownloadStatus == DownloadStatus.Downloaded); }
        }
        /// <summary>
        /// Number of songs skipped in last run
        /// </summary>
        public int Skipped
        {
            get { return PlayListItems.Count(i => i.DownloadStatus == DownloadStatus.Skipped); }

        }

        public DownloadHelper(string playListIdentifier)
        {
            PlayListId = GetPlaylistId(playListIdentifier);
            InitImageList();
        }

        /// <summary>
        /// Initialize the imagelist
        /// </summary>
        private void InitImageList()
        {
            ImageList downloadedImages = new ImageList();
            downloadedImages.ImageSize = new Size(236, 236);
            downloadedImages.ColorDepth = ColorDepth.Depth32Bit;
            PlayListItemsImageList = downloadedImages;
        }

        /// <summary>
        /// Get the playlist items
        /// </summary>
        /// <returns></returns>
        public bool GetPlayListItems()
        {
            if (String.IsNullOrEmpty(PlayListId))
            {
                throw new Exception("No playlist id!");
            }

            // get spotify token for download
            HttpRequest tokenRequest = new HttpRequest();
            tokenRequest.UserAgent = Http.ChromeUserAgent();
            String tokenJson = tokenRequest.Get("https://open.spotify.com/get_access_token?reason=transport&productType=web_player").ToString();
            JObject spotifyJsonToken = JObject.Parse(tokenJson);
            String spotifyToken = spotifyJsonToken.SelectToken("accessToken").ToString();

            // get playlist info
            HttpRequest getSpotifyPlaylist = new HttpRequest();
            getSpotifyPlaylist.AddHeader("Authorization", "Bearer " + spotifyToken);
            String playlist = getSpotifyPlaylist.Get("https://api.spotify.com/v1/playlists/" + PlayListId + "/tracks?offset=0&limit=100").ToString();

            // deserialize to list
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(playlist);
            PlayListItems = myDeserializedClass.items;

            // get images
            InitImageList();
            foreach (var item in PlayListItems)
            {
                PlayListItemsImageList.Images.Add(item.track.GetImage());
            }

            return PlayListItems.Any();
        }

        /// <summary>
        /// Download all songs
        /// </summary>
        public void DownloadAll(string targetFolder)
        {
            // doen in property
            // download all
            foreach (var item in PlayListItems)
            {
                Downloader d = new Downloader(item, targetFolder, null);
                d.DownloadSingleItem();
            }
        }

        /// <summary>
        /// Download a single item from the list of items
        /// </summary>
        /// <param name="playListItem">The item to download</param>
        /// <param name="targetFolder">The target folder for the download</param>
        public void DownloadSingleItem(Item item, string targetFolder)
        {
            Downloader d = new Downloader(item, targetFolder, null);
            d.DownloadSingleItem();
        }

        /// <summary>
        /// Get the playlist id from the textbox
        /// </summary>
        /// <returns>The id of the playlist</returns>        
        private string GetPlaylistId(string playListIdentifier)
        {
            // example url: https://open.spotify.com/playlist/37i9dQZF1DX4xuWVBs4FgJ?si=ee30c0f70aa84b59
            // resulting id: 37i9dQZF1DX4xuWVBs4FgJ

            string retVal = string.Empty;

            if (string.IsNullOrEmpty(playListIdentifier))
            {
                throw new Exception("No playlist provided");
            }

            // check url
            try
            {
                Uri u = new Uri(playListIdentifier);
                // get the last part
                retVal = u.Segments[2];
            }
            catch (Exception)
            {
            }

            // no url so it should be an playlist id
            if (string.IsNullOrEmpty(retVal))
            {
                retVal = playListIdentifier;
            }

            return retVal;
        }
    }
}
