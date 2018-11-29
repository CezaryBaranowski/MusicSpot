using MusicSpot.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MusicSpot.Models
{
    public class Song
    {
        public Song()
        {

        }
        public Song(string artist, string title, string album, string path, Image albumArt, TimeSpan totalTime)
        {
            Artist = artist;
            Title = title;
            Album = album;
            Path = path;
            AlbumArt = albumArt;
            TotalTime = totalTime;
        }

        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Path { get; set; }
        public IList<string> Genres { get; set; }   

        private Image _albumArt;
        [CanBeNull]
        public Image AlbumArt
        {
            get => _albumArt;
            set => _albumArt = value;
        }

        private TimeSpan _totalTime;
        public TimeSpan TotalTime
        {
            get => _totalTime;
            set { _totalTime = value; }
        }
    }
}
