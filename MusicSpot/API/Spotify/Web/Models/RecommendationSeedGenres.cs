using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class RecommendationSeedGenres : BasicModel
    {
        [JsonProperty("genres")]
        public List<string> Genres { get; set; }
    }
}