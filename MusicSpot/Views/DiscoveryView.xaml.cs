using System;
using System.Windows;
using System.Windows.Controls;
using DiscoveryViewModel = MusicSpot.ViewModels.DiscoveryViewModel;

namespace MusicSpot.Views
{
    /// <summary>
    /// Interaction logic for Video.xaml
    /// </summary>
    public partial class DiscoveryView : UserControl
    {
        private DiscoveryViewModel dataModel;

        public DiscoveryView()
        {
            InitializeComponent();
            dataModel = DiscoveryViewModel.GetInstance();
            this.DataContext = dataModel;
        }

        private void MusicProgressBar_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Math.Abs(e.OldValue - e.NewValue) > 2)
                MusicPlayer.MusicPlayer.audioFileReader.CurrentTime = new TimeSpan(0, 0, 0, (int)e.NewValue);
        }
    }
}
