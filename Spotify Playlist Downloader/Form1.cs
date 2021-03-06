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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spotify_Playlist_Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        JArray playlistArray;

        private void buttonGetSongs_Click(object sender, EventArgs e)
        {
            listView_SongsList.Items.Clear();

            ImageList dowloadedImages = new ImageList();
            dowloadedImages.ImageSize = new Size(236, 236);
            dowloadedImages.ColorDepth = ColorDepth.Depth32Bit;

            dowloadedImages.Images.Clear();
            try
            {
                HttpRequest tokenRequest = new HttpRequest();
                tokenRequest.UserAgent = Http.ChromeUserAgent();
                String tokenJson = tokenRequest.Get("https://open.spotify.com/get_access_token?reason=transport&productType=web_player").ToString();

                JObject spotifyJsonToken = JObject.Parse(tokenJson);

                String spotifyToken = spotifyJsonToken.SelectToken("accessToken").ToString();

                HttpRequest getSpotifyPlaylist = new HttpRequest();
                getSpotifyPlaylist.AddHeader("Authorization", "Bearer " + spotifyToken);
                String playlist = getSpotifyPlaylist.Get("https://api.spotify.com/v1/playlists/" + textBox_PlaylistID.Text + "/tracks?offset=0&limit=100").ToString();

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
                MessageBox.Show("Make Sure You Have passed Playlist ID and Not URL", "Playlist Not Found ERROR");
            }


        }

        private void listView_SongsList_MouseClick(object sender, MouseEventArgs e)
        {
            var downloadPath = Environment.CurrentDirectory + @"\\Downloads";
            var client = new WebClient();
            client.DownloadFile(playlistArray[listView_SongsList.FocusedItem.Index].SelectToken("track").SelectToken("album").SelectToken("images")[0].SelectToken("url").ToString(),downloadPath+"/thumb.jpg");

            string songName = playlistArray[listView_SongsList.FocusedItem.Index].SelectToken("track").SelectToken("name").ToString();
            string artists = playlistArray[listView_SongsList.FocusedItem.Index].SelectToken("track").SelectToken("artists")[0].SelectToken("name").ToString();
            string songAlbum = playlistArray[listView_SongsList.FocusedItem.Index].SelectToken("track").SelectToken("album").SelectToken("name").ToString();
            



            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            process.StartInfo.FileName = Environment.CurrentDirectory + @"\\youtube-dl.exe";

            
            process.StartInfo.Arguments = "-x --no-continue " + "\"" + "ytsearch1: " + songName + " " + artists + "\" " + "--audio-format mp3 --audio-quality 0 -o " + "/Downloads/"+"\"" + songName + " - " + songAlbum +"\""+ "." + "%(ext)s";
            process.Start();
            process.WaitForExit();

            System.Diagnostics.Process tagEditor = new System.Diagnostics.Process();
            tagEditor.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            
            tagEditor.StartInfo.FileName = Environment.CurrentDirectory + @"\\tageditor.exe";


            tagEditor.StartInfo.Arguments = "set title=" + "\"" + songName + "\""+" album=" + "\"" + songAlbum + "\"" + " artist=" + "\"" + artists + "\"" + " cover=Downloads/thumb.jpg --files " + "\"" + "Downloads/" + songName + " - " + songAlbum + ".mp3"+"\"";
            tagEditor.Start();
            tagEditor.WaitForExit();

            File.Delete(Path.Combine(downloadPath, songName + " - " + songAlbum + ".mp3.bak"));
            File.Delete(Path.Combine(downloadPath, "thumb.jpg"));
        }

        private void paypalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/henry-richard7");
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
    }
}
