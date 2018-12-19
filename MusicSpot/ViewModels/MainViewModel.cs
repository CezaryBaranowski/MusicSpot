namespace MusicSpot.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private MainViewModel()
        {
            SelectedTabIndex = 0;
            SettingsViewModel = SettingsViewModel.GetInstance();
            MusicViewModel = MusicViewModel.GetInstance();
            DiscoveryViewModel = DiscoveryViewModel.GetInstance();
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

        private DiscoveryViewModel _discoveryViewModel;

        public DiscoveryViewModel DiscoveryViewModel
        {
            get => _discoveryViewModel;
            set
            {
                _discoveryViewModel = value;
                OnPropertyChanged();
            }
        }

        private SettingsViewModel _settingViewModel;

        public SettingsViewModel SettingsViewModel
        {
            get => _settingViewModel;
            set
            {
                _settingViewModel = value;
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

        private bool _isSettingFlyoutOpen;

        public bool IsSettingFlyoutOpen
        {
            get { return _isSettingFlyoutOpen; }
            set
            {
                if (value == _isSettingFlyoutOpen) return;
                _isSettingFlyoutOpen = value;
                OnPropertyChanged();
            }
        }


        public static MainViewModel GetInstance()
        {
            return mainViewModel;
        }
    }
}
