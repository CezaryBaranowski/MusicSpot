using MusicSpot.Models;

namespace MusicSpot.ViewModels
{
    public sealed class DiscoveryViewModel : ViewModelBase
    {

        private static readonly DiscoveryViewModel discoveryViewModel = new DiscoveryViewModel();

        static DiscoveryViewModel()
        {
        }

        private DiscoveryViewModel()
        {
            InitDiscoveryViewModel();
        }

        public static DiscoveryViewModel GetInstance()
        {
            return discoveryViewModel;
        }

        private void InitDiscoveryViewModel()
        {

        }

        private Song _currentlyPlayedSong;
        public Song CurrentlyPlayedSong
        {
            get => _currentlyPlayedSong;
            set
            {
                _currentlyPlayedSong = value;
                OnPropertyChanged();
            }
        }
    }
}
