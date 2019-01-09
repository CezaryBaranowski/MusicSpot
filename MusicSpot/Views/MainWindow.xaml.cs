using MahApps.Metro.Controls;
using MusicSpot.Models;
using MusicSpot.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace MusicSpot.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static bool _applicationLoaded = false;

        public MainWindow()
        {
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
            if (App.StartupSongsPaths.Count > 0)
            {
                MusicViewModel.GetInstance().LoadSongsToMusicView(App.StartupSongsPaths);
                MusicViewModel.GetInstance().RefreshMusicDirectories();
            }
            else
                await MusicViewModel.GetInstance().RefreshMusicDirectoriesAndLoadSongsAsync();

            _applicationLoaded = true;

            DiscoveryViewModel.GetInstance().InitSpotifyAPI();
        }

        public static bool IsApplicationLoaded()
        {
            return _applicationLoaded;
        }

        private void MusicProgressBar_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Math.Abs((UInt16)e.OldValue - (UInt16)e.NewValue) > 2)
                MusicPlayer.MusicPlayer.RepositionSong(new TimeSpan(0, 0, 0, (int)e.NewValue));
        }
    }
}
