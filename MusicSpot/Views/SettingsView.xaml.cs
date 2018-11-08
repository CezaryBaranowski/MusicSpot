﻿using Microsoft.WindowsAPICodePack.Dialogs;
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

        private void RemoveVideoDirectories(object sender, RoutedEventArgs e)
        {
            var selectedDirectory = VideoDirectoriesListBox.SelectedItem;
            var viewModel = SettingsViewModel.GetInstance();
            viewModel.RemoveVideoDirectory(selectedDirectory as string);
        }

        private void RemovePictureDirectories(object sender, RoutedEventArgs e)
        {
            var selectedDirectory = PictureDirectoriesListBox.SelectedItem;
            var viewModel = SettingsViewModel.GetInstance();
            viewModel.RemovePictureDirectory(selectedDirectory as string);
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

        private void AddVideoDirectory(object sender, RoutedEventArgs e)
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
            if (directory != null) viewModel.AddVideoDirectory(directory);
        }

        private void AddPictureDirecotry(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            string directory = null;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                directory = dialog.FileName;
            }

            var viewModel = SettingsViewModel.GetInstance();
            if (directory != null) viewModel.AddPictureDirectory(directory);
        }
    }
}
