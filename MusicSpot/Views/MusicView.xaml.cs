using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MusicSpot.Models;
using MusicSpot.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
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
            var addedItem = (string)e.AddedItems[0];
            if (!addedItem.Equals("All"))
            {
                var musicViewModel = MusicViewModel.GetInstance();
                musicViewModel.RefreshSelectedSong();
                 System.Threading.Tasks.Task.Run(() => musicViewModel.LoadSongsToMusicView(
                     musicViewModel.LoadMusicFilesNamesFromDirectories(musicViewModel.GetMusicDirectories())));

                //musicViewModel.LoadSongsToMusicView(
                //    musicViewModel.LoadMusicFilesNamesFromDirectories(musicViewModel.GetMusicDirectories()));
            }
        }

        private void MainMusicDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.DataGrid dg = sender as DataGrid;
            object dgc = dg.CurrentItem;
            if (dgc != null)
            {
                MusicPlayer.MusicPlayer.MouseDoubleClickPlayAction(dgc);
            }
        }

        private void MusicProgressBar_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Math.Abs((UInt16)e.OldValue - (UInt16)e.NewValue) > 2)
                MusicPlayer.MusicPlayer.RepositionSong(new TimeSpan(0, 0, 0, (int)e.NewValue));
        }

        private void SearchBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                var textBox = (TextBox)sender;
                if (textBox != null)
                {
                    textBox.Text = string.Empty;
                    MusicViewModel.GetInstance().FilterSongs(null);
                }
            }
        }

        private void MusicGenresComboBox_OnGotMouseCapture(object sender, MouseEventArgs e)
        {
            if (!MusicViewModel.GetInstance().GenresLoaded)
                GenresManager.InitGenres();
            MusicViewModel.GetInstance().GenresLoaded = true;
        }

        private void MusicGenresComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MusicViewModel.GetInstance().LoadSongsByGenre();
        }

        private void PlaylistsComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainWindow.IsApplicationLoaded() == false)
                return;

            var addedItem = (string)e.AddedItems[0];
            if (!addedItem.Equals("None"))
            {
                MusicViewModel.GetInstance().SelectedPlaylistName = addedItem;
                MusicViewModel.GetInstance().LoadSongsFromPlaylistToMusicView();
            }
            else
            {
                MusicViewModel.GetInstance().SelectedPlaylistName = addedItem;
                MusicViewModel.GetInstance().LoadSongsFromPlaylistToMusicView();
            }
        }

        private async void ShowPlaylistCreationDialog(object sender, RoutedEventArgs e)
        {
            var window = (MetroWindow)Window.GetWindow(this);

            var result = await window.ShowInputAsync("Write the name of your playlist", "Name");

            if (result == null) //user pressed cancel
                return;

            PlaylistManager.AddEmptyPlaylist(result);

            await window.ShowMessageAsync("", "Add song to playlist by right clicking on song and choosing option from context menu ");
        }

        private void ContextMenuPlay_OnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem mi = (MenuItem)e.OriginalSource;
            var actionName = (string)mi.Header;
            if (actionName.Equals("Play") && MusicViewModel.GetInstance().CurrentlySelectedSong != null)
            {
                MusicPlayer.MusicPlayer.PlayAudioFile(MusicViewModel.GetInstance().CurrentlySelectedSong.Path);
                MusicViewModel.GetInstance().CurrentlyPlayedSong = MusicViewModel.GetInstance().CurrentlySelectedSong;
            }
        }

        private void MenuItemAddToPlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            //playlist info
            System.Windows.Controls.MenuItem mi = (MenuItem)e.OriginalSource;
            var playlistName = (string)mi.Header;
            var song = MusicViewModel.GetInstance().CurrentlySelectedSong;
            PlaylistManager.AddSongToPlaylist(playlistName, song);

        }

        private void MenuItemRemoveFromQueue_Click(object sender, RoutedEventArgs e)
        {
            MusicViewModel.GetInstance().FilteredSongs.Remove(MusicViewModel.GetInstance().CurrentlySelectedSong);
        }

        private void MenuItemRemoveGoToDirectory_Click(object sender, RoutedEventArgs e)
        {
            var fullPath = MusicViewModel.GetInstance().CurrentlySelectedSong.Path;
            var directory = Path.GetDirectoryName(fullPath);

            Process.Start("explorer.exe", directory);

        }
    }
}
