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
            btnDownloadAll.Enabled = helper != null;
        }

        /// <summary>
        /// Show a messagebox with the result
        /// </summary>
        private void ShowResult()
        {
            MessageBox.Show($"Done! Downloaded {helper.Downloaded} songs, skipped {helper.Skipped} already existing songs!");
        }

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
            helper.ResetCounters();
            helper.DownloadSingleItem(helper.PlayListItems[listView_SongsList.FocusedItem.Index], Environment.CurrentDirectory + @"\\Downloads");
            ShowResult();
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
            helper.DownloadAll();
            ShowResult();
        }

        private void TextBox_PlaylistID_TextChanged(object sender, EventArgs e)
        {
            EnableElements();
        }

    }
}
