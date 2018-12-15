using System.Collections.Generic;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class ListResponse<T> : BasicModel
    {
        public List<T> List { get; set; }
    }
}