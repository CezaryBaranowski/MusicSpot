using Newtonsoft.Json;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class FollowedArtists : BasicModel
    {
        [JsonProperty("artists")]
        public CursorPaging<FullArtist> Artists { get; set; }
    }
}