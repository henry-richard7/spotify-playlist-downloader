using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spotify_Playlist_Downloader.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spotify_Playlist_Downloader
{
    public partial class Form1 : Form
    {
        private DownloadHelper helper;

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
            btnDownloadAll.Enabled = !string.IsNullOrEmpty(textBox_PlaylistID.Text) && playlistItems != null && playlistItems.Count > 0;
        }

        /// <summary>
        /// Show a messagebox with the result
        /// </summary>
        private void ShowResult()
        {
            MessageBox.Show($"Done! Downloaded {downloaded} songs, skipped {skipped} already existing songs!");
        }

        List<Item> playlistItems;
        int downloaded, skipped;

        private void buttonGetSongs_Click(object sender, EventArgs e)
        {
            // init
            listView_SongsList.Items.Clear();
            helper = new DownloadHelper(textBox_PlaylistID.Text);

            try
            {
                helper.GetPlayListItems();                

                // playlist items to listview
                for (int i = 0; i < helper.PlayListItems.Count; i++)
                {
                    listView_SongsList.Items.Add(helper.PlayListItems[i].track.name, i);                    
                }
                listView_SongsList.LargeImageList = helper.PlayListItemsImageList;
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
            DownloadSingleItem(playlistItems[listView_SongsList.FocusedItem.Index], Environment.CurrentDirectory + @"\\Downloads");
            ShowResult();
        }


        /// <summary>
        /// Download a single item from the list of items
        /// </summary>
        /// <param name="playListItem">The item to download</param>
        /// <param name="targetFolder">The target folder for the download</param>
        private void DownloadSingleItem(Item item, string targetFolder)
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

            if (playlistItems == null || playlistItems.Count == 0)
            {
                return;
            }
            // download all
            foreach (var item in playlistItems)
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
