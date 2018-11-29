using System.Collections.ObjectModel;

namespace MusicSpot.Models
{
    public class Playlist
    {

        public string Name { get; set; }
        public string Path { get; set; }

        public ObservableCollection<Song> Songs { get; set; }
    }
}
