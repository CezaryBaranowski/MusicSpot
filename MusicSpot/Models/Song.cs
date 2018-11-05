using System;

namespace MusicSpot.Models
{
    public class Song
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public TimeSpan TotalTime { get; set; }
    }
}
