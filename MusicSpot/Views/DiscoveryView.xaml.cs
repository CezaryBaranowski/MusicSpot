using MusicSpot.API.Spotify.Web.Models;
using System.Linq;
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Otwieram Youtube");
            Button btn = (Button)e.Source;
            var selectedTrack = (FullTrack)btn.DataContext;
            if (selectedTrack != null)
            {
                var adress = "https://www.youtube.com/results?search_query=" + selectedTrack.Artists.FirstOrDefault().Name + " " +
                         selectedTrack.Name;
                System.Diagnostics.Process.Start(adress);
            }
            else
            {
                MessageBox.Show("Cannot find resource");
            }
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Otwieram Spotify");
            Button btn = (Button)e.Source;
            var selectedTrack = (FullTrack)btn.DataContext;

            var adress = selectedTrack.ExternUrls["spotify"];
            System.Diagnostics.Process.Start(adress);
        }
    }
}
