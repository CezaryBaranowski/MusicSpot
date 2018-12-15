using Newtonsoft.Json;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class NewAlbumReleases : BasicModel
    {
        [JsonProperty("albums")]
        public Paging<SimpleAlbum> Albums { get; set; }
    }
}