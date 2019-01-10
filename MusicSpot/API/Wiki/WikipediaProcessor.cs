using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicSpot.API.Wiki
{
    public class WikiProcessor
    {
        private static string baseUrl =
            "https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro&explaintext&redirects=1&titles=";

        public static async Task<string> GetArticle(string title)
        {
            var validTitle = Regex.Replace(title, " ", "%20");
            var url = String.Concat(baseUrl, validTitle);
            var httpClient = new HttpClient();

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result != null && result.Length > 1)
                    {
                        var data = JObject.Parse(result);
                        var text = data["query"]["pages"];
                        text = (string)text.First.First["extract"];
                        return text.ToString();
                    }
                    return "";
                }

                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
