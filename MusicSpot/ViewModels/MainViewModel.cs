namespace MusicSpot.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            _isPlaying = false;
        }

       // private MainViewModel mainViewModel;
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

        //public MainViewModel GetMainViewModel()
        //{
        //    return mainViewModel ?? new MainViewModel();
        //}
    }
}
