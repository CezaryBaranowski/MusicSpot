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
        public MusicViewModel MusicViewModel { get; set; }


        private byte _selectedTabIndex;

        public byte SelectedTabIndex
        {
            get => _selectedTabIndex;
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
