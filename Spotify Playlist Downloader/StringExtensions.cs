using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spotify_Playlist_Downloader
{
    public static class StringExtensions
    {
        /// <summary>
        /// Remove characters that create errors in file names
        /// </summary>
        /// <param name="str">The input string</param>
        /// <returns>The string with replaced characters</returns>
        public static string RemoveSpecialChars(this string str)
        {
            Regex r = new Regex("(?:[^a-z0-9() ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(str, String.Empty);
        }
    }
}
