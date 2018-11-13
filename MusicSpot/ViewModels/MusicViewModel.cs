using MusicSpot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application = System.Windows.Application;

namespace MusicSpot.ViewModels
{
    public sealed class MusicViewModel : ViewModelBase
    {

        private static readonly MusicViewModel musicViewModel = new MusicViewModel();

        private MusicViewModel()
        {
            IsPlaying = false;
            IsMuted = false;
            IsEditingFilesEnabled = true;
            MusicSelectedDirectory = "All";
            RefreshMusicDirectoriesAsync();
        }

        public static MusicViewModel GetInstance()
        {
            return musicViewModel;
        }

        public void LoadMusicFiles()
        {
            ICollection<string> directories = new List<string>(GetMusicDirectories());
            IEnumerable<string> files = null;
            Songs = new ObservableCollection<Song>();
            foreach (string directory in directories)
            {
                files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

                var musicFiles = (files).Where(f =>
                    new[] { ".mp3", ".mp4", ".ogg", ".flac", ".wma", ".wav" }.Contains(Path.GetExtension(f)));

                foreach (var musicFile in musicFiles)
                {
                    TagLib.File f = TagLib.File.Create(musicFile);
                    byte[] bin = null;
                    if (f.Tag.Pictures.Length > 0)
                        bin = (byte[])(f.Tag.Pictures[0].Data.Data);

                    var song = new Song
                    {
                        Title = f.Tag.Title,
                        Album = f.Tag.Album,
                        Artist = f.Tag.JoinedPerformers,
                        Path = f.Name,
                        AlbumArt = (bin != null) ? Image.FromStream(new MemoryStream(bin)) : null,
                        TotalTime = f.Properties.Duration,
                    };
                    Application.Current.Dispatcher.BeginInvoke((Action)delegate
                    {
                        //if (typeof(Song).GetProperties().All(propertyInfo => propertyInfo.GetValue(song) != null))
                        Songs.Add(song);
                    });
                }
            }

        }

        #region Properties

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

        private bool _isMuted;
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                OnPropertyChanged(nameof(IsMuted));
            }
        }

        private bool _isEditingFilesEnabled;
        public bool IsEditingFilesEnabled
        {
            get => _isEditingFilesEnabled;
            set
            {
                _isEditingFilesEnabled = value;
                OnPropertyChanged(nameof(IsEditingFilesEnabled));
            }
        }

        private ObservableCollection<string> _musicDirectories;
        public ObservableCollection<string> MusicDirectories
        {
            get => _musicDirectories;
            set
            {
                _musicDirectories = value;
                OnPropertyChanged();
            }
        }

        private string _musicSelectedDirectory;
        public string MusicSelectedDirectory
        {
            get => _musicSelectedDirectory;
            set
            {
                _musicSelectedDirectory = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Song> _songs = new ObservableCollection<Song>();

        public ObservableCollection<Song> Songs
        {
            get => _songs;
            set
            {
                _songs = value;
                OnPropertyChanged();
            }
        }

        private Song _currentlySelectedSong;

        public Song CurrentlySelectedSong
        {
            get => _currentlySelectedSong;
            set
            {
                _currentlySelectedSong = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region MusicDirectoriesManipulating

        private IList<string> GetMusicDirectories()
        {
            if (MusicSelectedDirectory.Equals("All")) return SettingsViewModel.GetInstance().MusicDirectories;
            return new List<string> { MusicSelectedDirectory };
        }

        public void RefreshMusicDirectoriesAsync()
        {
            _musicDirectories = new ObservableCollection<string>(SettingsViewModel.GetInstance().MusicDirectories);
            _musicDirectories.Insert(0, "All");
            OnPropertyChanged(nameof(MusicDirectories));
            Task.Run(() => LoadMusicFiles());


            //var uiContext = SynchronizationContext.Current;
            //Task.Run(() => { uiContext.Send(x => LoadMusicFiles(), null); });
        }
        #endregion
    }
}