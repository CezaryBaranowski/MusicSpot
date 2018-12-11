using MahApps.Metro.Controls;
using MusicSpot.Models;
using MusicSpot.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace MusicSpot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static bool _applicationLoaded = false;

        public MainWindow()
        {
            //var runArgs = Environment.GetCommandLineArgs();
            InitializeComponent();
            this.DataContext = MainViewModel.GetInstance();
            this.Loaded += OnLoaded;
        }

        private void LaunchMenu(object sender, RoutedEventArgs e)
        {
            MainViewModel.GetInstance().IsSettingFlyoutOpen = !MainViewModel.GetInstance().IsSettingFlyoutOpen;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            await Task.Run(() => PlaylistManager.InitPlaylists());
            await MusicViewModel.GetInstance().RefreshMusicDirectoriesAndLoadSongsAsync();
            _applicationLoaded = true;
        }

        public static bool IsApplicationLoaded()
        {
            return _applicationLoaded;
        }
    }
}
