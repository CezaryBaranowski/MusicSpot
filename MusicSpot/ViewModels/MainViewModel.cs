namespace MusicSpot.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private MainViewModel()
        {
            SelectedTabIndex = 0;
            MusicViewModel = MusicViewModel.GetInstance();
        }
        //ensure beforefieldinit off
        static MainViewModel()
        {

        }

        private static readonly MainViewModel mainViewModel = new MainViewModel();


        private MusicViewModel _musicViewModel;

        public MusicViewModel MusicViewModel
        {
            get => _musicViewModel;
            set
            {
                _musicViewModel = value;
                OnPropertyChanged();
            }
        }

        private byte _selectedTabIndex;

        public byte SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                if (value == _selectedTabIndex) return;
                _selectedTabIndex = value;
                OnPropertyChanged();
            }
        }


        public static MainViewModel GetInstance()
        {
            return mainViewModel;
        }
    }
}
