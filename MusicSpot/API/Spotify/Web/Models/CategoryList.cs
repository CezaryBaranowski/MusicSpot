using Newtonsoft.Json;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class CategoryList : BasicModel
    {
        [JsonProperty("categories")]
        public Paging<Category> Categories { get; set; }
    }
}