using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicSpot.API.Spotify.Web.Models
{
    public class AvailabeDevices : BasicModel
    {
        [JsonProperty("devices")]
        public List<Device> Devices { get; set; }
    }
}