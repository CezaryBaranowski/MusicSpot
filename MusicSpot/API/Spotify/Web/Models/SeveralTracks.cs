using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class SeveralTracks : BasicModel
    {
        [JsonProperty("tracks")]
        public List<FullTrack> Tracks { get; set; }
    }
}