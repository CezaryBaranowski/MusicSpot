﻿using MusicSpot.MahAppsMetro;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            MusicDirectories = new ObservableCollection<string>();
            VideoDirectories = new ObservableCollection<string>();
            PictureDirectories = new ObservableCollection<string>();

            Initialize();

            this.AccentColors = ThemeManager.Accents
                .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                .ToList();

            this.AppThemes = ThemeManager.AppThemes
                .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as Brush, ColorBrush = a.Resources["WhiteColorBrush"] as Brush })
                .ToList();

        }

        private void Initialize()
        {
            //         _musicDirectories.Add(@"E:\Muzyka\Muzyka\Klasyczna");
            _musicDirectories.Add(@"C:\Programowanie\NET\MusicSpotTestMedia");
            _videoDirectories.Add(@"C:\Programowanie\NET\MusicSpotTestMedia");
            _videoDirectories.Add(@"C:\Programowanie\NET");
            _pictureDirectories.Add(@"C:\Programowanie\NET\MusicSpotTestMedia");
        }

        public static SettingsViewModel GetInstance()
        {
            return _settingsViewModel;

        }

        #region MediaDirectories

        private ObservableCollection<string> _musicDirectories;

        public ObservableCollection<string> MusicDirectories
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
            MusicViewModel.GetInstance().RefreshMusicDirectories();
        }

        public void RemoveMusicDirectory(string directory)
        {
            _musicDirectories.Remove(directory);
            OnPropertyChanged("MusicDirectories");
            MusicViewModel.GetInstance().RefreshMusicDirectories();
        }

        private ObservableCollection<string> _videoDirectories;
        public ObservableCollection<string> VideoDirectories
        {
            get => _videoDirectories;
            private set => _videoDirectories = value;
        }

        public void AddVideoDirectory(string directory)
        {
            VideoDirectories.Add(directory);
        }

        public void RemoveVideoDirectory(string directory)
        {
            VideoDirectories.Remove(directory);
        }


        private ObservableCollection<string> _pictureDirectories;

        public ObservableCollection<string> PictureDirectories
        {
            get => _pictureDirectories;
            private set => _pictureDirectories = value;
        }

        public void AddPictureDirectory(string directory)
        {
            PictureDirectories.Add(directory);
        }

        public void RemovePictureDirectory(string directory)
        {
            PictureDirectories.Remove(directory);
        }

        private string _musicDirectoriesSelectedItem;
        public string MusicDirectoriesSelectedItem
        {
            get => _musicDirectoriesSelectedItem;
            set
            {
                _musicDirectoriesSelectedItem = value;
                OnPropertyChanged();
            }
        }

        private string _videoDirectoriesSelectedItem;
        public string VideoDirectoriesSelectedItem
        {
            get => _videoDirectoriesSelectedItem;
            set
            {
                _videoDirectoriesSelectedItem = value;
                OnPropertyChanged();
            }
        }

        private string _pictureDirectoriesSelectedItem;
        public string PictureDirectoriesSelectedItem
        {
            get => _pictureDirectoriesSelectedItem;
            set
            {
                _pictureDirectoriesSelectedItem = value;
                OnPropertyChanged();
            }
        }

        #endregion

    }
}
