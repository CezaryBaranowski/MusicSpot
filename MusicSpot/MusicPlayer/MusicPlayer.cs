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

        public static AudioFileReader audioFileReader;
        //private static NAudio.Vorbis.VorbisWaveReader vorbisAudioFileReader;

        private static readonly System.Timers.Timer timer = new System.Timers.Timer(100);

        public static void PlayAudioFile(string fileName)
        {
            CloseWaveOut();
            waveOutDevice = new NAudio.Wave.WaveOut();
            audioFileReader = new AudioFileReader(fileName);
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
            timer.Elapsed += UpdateCurrentTrackTime;
            timer.AutoReset = true;
            timer.Enabled = true;
            waveOutDevice.PlaybackStopped += SongEndedAction;
            MusicViewModel.GetInstance().TrackTotalTime = audioFileReader.TotalTime;
            MusicViewModel.GetInstance().IsPlaying = true;

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
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
            }
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

        public static bool CanClickPlayPauseButton(object parameter)    // May cause bug - test it 
        {
            if (MusicViewModel.GetInstance().CurrentlySelectedSong != null)
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
                    }
                    else ResumeAudioFile();
                }
                else
                {
                    PlayAudioFile(currentlySelectedSong.Path);
                    MusicViewModel.GetInstance().CurrentlyPlayedSong = currentlySelectedSong;
                }
            }
            else
            {
                PauseAudioFile();
            }
        }

        public static void MouseDoubleClickPlayAction(object parameter)
        {
            var currentlySelectedSong = MusicViewModel.GetInstance().CurrentlySelectedSong;
            PlayAudioFile(currentlySelectedSong.Path);
            MusicViewModel.GetInstance().CurrentlyPlayedSong = currentlySelectedSong;
        }

        public static bool CanClickNextSongButton(object parameter)
        {
            var currentlyPlayedSong = MusicViewModel.GetInstance().CurrentlyPlayedSong;
            var currentlyPlayedSongIndex = MusicViewModel.GetInstance().FilteredSongs.IndexOf(currentlyPlayedSong);
            if (currentlyPlayedSongIndex < MusicViewModel.GetInstance().FilteredSongs.Count - 1)
                return true;
            return false;
        }

        public static void NextSongAction(object parameter)
        {
            var currentlyPlayedSong = MusicViewModel.GetInstance().CurrentlyPlayedSong;
            var index = MusicViewModel.GetInstance().FilteredSongs.IndexOf(currentlyPlayedSong) + 1;
            var nextlyPlayedSong = MusicViewModel.GetInstance().FilteredSongs.ElementAt(index);
            MusicViewModel.GetInstance().CurrentlySelectedSong = nextlyPlayedSong;
            MusicViewModel.GetInstance().CurrentlyPlayedSong = nextlyPlayedSong;
            PlayAudioFile(nextlyPlayedSong.Path);
        }

        public static bool CanClickPreviousSongButton(object parameter)
        {
            var currentlyPlayedSong = MusicViewModel.GetInstance().CurrentlyPlayedSong;
            var currentlyPlayedSongIndex = MusicViewModel.GetInstance().FilteredSongs.IndexOf(currentlyPlayedSong);
            if (currentlyPlayedSongIndex >= 1)
                return true;
            return false;
        }

        public static void PreviousSongAction(object parameter)
        {
            var currentlyPlayedSong = MusicViewModel.GetInstance().CurrentlyPlayedSong;
            var index = MusicViewModel.GetInstance().FilteredSongs.IndexOf(currentlyPlayedSong) - 1;
            var nextlyPlayedSong = MusicViewModel.GetInstance().FilteredSongs.ElementAt(index);
            MusicViewModel.GetInstance().CurrentlySelectedSong = nextlyPlayedSong;
            MusicViewModel.GetInstance().CurrentlyPlayedSong = nextlyPlayedSong;
            PlayAudioFile(nextlyPlayedSong.Path);
        }

        public static void SongEndedAction(object parameter, StoppedEventArgs e)
        {
            if (CanClickNextSongButton(null))
                NextSongAction(null);
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

    }
}
