using Leaf.xNet;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spotify_Playlist_Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            EnableElements();
        }

        /// <summary>
        /// Handle form elements enabled
        /// </summary>
        private void EnableElements()
        {
            buttonGetSongs.Enabled = !string.IsNullOrEmpty(textBox_PlaylistID.Text);
            btnDownloadAll.Enabled = !string.IsNullOrEmpty(textBox_PlaylistID.Text) && playlistArray != null && playlistArray.HasValues;
        }

        /// <summary>
        /// Show a messagebox with the result
        /// </summary>
        private void ShowResult()
        {
            MessageBox.Show($"Done! Downloaded {downloaded} songs, skipped {skipped} already existing songs!");
        }

        JArray playlistArray;
        int downloaded, skipped;

        private void buttonGetSongs_Click(object sender, EventArgs e)
        {
            listView_SongsList.Items.Clear();

            ImageList dowloadedImages = new ImageList();
            dowloadedImages.ImageSize = new Size(236, 236);
            dowloadedImages.ColorDepth = ColorDepth.Depth32Bit;

            dowloadedImages.Images.Clear();
            try
            {
                // fail early
                var playlistId = GetPlaylistId();

                HttpRequest tokenRequest = new HttpRequest();
                tokenRequest.UserAgent = Http.ChromeUserAgent();
                String tokenJson = tokenRequest.Get("https://open.spotify.com/get_access_token?reason=transport&productType=web_player").ToString();

                JObject spotifyJsonToken = JObject.Parse(tokenJson);

                String spotifyToken = spotifyJsonToken.SelectToken("accessToken").ToString();

                HttpRequest getSpotifyPlaylist = new HttpRequest();
                getSpotifyPlaylist.AddHeader("Authorization", "Bearer " + spotifyToken);
                String playlist = getSpotifyPlaylist.Get("https://api.spotify.com/v1/playlists/" + playlistId + "/tracks?offset=0&limit=100").ToString();

                JObject jobject = JObject.Parse(playlist);

                playlistArray = JArray.Parse(jobject.SelectToken("items").ToString());

                for (int i = 0; i < playlistArray.Count; i++)
                {
                    HttpRequest w = new HttpRequest();
                    listView_SongsList.Items.Add(playlistArray[i].SelectToken("track").SelectToken("name").ToString(), i);
                    try
                    {
                        var response = w.Get(playlistArray[i].SelectToken("track").SelectToken("album").SelectToken("images")[0].SelectToken("url").ToString());
                        byte[] imageBytes = response.ToBytes();
                        MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        memoryStream.Write(imageBytes, 0, imageBytes.Length);
                        Image imgs = Image.FromStream(memoryStream, true);
                        dowloadedImages.Images.Add(imgs);
                    }
                    catch
                    {
                        var response = w.Get("https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/600px-No_image_available.svg.png");
                        byte[] imageBytes = response.ToBytes();
                        MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        memoryStream.Write(imageBytes, 0, imageBytes.Length);
                        Image imgs = Image.FromStream(memoryStream, true);
                        dowloadedImages.Images.Add(imgs);
                    }


                }
                listView_SongsList.LargeImageList = dowloadedImages;
            }
            catch
            {
                MessageBox.Show("Make sure sou have passed a valid playlist ID or valid URL", "Playlist Not Found ERROR");
            }

            EnableElements();
        }

        /// <summary>
        /// Get the playlist id from the textbox
        /// </summary>
        /// <returns>The id of the playlist</returns>        
        private string GetPlaylistId()
        {
            // example url: https://open.spotify.com/playlist/37i9dQZF1DX4xuWVBs4FgJ?si=ee30c0f70aa84b59
            // resulting id: 37i9dQZF1DX4xuWVBs4FgJ

            string retVal = string.Empty;

            if (string.IsNullOrEmpty(textBox_PlaylistID.Text))
            {
                throw new Exception("No playlist provided");                
            }

            // check url
            try
            {
                Uri u = new Uri(textBox_PlaylistID.Text);
                // get the last part
                retVal = u.Segments[2];
            }
            catch (Exception)
            {
            }

            // no url so it should be an playlist id
            if (string.IsNullOrEmpty(retVal))
            {
                retVal = textBox_PlaylistID.Text;
            }

            return retVal;            
        }

        private void listView_SongsList_MouseClick(object sender, MouseEventArgs e)
        {
            downloaded = 0;
            skipped = 0;
            DownloadSingleItem(playlistArray[listView_SongsList.FocusedItem.Index], Environment.CurrentDirectory + @"\\Downloads");
            ShowResult();
        }


        /// <summary>
        /// Download a single item from the list of items
        /// </summary>
        /// <param name="playListItem">The item to download</param>
        /// <param name="targetFolder">The target folder for the download</param>
        private void DownloadSingleItem(JToken playListItem, string targetFolder)
        {
            // create targetfolde if not exists
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            var client = new WebClient();
            client.DownloadFile(playListItem.SelectToken("track").SelectToken("album").SelectToken("images")[0].SelectToken("url").ToString(), targetFolder + "/thumb.jpg");

            string songName = playListItem.SelectToken("track").SelectToken("name").ToString();
            string songNameFile = songName.RemoveSpecialChars();
            string artists = playListItem.SelectToken("track").SelectToken("artists")[0].SelectToken("name").ToString();
            string artistsFile = artists.RemoveSpecialChars();
            string songAlbum = playListItem.SelectToken("track").SelectToken("album").SelectToken("name").ToString();
            string songAlbumFile = songAlbum.RemoveSpecialChars();

            // only process if file not exists
            string target = Path.Combine(targetFolder, songNameFile + " - " + artistsFile + ".mp3");
            if (File.Exists(target))
            {
                skipped++;
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
                downloaded++;
            }
        }

        private void paypalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/henry-richard7");
        }

        private void paypalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/paypalme/henryrics");
        }

        private void youtubeChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCVGasc5jr45eZUpZNHvbtWQ");
        }

        private void telegramChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://t.me/cracked4free");
        }

        /// <summary>
        /// Download all songs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDownloadAll_Click(object sender, EventArgs e)
        {
            downloaded = 0;
            skipped = 0;

            if (playlistArray == null || !playlistArray.HasValues)
            {
                return;
            }
            // download all
            foreach (var item in playlistArray)
            {
                DownloadSingleItem(item, Environment.CurrentDirectory + @"\\Downloads");
            }
            ShowResult();
        }

        private void TextBox_PlaylistID_TextChanged(object sender, EventArgs e)
        {
            EnableElements();
        }

    }
}
