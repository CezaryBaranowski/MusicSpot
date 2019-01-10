using MusicSpot.API.Spotify.Web.Models;
using MusicSpot.Models;
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
            Button btn = (Button)e.Source;
            var selectedTrack = (FullTrack)btn.DataContext;

            var adress = selectedTrack.ExternUrls["spotify"];
            System.Diagnostics.Process.Start(adress);
        }

        private void SimilarArtistsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)e.Source;
            var selectedArtist = (FullArtistWithImage)btn.DataContext;

            var adress = selectedArtist.FullArtist.ExternalUrls["spotify"];
            System.Diagnostics.Process.Start(adress);

        }

        private void WikiButton_OnClick(object sender, RoutedEventArgs e)
        {
            string baseAdress = "https://en.wikipedia.org/wiki/";
            string artistName = DiscoveryViewModel.GetInstance().CurrentlyPlayedSong.Artist;
            string url = string.Concat(baseAdress, artistName);
            System.Diagnostics.Process.Start(url);
        }
    }
}
