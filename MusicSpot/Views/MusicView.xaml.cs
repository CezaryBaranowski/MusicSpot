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
            MusicViewModel.GetInstance().RefreshSelectedSong();
        }

        private void MainMusicDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.DataGrid dg = sender as DataGrid;
            object dgc = dg.CurrentItem;
            MusicPlayer.MusicPlayer.MouseDoubleClickPlayAction(dgc);

        }

        private void MusicProgressBar_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Math.Abs(e.OldValue - e.NewValue) > 2)
                MusicPlayer.MusicPlayer.audioFileReader.CurrentTime = new TimeSpan(0, 0, 0, (int)e.NewValue);
        }

        private void SearchBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                var tb = (TextBox)sender;
                if (tb != null)
                {
                    tb.Text = string.Empty;
                    MusicViewModel.GetInstance().FilterSongs(null);
                }
            }
        }

        private void MusicGenresComboBox_OnGotMouseCapture(object sender, MouseEventArgs e)
        {
            if (!MusicViewModel.GetInstance().GenresLoaded)
                MusicViewModel.GetInstance().InitGenres();
            MusicViewModel.GetInstance().GenresLoaded = true;
        }

        private void MusicGenresComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MusicViewModel.GetInstance().LoadSongsByGenre();
        }
    }
}
