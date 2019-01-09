using MusicSpot.Annotations;
using MusicSpot.API.Spotify.Web;
using MusicSpot.API.Spotify.Web.Enums;
using MusicSpot.API.Spotify.Web.Models;
using MusicSpot.API.Tekstowo;
using MusicSpot.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Image = System.Drawing.Image;

namespace MusicSpot.ViewModels
{
    public sealed class DiscoveryViewModel : ViewModelBase
    {

        private SpotifyWebAPI _api;

        private static readonly DiscoveryViewModel discoveryViewModel = new DiscoveryViewModel();

        static DiscoveryViewModel()
        {
        }

        private DiscoveryViewModel()
        {
            InitDiscoveryViewModel();
        }

        public static DiscoveryViewModel GetInstance()
        {
            return discoveryViewModel;
        }

        private void InitDiscoveryViewModel()
        {
            MostPopularArtistTracks = new ObservableCollection<FullTrack>();
            RelatedArtists = new ObservableCollection<FullArtist>();
            ArtistTopTracks = new ObservableCollection<FullTrack>();
        }

        public bool IsSpotifyApiInitialized()
        {
            return _api != null;
        }

        public async void InitSpotifyAPI()
        {
            var token = await API.Spotify.SpotifyApiInitializer.GetSpotifyApiToken();
            if (token != null)
                _api = new SpotifyWebAPI
                {
                    AccessToken = token.AccessToken,
                    TokenType = token.TokenType
                };

            //var something = await _api.GetArtistsTopTracksAsync("6HCnsY0Rxi3cg53xreoAIm", "pl");

            // MostPopularArtistTracks = new ObservableCollection<FullTrack>(something.Tracks);
        }

        public async void LoadNewArtistDetails(string artistName)
        {
            SearchItem artistSearchResults = await _api.SearchItemsAsync(artistName, SearchType.Artist, 1, 0);
            var artist = artistSearchResults.Artists.Items.FirstOrDefault();
            if (artist != null)
            {
                await LoadArtistImageFromApi(artist.Images[0].Url);

                SeveralTracks artistsTopTracks = await _api.GetArtistsTopTracksAsync(artist.Id, "us");
                ArtistTopTracks = new ObservableCollection<FullTrack>(artistsTopTracks.Tracks);

                await LoadSongLyrics(MusicViewModel.GetInstance().CurrentlyPlayedSong.Title, artist.Name);
            }

        }

        public async Task LoadSongLyrics(string songName, string artistName)
        {
            SongLyrics = await TekstowoCrawler.GetSongLyrics(songName, artistName);
        }

        private async Task LoadArtistImageFromApi(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                var data = await webClient.DownloadDataTaskAsync(url);

                using (MemoryStream mem = new MemoryStream(data))
                {
                    ArtistImage = Image.FromStream(mem);
                }
            }
        }

        private Song _currentlyPlayedSong;
        public Song CurrentlyPlayedSong
        {
            get => _currentlyPlayedSong;
            set
            {
                _currentlyPlayedSong = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FullTrack> _mostPopularArtistTracks;
        public ObservableCollection<FullTrack> MostPopularArtistTracks
        {
            get => _mostPopularArtistTracks;
            set
            {
                _mostPopularArtistTracks = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FullArtist> _relatedArtists;
        public ObservableCollection<FullArtist> RelatedArtists
        {
            get => _relatedArtists;
            set
            {
                _relatedArtists = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FullTrack> _artistTopTracks;
        public ObservableCollection<FullTrack> ArtistTopTracks
        {
            get => _artistTopTracks;
            set
            {
                _artistTopTracks = value;
                OnPropertyChanged();
            }
        }

        private Image _artistImage;
        [CanBeNull]
        public Image ArtistImage
        {
            get => _artistImage;
            set
            {
                _artistImage = value;
                OnPropertyChanged();
            }
        }

        private String _songLyrics;
        public String SongLyrics
        {
            get => _songLyrics;
            set
            {
                _songLyrics = value;
                OnPropertyChanged();
            }
        }

        private String _artistBio;
        public String ArtistBio
        {
            get => _artistBio;
            set
            {
                _artistBio = value;
                OnPropertyChanged();
            }
        }

    }
}
