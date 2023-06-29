using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Spotify_Playlist_Downloader.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Leaf.xNet;
using System.IO;
using System.Net;

namespace Spotify_Playlist_Downloader
{

    public class DownloadHelper
    {
        public string PlayListId { get; set; }
        public List<Item> PlayListItems { get; private set; }
        public ImageList PlayListItemsImageList { get; private set; }
        /// <summary>
        /// Number of songs downloaded in last run
        /// </summary>
        public int Downloaded { get; private set; }
        /// <summary>
        /// Number of songs skipped in last run
        /// </summary>
        public int Skipped { get; set; }

        public DownloadHelper(string playListIdentifier)
        {
            PlayListId = GetPlaylistId(playListIdentifier);
            InitImageList();
            ResetCounters();
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
        /// Reset download counters
        /// </summary>
        public void ResetCounters()
        {
            Downloaded = 0; 
            Skipped = 0;
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

        public void DownloadAll()
        {
            ResetCounters();

            // download all
            foreach (var item in PlayListItems)
            {
                DownloadSingleItem(item, Environment.CurrentDirectory + @"\\Downloads");
            }
        }

        /// <summary>
        /// Download a single item from the list of items
        /// </summary>
        /// <param name="playListItem">The item to download</param>
        /// <param name="targetFolder">The target folder for the download</param>
        public void DownloadSingleItem(Item item, string targetFolder)
        {
            // create targetfolde if not exists
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            var client = new WebClient();
            client.DownloadFile(item.track.album.images[0].url, targetFolder + "/thumb.jpg");

            string songName = item.track.name;
            string songNameFile = songName.RemoveSpecialChars();
            string artists = item.track.artists[0].name;
            string artistsFile = artists.RemoveSpecialChars();
            string songAlbum = item.track.album.name;
            string songAlbumFile = songAlbum.RemoveSpecialChars();

            // only process if file not exists
            string target = Path.Combine(targetFolder, songNameFile + " - " + artistsFile + ".mp3");
            if (File.Exists(target))
            {
                Skipped++;
            }
            else
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                process.StartInfo.FileName = Environment.CurrentDirectory + @"\\youtube-dl.exe";

                process.StartInfo.Arguments = "-x --no-continue " + "\"" + "ytsearch1: " + songNameFile + " " + artistsFile + "\" " + "--audio-format mp3 --audio-quality 0 -o " + "/Downloads/" + "\"" + songNameFile + " - " + artistsFile + "\"" + "." + "%(ext)s";
                process.Start();
                process.WaitForExit();

                System.Diagnostics.Process tagEditor = new System.Diagnostics.Process();
                tagEditor.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

                tagEditor.StartInfo.FileName = Environment.CurrentDirectory + @"\\tageditor.exe";

                tagEditor.StartInfo.Arguments = "set title=" + "\"" + songName + "\"" + " album=" + "\"" + songAlbum + "\"" + " artist=" + "\"" + artists + "\"" + " cover=Downloads/thumb.jpg --files " + "\"" + "Downloads/" + songNameFile + " - " + artistsFile + ".mp3" + "\"";
                tagEditor.Start();
                tagEditor.WaitForExit();

                File.Delete(Path.Combine(targetFolder, songNameFile + " - " + artistsFile + ".mp3.bak"));
                File.Delete(Path.Combine(targetFolder, "thumb.jpg"));
                Downloaded++;
            }
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
