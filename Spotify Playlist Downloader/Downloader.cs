using Spotify_Playlist_Downloader.Models;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Spotify_Playlist_Downloader
{
    /// <summary>
    /// Class to actually download a song to make multi threading possible
    /// </summary>
    public class Downloader
    {
        public Item itemToDownload { get; private set; }
        public String targetFolder { get; private set; }

        private ManualResetEvent _doneEvent;

        public Downloader(Item item, string targetFolder, ManualResetEvent doneEvent)
        {
            itemToDownload = item;
            _doneEvent = doneEvent;
            this.targetFolder = targetFolder;
        }

        public void DownloadSingleItem()
        {
            itemToDownload.DownloadStatus = DownloadStatus.Unknown;

            // create targetfolde if not exists
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string songName = itemToDownload.track.name;
            string songNameFile = songName.RemoveSpecialChars();
            string artists = itemToDownload.track.artists[0].name;
            string artistsFile = artists.RemoveSpecialChars();
            string songAlbum = itemToDownload.track.album.name;
            string songAlbumFile = songAlbum.RemoveSpecialChars();

            // only process if file not exists
            string target = Path.Combine(targetFolder, songNameFile + " - " + artistsFile + ".mp3");
            if (File.Exists(target))
            {
                itemToDownload.DownloadStatus = DownloadStatus.Skipped;
            }
            else
            {
                // download song
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                process.StartInfo.FileName = Environment.CurrentDirectory + @"\\youtube-dl.exe";

                process.StartInfo.Arguments = "-x --no-continue " + "\"" + "ytsearch1: " + songNameFile + " " + artistsFile + "\" " + "--audio-format mp3 --audio-quality 0 -o " + "/Downloads/" + "\"" + songNameFile + " - " + artistsFile + "\"" + "." + "%(ext)s";
                process.Start();
                process.WaitForExit();


                // download thumb
                // tmp filename to make multithreading possible
                string guid = Guid.NewGuid().ToString();
                var client = new WebClient();
                client.DownloadFile(itemToDownload.track.album.images[0].url, targetFolder + $"/{guid}.jpg");

                // edit tags
                System.Diagnostics.Process tagEditor = new System.Diagnostics.Process();
                tagEditor.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                tagEditor.StartInfo.FileName = Environment.CurrentDirectory + @"\\tageditor.exe";
                tagEditor.StartInfo.Arguments = "set title=" + "\"" + songName + "\"" + " album=" + "\"" + songAlbum + "\"" + " artist=" + "\"" + artists + "\"" + $" cover=Downloads/{guid}.jpg --files " + "\"" + "Downloads/" + songNameFile + " - " + artistsFile + ".mp3" + "\"";
                tagEditor.Start();
                tagEditor.WaitForExit();

                // cleanup
                File.Delete(Path.Combine(targetFolder, songNameFile + " - " + artistsFile + ".mp3.bak"));
                File.Delete(Path.Combine(targetFolder, $"{guid}.jpg"));
                itemToDownload.DownloadStatus = DownloadStatus.Downloaded;
            }
        }
    }
}
