﻿using MahApps.Metro.Controls;
using MusicSpot.ViewModels;
using System.Windows;

namespace MusicSpot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainViewModel.GetInstance();
        }

        private void LaunchMenu(object sender, RoutedEventArgs e)
        {
            MainViewModel.GetInstance().IsSettingFlyoutOpen = !MainViewModel.GetInstance().IsSettingFlyoutOpen;
        }
    }
}