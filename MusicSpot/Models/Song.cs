using System;

namespace MusicSpot.Models
{
    public class Song
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }

        private TimeSpan _totalTime;
        public TimeSpan TotalTime
        {
            get => _totalTime;
            set { _totalTime = value; }
        }
    }
}
