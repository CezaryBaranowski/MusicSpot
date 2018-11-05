using MusicSpot.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MusicSpot.ViewModels
{
    public sealed class MusicViewModel : ViewModelBase
    {

        private static readonly MusicViewModel musicViewModel = new MusicViewModel();

        private MusicViewModel()
        {
            IsPlaying = false;
            IsMuted = false;
            IsEditingFilesEnabled = false;
            MusicSelectedDirectory = "All";
            RefreshMusicDirectories();
            LoadMusicFiles();
        }

        public static MusicViewModel GetInstance()
        {
            return musicViewModel;
        }

        public void LoadMusicFiles()
        {
            IList<string> directories = new List<string>(GetMusicDirectories());
            IEnumerable<string> files = null;
            foreach (string directory in directories)
            {
                files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            }

            var musicFiles = (files).Where(f =>
                new[] { ".mp3", ".mp4", ".ogg", ".flac", ".wmv", ".wav" }.Contains(Path.GetExtension(f)));

            Songs = new ObservableCollection<Song>();

            foreach (var musicFile in musicFiles)
            {
                TagLib.File f = TagLib.File.Create(musicFile);
                Songs.Add(new Song
                {
                    Title = f.Tag.Title,
                    Album = f.Tag.Album,
                    Artist = f.Tag.JoinedPerformers,
                    TotalTime = f.Properties.Duration

                });
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

        private bool _isEditingFilesEnables;
        public bool IsEditingFilesEnabled
        {
            get => _isEditingFilesEnables;
            set
            {
                _isEditingFilesEnables = value;
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

        private ObservableCollection<Song> _songs;

        public ObservableCollection<Song> Songs
        {
            get => _songs;
            set
            {
                _songs = value;
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

        public void RefreshMusicDirectories()
        {
            _musicDirectories = new ObservableCollection<string>(SettingsViewModel.GetInstance().MusicDirectories);
            _musicDirectories.Insert(0, "All");
            OnPropertyChanged(nameof(MusicDirectories));
        }
        #endregion
    }
}
