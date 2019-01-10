using HtmlAgilityPack;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicSpot.API.Tekstowo
{
    public class TekstowoCrawler
    {
        public static async Task<string> GetSongLyrics(string songName, string artistName)
        {
            var baseurl = "https://www.tekstowo.pl/piosenka,";
            var validSongName = songName.Replace(" ", "_");
            var validArtistName = artistName.Replace(" ", "_");
            var suffix = ".html";
            var url = string.Concat(baseurl, validArtistName, ",", validSongName, suffix);

            var httpClient = new HttpClient();

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    var html = await httpClient.GetStringAsync(url);

                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);

                    var text = htmlDocument.DocumentNode
                        .Descendants().FirstOrDefault(node => node.GetAttributeValue("class", "").Equals("song-text"))?.InnerHtml;

                    if (text != null)
                    {
                        var lyrics = System.Web.HttpUtility.HtmlDecode(text);
                        if (lyrics.Length > 1)
                        {
                            lyrics = Regex.Replace(lyrics, "<br>", "");
                            lyrics = Regex.Replace(lyrics, "    ", "");
                            lyrics = lyrics.Substring(33, lyrics.Length - 246);

                            return lyrics;
                        }

                        return "";
                    }

                    return "";
                }

                return "";

            }
        }
    }
}
