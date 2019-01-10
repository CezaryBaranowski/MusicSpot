using MusicSpot.Models;
using MusicSpot.ViewModels;
using NAudio.Wave;
using System;
using System.Linq;
using System.Timers;

namespace MusicSpot.MusicPlayer
{
    static class MusicPlayer
    {
        // playing device representation
        public static IWavePlayer waveOutDevice = new NAudio.Wave.WaveOutEvent();

        //public static AudioFileReader audioFileReader;
        public static MediaFoundationReader audioFileReader;
        //private static NAudio.Vorbis.VorbisWaveReader vorbisAudioFileReader;

        private static readonly System.Timers.Timer timer = new System.Timers.Timer(50);

        private static int songIndex = 0;

        public static void PlayAudioFile(string fileName)
        {
            CloseWaveOut();
            waveOutDevice = new NAudio.Wave.WaveOut();
            audioFileReader = new MediaFoundationReader(fileName);
            //audioFileReader = new AudioFileReader(fileName);
            MusicViewModel.GetInstance().TrackTotalTime = audioFileReader.TotalTime;
            MusicViewModel.GetInstance().IsPlaying = true;
            audioFileReader.Position = 0;
            waveOutDevice.PlaybackStopped += SongEndedAction;
            timer.Elapsed += UpdateCurrentTrackTime;
            timer.AutoReset = true;
            waveOutDevice.Init(audioFileReader);
            timer.Enabled = true;
            waveOutDevice.Play();
        }
        public static void PauseAudioFile()
        {
            waveOutDevice?.Pause();
            MusicViewModel.GetInstance().IsPlaying = false;
        }
        public static void ResumeAudioFile()
        {
            waveOutDevice?.Play();
            MusicViewModel.GetInstance().IsPlaying = true;
        }

        private static void CloseWaveOut()
        {
            waveOutDevice?.Stop();

            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }

            if (waveOutDevice != null)
            {
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
        }

        public static void StopPlayer()
        {
            var instance = MusicViewModel.GetInstance();
            instance.CurrentlySelectedSong =
            instance.FilteredSongs.FirstOrDefault();
            instance.IsPlaying = false;
            instance.TrackPosition = TimeSpan.Zero;
            instance.TrackTotalTime = TimeSpan.Zero;
            instance.CurrentlyPlayedSong = null;
        }

        public static bool CanClickPlayPauseButton(object parameter)    // May cause bug - test it 
        {
            if (MusicViewModel.GetInstance().CurrentlySelectedSong != null || waveOutDevice.PlaybackState == PlaybackState.Playing)
                return true;
            return false;
        }

        public static void PlayPauseButtonAction(object parameter)
        {
            if (MusicViewModel.GetInstance().IsPlaying == false)        // if player stopped or paused
            {
                var currentlySelectedSong = MusicViewModel.GetInstance().CurrentlySelectedSong;
                var currentlyPlayedSong = MusicViewModel.GetInstance().CurrentlyPlayedSong;
                if (currentlyPlayedSong != null)
                {
                    if (currentlySelectedSong.Path.Equals(currentlyPlayedSong.Path) == false)
                    {
                        PlayAudioFile(currentlySelectedSong.Path);
                        MusicViewModel.GetInstance().CurrentlyPlayedSong = currentlySelectedSong;
                        songIndex = MusicViewModel.GetInstance().FilteredSongs.IndexOf(currentlySelectedSong);
                    }
                    else ResumeAudioFile();
                }
                else
                {
                    PlayAudioFile(currentlySelectedSong.Path);
                    MusicViewModel.GetInstance().CurrentlyPlayedSong = currentlySelectedSong;
                    songIndex = MusicViewModel.GetInstance().FilteredSongs.IndexOf(currentlySelectedSong);
                }
            }
            else
            {
                PauseAudioFile();
            }
        }

        public static void MouseDoubleClickPlayAction(object parameter)
        {
            Song song = (Song)parameter;
            var currentSong = MusicViewModel.GetInstance().CurrentlyPlayedSong;
            if (currentSong == null || song.Path != currentSong.Path)
            {
                MusicViewModel.GetInstance().CurrentlyPlayedSong = song;
                songIndex = MusicViewModel.GetInstance().FilteredSongs.IndexOf(song);
                PlayAudioFile(song.Path);
            }
        }

        public static bool CanClickNextSongButton(object parameter)
        {
            var currentlyPlayedSongIndex = songIndex;
            if (currentlyPlayedSongIndex < MusicViewModel.GetInstance().FilteredSongs.Count - 1)
                return true;
            return false;
        }

        public static void NextSongAction(object parameter)
        {
            var newIndex = songIndex + 1;
            var nextlyPlayedSong = MusicViewModel.GetInstance().FilteredSongs.ElementAt(newIndex);
            MusicViewModel.GetInstance().CurrentlySelectedSong = nextlyPlayedSong;
            MusicViewModel.GetInstance().CurrentlyPlayedSong = nextlyPlayedSong;
            songIndex++;
            PlayAudioFile(nextlyPlayedSong.Path);
        }

        public static bool CanClickPreviousSongButton(object parameter)
        {
            var currentlyPlayedSongIndex = songIndex;
            if (currentlyPlayedSongIndex >= 1)
                return true;
            return false;
        }

        public static void PreviousSongAction(object parameter)
        {
            var newIndex = songIndex - 1;
            var nextlyPlayedSong = MusicViewModel.GetInstance().FilteredSongs.ElementAt(newIndex);
            MusicViewModel.GetInstance().CurrentlySelectedSong = nextlyPlayedSong;
            MusicViewModel.GetInstance().CurrentlyPlayedSong = nextlyPlayedSong;
            songIndex--;
            PlayAudioFile(nextlyPlayedSong.Path);
        }

        public static void SongEndedAction(object parameter, StoppedEventArgs e)
        {
            if (CanClickNextSongButton(null))
                NextSongAction(null);
            else
            {
                StopPlayer();
            }
        }

        public static void MuteSoundAction(object parameter)
        {
            if (!MusicViewModel.GetInstance().IsMuted)
            {
                MusicViewModel.GetInstance().IsMuted = true;
                waveOutDevice.Volume = 0.0f;
            }
            else
            if (MusicViewModel.GetInstance().IsMuted)
            {
                MusicViewModel.GetInstance().IsMuted = false;
                waveOutDevice.Volume = MusicViewModel.GetInstance().Volume / 100;
            }
        }

        public static void UpdateCurrentTrackTime(Object source, ElapsedEventArgs e)       // Method for timer
        {
            MusicViewModel.GetInstance().TrackPosition = audioFileReader.CurrentTime;

        }

        public static void RepositionSong(TimeSpan newTimespan)
        {
            audioFileReader.CurrentTime = newTimespan;
        }

    }
}
