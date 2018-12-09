using Microsoft.WindowsAPICodePack.Dialogs;
using MusicSpot.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MusicSpot.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void RemoveMusicDirectories(object sender, RoutedEventArgs e)
        {
            var selectedDirectory = MusicDirectoriesListBox.SelectedItem;
            var viewModel = SettingsViewModel.GetInstance();
            viewModel.RemoveMusicDirectory(selectedDirectory as string);
        }

        private void AddMusicDirectory(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "E:\\Muzyka\\Muzyka";
            dialog.IsFolderPicker = true;
            string directory = null;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                directory = dialog.FileName;
            }

            var viewModel = SettingsViewModel.GetInstance();
            if (directory != null) viewModel.AddMusicDirectory(directory);
        }

    }
}
