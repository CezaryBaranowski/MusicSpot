using MusicSpot.MahAppsMetro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;


namespace MusicSpot.ViewModels
{
    public sealed class SettingsViewModel : ViewModelBase
    {

        private static readonly SettingsViewModel _settingsViewModel = new SettingsViewModel();
        public List<AppThemeMenuData> AppThemes { get; set; }
        public List<AccentColorMenuData> AccentColors { get; set; }

        private SettingsViewModel()
        {
            MusicDirectories = new List<string>();
            VideoDirectories = new LinkedList<string>();
            PictureDirectories = new LinkedList<string>();

            AddMusicDirectory(@"C:\Programowanie\NET\MusicSpotTestMedia");
            AddMusicDirectory(@"E:\Muzyka\Muzyka\Klasyczna");

            // create accent color menu items for the demo
            this.AccentColors = ThemeManager.Accents
                .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                .ToList();

            // create metro theme color menu items for the demo
            this.AppThemes = ThemeManager.AppThemes
                .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as Brush, ColorBrush = a.Resources["WhiteColorBrush"] as Brush })
                .ToList();

        }

        public static SettingsViewModel GetInstance()
        {
            return _settingsViewModel;

        }

        #region MediaDirectories

        private List<string> _musicDirectories;

        public List<string> MusicDirectories
        {
            get => _musicDirectories;
            private set
            {
                _musicDirectories = value;
                OnPropertyChanged();
            }
        }

        public void AddMusicDirectory(string directory)
        {
            _musicDirectories.Add(directory);
            OnPropertyChanged("MusicDirectories");
        }

        public void RemoveMusicDirectory(string directory)
        {
            _musicDirectories.Remove(directory);
            OnPropertyChanged("MusicDirectories");
        }

        private LinkedList<string> _videoDirectories;
        public LinkedList<string> VideoDirectories
        {
            get => _videoDirectories;
            private set => _videoDirectories = value;
        }

        public void AddVideoDirectory(string directory)
        {
            VideoDirectories.AddLast(directory);
        }

        public void RemoveVideoDirectory(string directory)
        {
            VideoDirectories.Remove(directory);
        }


        private LinkedList<string> _pictureDirectories;
        public LinkedList<string> PictureDirectories
        {
            get => _pictureDirectories;
            private set => _pictureDirectories = value;
        }

        public void AddPictureDirectory(string directory)
        {
            PictureDirectories.AddLast(directory);
        }

        public void RemovePictureDirectory(string directory)
        {
            PictureDirectories.Remove(directory);
        }
        #endregion

    }
}
