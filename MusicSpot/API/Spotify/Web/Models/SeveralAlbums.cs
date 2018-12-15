using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class SeveralAlbums : BasicModel
    {
        [JsonProperty("albums")]
        public List<FullAlbum> Albums { get; set; }
    }
}