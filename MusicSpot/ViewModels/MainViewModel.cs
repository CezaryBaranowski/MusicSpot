namespace MusicSpot.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private MainViewModel()
        {
            IsPlaying = false;
        }

        static MainViewModel()
        {

        }

        private static readonly MainViewModel mainViewModel = new MainViewModel();

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged("IsPlaying");
            }
        }

        public static MainViewModel GetInstance()
        {
            return mainViewModel ?? new MainViewModel();
        }
    }
}
