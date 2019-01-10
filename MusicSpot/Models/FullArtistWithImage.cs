using MusicSpot.API.Spotify.Web.Models;
using Image = System.Drawing.Image;

namespace MusicSpot.Models
{
    public class FullArtistWithImage : ViewModelBase
    {
        private FullArtist _fullArtist;

        public FullArtist FullArtist
        {
            get => _fullArtist;
            set
            {
                _fullArtist = value;
                OnPropertyChanged();
            }
        }

        private Image _smallImage;

        public Image SmallImage
        {
            get => _smallImage;
            set
            {
                _smallImage = value;
                OnPropertyChanged();
            }
        }
    }
}
