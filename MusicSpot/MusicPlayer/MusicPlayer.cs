using MusicSpot.ViewModels;
using NAudio.Wave;
using System;
using System.Timers;

namespace MusicSpot.MusicPlayer
{
    static class MusicPlayer
    {

        // playing device representation
        public static IWavePlayer waveOutDevice = new NAudio.Wave.WaveOutEvent();

        public static AudioFileReader audioFileReader;
        private static NAudio.Vorbis.VorbisWaveReader vorbisAudioFileReader;

        private static readonly System.Timers.Timer timer = new System.Timers.Timer(100);

        public static void PlayAudioFile(string fileName)
        {
            if (!fileName.EndsWith(".ogg"))
            {
                CloseWaveOut();
                waveOutDevice = new NAudio.Wave.WaveOut();
                audioFileReader = new AudioFileReader(fileName);
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();
                timer.Elapsed += UpdateCurrentTrackTime;
                timer.AutoReset = true;
                timer.Enabled = true;
            }
            else
            {
                CloseWaveOut();
                waveOutDevice = new NAudio.Wave.WaveOutEvent();
                vorbisAudioFileReader = new NAudio.Vorbis.VorbisWaveReader(fileName);
                waveOutDevice.Init(vorbisAudioFileReader);
                waveOutDevice.Play();
            }
        }
        public static void PauseAudioFile()
        {
            waveOutDevice?.Pause();
            Console.WriteLine(audioFileReader.CurrentTime.ToString());
        }
        public static void ResumeAudioFile()
        {
            waveOutDevice?.Play();
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
            if (vorbisAudioFileReader != null)
            {
                vorbisAudioFileReader.Dispose();
                vorbisAudioFileReader = null;
            }
            if (waveOutDevice != null)
            {
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
        }

        public static bool CanClickPlayPauseButton(object parameter)
        {
            if (MusicViewModel.GetInstance().CurrentlySelectedSong != null)
                return true;
            return false;
        }

        public static void PlayPauseButtonAction(object parameter)  // REFACTOR!!!
        {
            if (MusicViewModel.GetInstance().IsPlaying == false)        // if player stopped or paused
            {
                var currentlyPlayedSong = MusicViewModel.GetInstance().CurrentlyPlayedSong;
                if (currentlyPlayedSong != null && MusicViewModel.GetInstance().CurrentlySelectedSong.Path
                    .Equals(currentlyPlayedSong.Path))
                {
                    ResumeAudioFile();
                    MusicViewModel.GetInstance().IsPlaying = true;
                    MusicViewModel.GetInstance().PlayControlOn = false;
                }
                else
                {
                    var currentSong = MusicViewModel.GetInstance().CurrentlySelectedSong;
                    PlayAudioFile(currentSong.Path);
                    MusicViewModel.GetInstance().CurrentlyPlayedSong = currentSong;
                    MusicViewModel.GetInstance().TrackTotalTime = audioFileReader.TotalTime;
                    MusicViewModel.GetInstance().IsPlaying = true;
                    MusicViewModel.GetInstance().PlayControlOn = false;
                }

            }
            else if (MusicViewModel.GetInstance().PlayControlOn)
            {
                var currentSong = MusicViewModel.GetInstance().CurrentlySelectedSong;
                PlayAudioFile(currentSong.Path);
                MusicViewModel.GetInstance().CurrentlyPlayedSong = currentSong;
                MusicViewModel.GetInstance().TrackTotalTime = audioFileReader.TotalTime;
                MusicViewModel.GetInstance().IsPlaying = true;
                MusicViewModel.GetInstance().PlayControlOn = false;
            }
            else
            {
                PauseAudioFile();
                MusicViewModel.GetInstance().IsPlaying = false;
                MusicViewModel.GetInstance().PlayControlOn = true;
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

        private static void UpdateCurrentTrackTime(Object source, ElapsedEventArgs e)
        {
            MusicViewModel.GetInstance().TrackPosition = audioFileReader.CurrentTime;
        }

    }
}
