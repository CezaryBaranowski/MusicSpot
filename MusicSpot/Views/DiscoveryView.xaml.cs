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
    }
}
