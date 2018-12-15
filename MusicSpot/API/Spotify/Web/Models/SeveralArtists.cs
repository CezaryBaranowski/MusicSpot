using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class SeveralArtists : BasicModel
    {
        [JsonProperty("artists")]
        public List<FullArtist> Artists { get; set; }
    }
}