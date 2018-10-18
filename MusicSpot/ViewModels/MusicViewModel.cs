namespace MusicSpot.ViewModels
{
    public sealed class MusicViewModel : ViewModelBase
    {

        private static readonly MusicViewModel musicViewModel = new MusicViewModel();

        private MusicViewModel()
        {
            IsPlaying = false;
        }

        private bool _isPlaying;

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        public static MusicViewModel GetInstance()
        {
            return musicViewModel;
        }
    }
}
