﻿using MusicSpot.Models;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicSpot.ViewModels
{
    public sealed class MusicViewModel : ViewModelBase
    {

        private static readonly MusicViewModel musicViewModel = new MusicViewModel();

        public static IWavePlayer MusicPlayerController = MusicPlayer.MusicPlayer.waveOutDevice;

        private MusicViewModel()
        {
            IsPlaying = false;
            PlayControlOn = true;
            IsMuted = false;
            IsEditingFilesEnabled = true;
            Volume = 100.0f;
            MusicSelectedDirectory = "All";
            RefreshMusicDirectoriesAsync();
        }

        public static MusicViewModel GetInstance()
        {
            return musicViewModel;
        }

        #region Commands

        private ICommand playMusicCommand;

        public ICommand PlayMusicCommand => this.playMusicCommand ??
                                            (playMusicCommand = new SimpleCommand
                                            {
                                                CanExecuteDelegate = MusicPlayer.MusicPlayer.CanClickPlayPauseButton,
                                                ExecuteDelegate = MusicPlayer.MusicPlayer.PlayPauseButtonAction
                                            });

        private ICommand muteCommand;

        public ICommand MuteCommand => this.muteCommand ??
                                            (muteCommand = new SimpleCommand
                                            {
                                                CanExecuteDelegate = x => true,
                                                ExecuteDelegate = MusicPlayer.MusicPlayer.MuteSoundAction
                                            });

        private ICommand nextSongCommand;

        public ICommand NextSongCommand => this.nextSongCommand ??
                                            (nextSongCommand = new SimpleCommand
                                            {
                                                CanExecuteDelegate = MusicPlayer.MusicPlayer.CanClickNextSongButton,
                                                ExecuteDelegate = MusicPlayer.MusicPlayer.NextSongAction
                                            });

        private ICommand previousSongCommand;

        public ICommand PreviousSongCommand => this.previousSongCommand ??
                                            (previousSongCommand = new SimpleCommand
                                            {
                                                CanExecuteDelegate = MusicPlayer.MusicPlayer.CanClickPreviousSongButton,
                                                ExecuteDelegate = MusicPlayer.MusicPlayer.PreviousSongAction
                                            });

        private ICommand filterSongsCommand;

        public ICommand FilterSongsCommand => this.filterSongsCommand ??
                                              (filterSongsCommand = new SimpleCommand
                                              {
                                                  CanExecuteDelegate = CanFilterSongs,
                                                  ExecuteDelegate = FilterSongs
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
                _currentlyPlayedSong = value;
                if (_currentlySelectedSong != null)
                {
                    TagLib.File f = TagLib.File.Create(CurrentlySelectedSong.Path);
                    byte[] bin = null;
                    if (f.Tag.Pictures.Length > 0)
                        bin = (byte[])(f.Tag.Pictures[0].Data.Data);
                    CurrentlySelectedSong.AlbumArt = (bin != null) ? Image.FromStream(new MemoryStream(bin)) : null;
                }
                OnPropertyChanged();
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

        #endregion

        #region MusicDirectoriesManipulating

        private IList<string> GetMusicDirectories()
        {
            if (MusicSelectedDirectory.Equals("All"))
                return SettingsViewModel.GetInstance().MusicDirectories;
            return new List<string> { MusicSelectedDirectory };
        }

        public async void RefreshMusicDirectoriesAsync()
        {
            _musicDirectories = new ObservableCollection<string>(SettingsViewModel.GetInstance().MusicDirectories);
            _musicDirectories.Insert(0, "All");
            OnPropertyChanged(nameof(MusicDirectories));
            await Task.Run(() => LoadMusicFiles());

            //var uiContext = SynchronizationContext.Current;
            //Task.Run(() => { uiContext.Send(x => LoadMusicFiles(), null); });
        }
        #endregion

        #region FilteringMethods

        public bool CanFilterSongs(object parameter)
        {
            string pattern;
            System.Windows.Controls.TextBox tb;
            if (parameter.ToString().StartsWith("System.Controls"))
            {
                tb = (System.Windows.Controls.TextBox)parameter;
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
            FilteredSongs = new ObservableCollection<Song>();
            if (pattern.Length > 2)
            {
                LoadSongsByTitle(pattern);
                LoadSongsByArtist(pattern);
                if (FilteredSongs.Count == 0)
                    LoadSongsByAlbum(pattern);
            }
            else if (pattern.Length == 0)
                FilteredSongs = Songs;
        }

        public void LoadSongsByTitle(string title)
        {
            //FilteredSongs = Songs.Where(song => song.Title.Contains(title)) as ObservableCollection<Song>;
            foreach (var song in Songs)
            {
                if (song.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    FilteredSongs.Add(song);
                }
            }
        }
        public void LoadSongsByArtist(string artist)
        {
            foreach (var song in Songs)
            {
                if (song.Artist.IndexOf(artist, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    FilteredSongs.Add(song);
                }
            }
        }
        public void LoadSongsByAlbum(string album)
        {
            foreach (var song in Songs)
            {
                if (song.Album.IndexOf(album, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    FilteredSongs.Add(song);
                }
            }
        }

        #endregion

        public void RefreshSelectedSong()
        {
            CurrentlySelectedSong = CurrentlyPlayedSong;
        }

        public void LoadMusicFiles()
        {
            ICollection<string> directories = new List<string>(GetMusicDirectories());
            IEnumerable<string> files = null;
            Songs = new ObservableCollection<Song>();
            FilteredSongs = new ObservableCollection<Song>();
            foreach (string directory in directories)
            {
                files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

                var musicFiles = (files).Where(f =>
                    new[] { ".mp3", ".mp4", ".flac", ".wma", ".wav" }.Contains(Path.GetExtension(f)));

                foreach (var musicFile in musicFiles)
                {
                    TagLib.File f = TagLib.File.Create(musicFile);

                    var song = new Song
                    {
                        Title = f.Tag.Title,
                        Album = f.Tag.Album,
                        Artist = f.Tag.JoinedPerformers,
                        Path = f.Name,
                        TotalTime = f.Properties.Duration,
                    };
                    if (song.Path != null && song.Artist != null && song.Title != null && song.TotalTime.TotalSeconds > 0)
                        Songs.Add(song);

                    Application.Current.Dispatcher.BeginInvoke((Action)delegate
                    {
                        FilteredSongs.Add(song);
                    });
                }
            }

        }
    }
}