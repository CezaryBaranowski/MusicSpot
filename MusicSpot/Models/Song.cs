﻿using System;
using System.Drawing;

namespace MusicSpot.Models
{
    public class Song
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Path { get; set; }

        private Image _albumArt;
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
