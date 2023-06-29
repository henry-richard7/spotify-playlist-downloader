using Leaf.xNet;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace Spotify_Playlist_Downloader.Models
{
    public class Track
    {
        public Album album { get; set; }
        public List<Artist> artists { get; set; }
        public List<string> available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool episode { get; set; }
        public bool @explicit { get; set; }
        public ExternalIds external_ids { get; set; }
        public ExternalUrls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public bool is_local { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public bool track { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

        /// <summary>
        /// Download the image
        /// </summary>
        /// <returns>The image</returns>
        public Image GetImage()
        {
            HttpRequest w = new HttpRequest();
            try
            {
                // get image
                var response = w.Get(this.album.images[0].url);
                byte[] imageBytes = response.ToBytes();
                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                Image imgs = Image.FromStream(memoryStream, true);
                return imgs;
            }
            catch
            {
                var response = w.Get("https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/600px-No_image_available.svg.png");
                byte[] imageBytes = response.ToBytes();
                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                Image imgs = Image.FromStream(memoryStream, true);
                return imgs;
            }
        }
    }
}
