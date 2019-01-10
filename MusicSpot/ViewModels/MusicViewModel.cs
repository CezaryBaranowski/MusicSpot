using MusicSpot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using File = TagLib.File;
using Image = System.Drawing.Image;

namespace MusicSpot.ViewModels
{
    public sealed class MusicViewModel : ViewModelBase
    {

        private static readonly MusicViewModel musicViewModel = new MusicViewModel();


        static MusicViewModel()
        {
        }

        private MusicViewModel()
        {
            InitMusicViewModel();
        }

        public static MusicViewModel GetInstance()
        {
            return musicViewModel;
        }

        private void InitMusicViewModel()
        {
            IsPlaying = false;
            PlayControlOn = true;
            IsMuted = false;
            IsEditingFilesEnabled = true;
            Volume = 50.0f;
            FilteredSongs = new ObservableCollection<Song>();
            SongsToFilter = new ObservableCollection<Song>();
            //RefreshMusicDirectoriesAndLoadSongsAsync();
            MyMusicGenres = new ObservableCollection<string> { "All" };
            PlaylistsNames.Add("None");
            SelectedPlaylistName = "None";
            MusicSelectedDirectory = "All";
            SelectedGenre = "All";
        }

        public void RefreshSelectedSong()
        {
            CurrentlySelectedSong = CurrentlyPlayedSong;
        }

        #region Commands

        private ICommand playMusicCommand;

        public ICommand PlayMusicCommand => playMusicCommand ??
                                            (playMusicCommand = new SimpleCommand
                                            {
                                                CanExecuteDelegate = MusicPlayer.MusicPlayer.CanClickPlayPauseButton,
                                                ExecuteDelegate = MusicPlayer.MusicPlayer.PlayPauseButtonAction
                                            });

        private ICommand muteCommand;

        public ICommand MuteCommand => muteCommand ??
                                            (muteCommand = new SimpleCommand
                                            {
                                                CanExecuteDelegate = x => true,
                                                ExecuteDelegate = MusicPlayer.MusicPlayer.MuteSoundAction
                                            });

        private ICommand nextSongCommand;

        public ICommand NextSongCommand => nextSongCommand ??
                                            (nextSongCommand = new SimpleCommand
                                            {
                                                CanExecuteDelegate = MusicPlayer.MusicPlayer.CanClickNextSongButton,
                                                ExecuteDelegate = MusicPlayer.MusicPlayer.NextSongAction
                                            });

        private ICommand previousSongCommand;

        public ICommand PreviousSongCommand => previousSongCommand ??
                                            (previousSongCommand = new SimpleCommand
                                            {
                                                CanExecuteDelegate = MusicPlayer.MusicPlayer.CanClickPreviousSongButton,
                                                ExecuteDelegate = MusicPlayer.MusicPlayer.PreviousSongAction
                                            });

        private ICommand filterSongsCommand;

        public ICommand FilterSongsCommand => filterSongsCommand ??
                                              (filterSongsCommand = new SimpleCommand
                                              {
                                                  CanExecuteDelegate = CanFilterSongs,
                                                  ExecuteDelegate = FilterSongs
                                              });

        private ICommand removeSongFromPlaylistCommand;

        public ICommand RemoveSongFromPlaylistCommand => removeSongFromPlaylistCommand ??
                                                         (removeSongFromPlaylistCommand = new SimpleCommand
                                                         {
                                                             CanExecuteDelegate =
                                                                 PlaylistManager.CanRemoveSongFromPlaylist,
                                                             ExecuteDelegate = PlaylistManager.RemoveSongFromPlaylist
                                                         });

        #endregion

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

        private bool _playControlOn;
        public bool PlayControlOn
        {
            get => _playControlOn;
            set
            {
                _playControlOn = value;
                OnPropertyChanged();
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

        //all loaded songs
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

        //songs visible in datagrid inside view
        private ObservableCollection<Song> _filteredSongs = new ObservableCollection<Song>();
        public ObservableCollection<Song> FilteredSongs
        {
            get => _filteredSongs;
            set
            {
                _filteredSongs = value;
                OnPropertyChanged();
            }
        }

        //temporary collection
        private ObservableCollection<Song> _songsToFilter = new ObservableCollection<Song>();
        public ObservableCollection<Song> SongsToFilter
        {
            get => _songsToFilter;
            set
            {
                _songsToFilter = value;
                OnPropertyChanged();
            }
        }

        private IList<Playlist> _playlists = new ObservableCollection<Playlist>();
        public IList<Playlist> Playlists
        {
            get => _playlists;
            set
            {
                _playlists = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _playlistsNames = new ObservableCollection<string>();
        public ObservableCollection<string> PlaylistsNames
        {
            get => _playlistsNames;
            set
            {
                _playlistsNames = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> PlaylistNamesInContextMenu
        {
            get => new ObservableCollection<string>(_playlistsNames.Skip(1));
        }

        private string _selectedPlaylistName;

        public string SelectedPlaylistName
        {
            get => _selectedPlaylistName;
            set
            {
                _selectedPlaylistName = value;
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

        private Song _currentlyPlayedSong;
        public Song CurrentlyPlayedSong
        {
            get => _currentlyPlayedSong;
            set
            {
                Song lastlyPlayedSong = _currentlyPlayedSong;
                _currentlyPlayedSong = value;
                if (_currentlyPlayedSong != null)
                {
                    File f = File.Create(CurrentlyPlayedSong.Path);
                    byte[] bin = null;
                    if (f.Tag.Pictures.Length > 0)
                        bin = f.Tag.Pictures[0].Data.Data;
                    CurrentlyPlayedSong.AlbumArt = (bin != null) ? Image.FromStream(new MemoryStream(bin)) : null;
                }
                DiscoveryViewModel.GetInstance().CurrentlyPlayedSong = CurrentlyPlayedSong;
                OnPropertyChanged();
                if (lastlyPlayedSong == null || !lastlyPlayedSong.Artist.Equals(_currentlyPlayedSong.Artist))
                {
                    if (DiscoveryViewModel.GetInstance().IsSpotifyApiInitialized())
                        DiscoveryViewModel.GetInstance().LoadNewArtistDetails(_currentlyPlayedSong.Artist);
                }
                else
                if (lastlyPlayedSong.Artist.Equals(_currentlyPlayedSong.Artist))
                {
                    if (DiscoveryViewModel.GetInstance().IsSpotifyApiInitialized())
                        DiscoveryViewModel.GetInstance().LoadsNewSongDetails();
                }

            }
        }

        private float _volume;
        public float Volume
        {
            get => _volume * 100;
            set
            {
                _volume = value / 100;
                MusicPlayer.MusicPlayer.waveOutDevice.Volume = _volume;
                OnPropertyChanged();
            }
        }

        private TimeSpan _trackPosition;

        public TimeSpan TrackPosition
        {
            get => _trackPosition;
            set
            {
                _trackPosition = value;
                OnPropertyChanged();
            }
        }

        private int _trackPositionSeconds;

        public int TrackPositionSeconds
        {
            get => _trackPosition.Seconds;
            set
            {
                _trackPositionSeconds = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _trackTotalTime;
        public TimeSpan TrackTotalTime
        {
            get => _trackTotalTime;
            set
            {
                _trackTotalTime = value;
                OnPropertyChanged();
            }

        }

        private bool _genresLoaded;

        public bool GenresLoaded
        {
            get => _genresLoaded;
            set
            {
                _genresLoaded = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _myMusicGenres;
        public ObservableCollection<string> MyMusicGenres
        {
            get => _myMusicGenres;
            set
            {
                _myMusicGenres = value;
                OnPropertyChanged();
            }
        }

        private string _selectedGenre;
        public string SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region MusicDirectoriesManipulating

        public IList<string> GetMusicDirectories()
        {
            if (MusicSelectedDirectory.Equals("All"))
                return SettingsViewModel.GetInstance().MusicDirectories;
            return new List<string> { MusicSelectedDirectory };
        }

        public void RefreshMusicDirectories()
        {
            _musicDirectories = new ObservableCollection<string>(SettingsViewModel.GetInstance().MusicDirectories);
            _musicDirectories.Insert(0, "All");
            OnPropertyChanged(nameof(MusicDirectories));
        }

        public async Task RefreshMusicDirectoriesAndLoadSongsAsync()
        {
            _musicDirectories = new ObservableCollection<string>(SettingsViewModel.GetInstance().MusicDirectories);
            _musicDirectories.Insert(0, "All");
            OnPropertyChanged(nameof(MusicDirectories));
            await Task.Run(() => LoadSongsToMusicView(LoadMusicFilesNamesFromDirectories(GetMusicDirectories())));
        }
        #endregion

        #region FilteringMethods

        public bool CanFilterSongs(object parameter)
        {
            string pattern;
            TextBox tb;
            if (parameter.ToString().StartsWith("System.Controls"))
            {
                tb = (TextBox)parameter;
                pattern = tb.Text;
            }
            else
                pattern = parameter.ToString();

            if (pattern.Length > 2 || pattern.Length == 0) return true;

            return false;
        }

        public void FilterSongs(object parameter)                   // Async may be needed
        {
            var pattern = (parameter != null) ? parameter.ToString() : "";
            if (pattern.Length > 2)
            {
                SongsToFilter = FilteredSongs;
                FilteredSongs = new ObservableCollection<Song>();
                LoadSongsByTitle(pattern);
                LoadSongsByArtist(pattern);
                if (FilteredSongs.Count == 0)
                    LoadSongsByAlbum(pattern);
            }
            else if (pattern.Length == 0)
            {
                SongsToFilter = Songs;
                LoadSongsByGenre();
            }
        }

        public void LoadSongsByTitle(string title)
        {
            foreach (var song in SongsToFilter)
            {
                if (song.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    FilteredSongs.Add(song);
                }
            }
        }
        public void LoadSongsByArtist(string artist)
        {
            foreach (var song in SongsToFilter)
            {
                if (song.Artist.IndexOf(artist, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    FilteredSongs.Add(song);
                }
            }
        }
        public void LoadSongsByAlbum(string album)
        {
            foreach (var song in SongsToFilter)
            {
                if (song.Album?.IndexOf(album, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    FilteredSongs.Add(song);
                }
            }
        }

        public void LoadSongsByGenre()
        {
            string genre = SelectedGenre;
            if (!genre.Equals("All"))
            {
                SongsToFilter = Songs;
                FilteredSongs = new ObservableCollection<Song>();
                var songsToAdd = SongsToFilter.Where(s =>
                    s.Genres.Any(g => g.IndexOf(genre, StringComparison.OrdinalIgnoreCase) >= 0));
                foreach (var song in songsToAdd)
                {
                    FilteredSongs.Add(song);
                }
            }
            else FilteredSongs = SongsToFilter;
        }

        #endregion

        #region SongsLoading

        public async void LoadSongsFromPlaylistToMusicView()
        {
            string currentlySelectedPlaylistName = SelectedPlaylistName;

            if (!currentlySelectedPlaylistName.Equals("None"))
            {
                Playlist selectedPlaylist = Playlists
                .FirstOrDefault(p => (p.Name.Equals(currentlySelectedPlaylistName)));

                if (selectedPlaylist != null)
                {
                    IEnumerable<string> musicFilesStrings = selectedPlaylist.Songs.Select(s => s.Path);
                    LoadSongsToMusicView(musicFilesStrings);
                }
            }
            else await Task.Run(() => LoadSongsToMusicView(LoadMusicFilesNamesFromDirectories(GetMusicDirectories())));
        }

        public IEnumerable<string> LoadMusicFilesNamesFromDirectories(ICollection<string> directories)
        {
            IEnumerable<string> musicFiles = new List<string>();

            foreach (string directory in directories)
            {
                IEnumerable<string> files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

                musicFiles = musicFiles.Concat((files).Where(f =>
                    new[] { ".mp3", ".mp4", ".flac", ".wma", ".wav" }.Contains(Path.GetExtension(f))));
            }

            return musicFiles;
        }

        public void LoadSongsToMusicView(IEnumerable<string> musicFilesStrings)
        {

            Songs = new ObservableCollection<Song>();
            FilteredSongs = new ObservableCollection<Song>();
            GenresLoaded = false;
            var filesStrings = musicFilesStrings as string[] ?? musicFilesStrings.ToArray();
            if (!filesStrings.Any())
                return;

            foreach (var musicFile in filesStrings)
            {
                File f = File.Create(musicFile);

                var song = new Song
                {
                    Title = f.Tag.Title,
                    Album = f.Tag.Album,
                    Artist = f.Tag.JoinedPerformers,
                    Path = f.Name,
                    TotalTime = f.Properties.Duration,
                    Genres = f.Tag.Genres
                };
                if (song.Path != null && song.Artist != null && song.Title != null &&
                    song.TotalTime.TotalSeconds > 0)
                {
                    Songs.Add(song);

                    Application.Current.Dispatcher.BeginInvoke((Action)delegate { FilteredSongs.Add(song); });
                }
            }
        }
        #endregion

    }
}