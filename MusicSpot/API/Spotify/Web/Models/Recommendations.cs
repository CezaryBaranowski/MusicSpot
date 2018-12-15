using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class Recommendations : BasicModel
    {
        [JsonProperty("seeds")]
        public List<RecommendationSeed> Seeds { get; set; }

        [JsonProperty("tracks")]
        public List<SimpleTrack> Tracks { get; set; }
    }
}