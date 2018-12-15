using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class SeveralAudioFeatures : BasicModel
    {
        [JsonProperty("audio_features")]
        public List<AudioFeatures> AudioFeatures { get; set; }
    }
}