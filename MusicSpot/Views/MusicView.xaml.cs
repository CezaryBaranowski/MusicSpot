using MusicSpot.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicSpot.Views
{
    /// <summary>
    /// Interaction logic for Music.xaml
    /// </summary>
    public partial class MusicView : UserControl
    {
        public MusicViewModel dataModel;

        public MusicView()
        {
            InitializeComponent();
            dataModel = MusicViewModel.GetInstance();
            this.DataContext = dataModel;
        }

        private void ChangeMusicDirectory(object sender, SelectionChangedEventArgs e)
        {
            MusicViewModel.GetInstance().LoadMusicFiles();
        }

        private void MainMusicDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MusicPlayer.MusicPlayer.MouseDoubleClickPlayAction(null);
        }

        private void MusicProgressBar_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Math.Abs(e.OldValue - e.NewValue) > 1.0)
                MusicPlayer.MusicPlayer.audioFileReader.CurrentTime = new TimeSpan(0, 0, 0, (int)e.NewValue);
            //var timespan = new TimeSpan(0, 0, 0, 140);
        }
    }
}
