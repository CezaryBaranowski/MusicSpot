using System.Threading.Tasks;
using MusicSpot.API.Spotify.Web.Models;
using MusicSpot.API.Spotify.WebAuth;

namespace MusicSpot.API.Spotify
{
    public static class SpotifyApiInitializer
    {   
        private static string _clientId = "571a40e7370e4d5cbedcd33f1119673e";
        private static string _secretId = "0b6005ddd8c34a7e9065728f7eeaede0";
        private static CredentialsAuth auth;

        public static async Task<Token> GetSpotifyApiToken()
        {
            auth = new CredentialsAuth(_clientId, _secretId);
            var token = await auth.GetToken();
            return token;
        }
    }
}
