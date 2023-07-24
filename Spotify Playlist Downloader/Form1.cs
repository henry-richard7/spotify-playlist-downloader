using System;
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

        private void listView_SongsList_MouseClick(object sender, MouseEventArgs e)
        {
            helper.DownloadSingleItem(helper.PlayListItems[listView_SongsList.FocusedItem.Index], Environment.CurrentDirectory + @"\\Downloads");
            if (helper.PlayListItems[listView_SongsList.FocusedItem.Index].DownloadStatus == Models.DownloadStatus.Downloaded)
            {
                MessageBox.Show($"Done! Downloaded 1 song.");
            }
            else
            {
                MessageBox.Show($"Done! skipped 1 song.");
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
            helper.DownloadAll(Environment.CurrentDirectory + @"\\Downloads");
            ShowResult();
        }

        private void TextBox_PlaylistID_TextChanged(object sender, EventArgs e)
        {
            EnableElements();
        }

    }
}
