using MusicSpot.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MusicSpot.Models
{
    public class Playlist
    {

        public string Name { get; set; }
        public string Path { get; set; }

        public ObservableCollection<Song> Songs { get; set; }

        public Playlist()
        {
            Songs = new ObservableCollection<Song>();
        }
        public Playlist(string name, [CanBeNull] ObservableCollection<Song> songs = null)
        {
            Name = name;
            Path = PlaylistManager.PlaylistDirectory + @"\\" + name;
            Songs = songs ?? new ObservableCollection<Song>();
        }

        public Playlist(string name, string path, [CanBeNull] ICollection<string> Songs = null)
        {
            Name = name;
            Path = path;
            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}
