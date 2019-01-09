using MusicSpot.API.Spotify.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicSpot.API.Spotify.WebAuth
{
    public class CredentialsAuth
    {
        public string ClientSecret { get; set; }

        public string ClientId { get; set; }

        public CredentialsAuth(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public async Task<Token> GetToken()
        {
            try
            {
                string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientId + ":" + ClientSecret));

                List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                };

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");
                HttpContent content = new FormUrlEncodedContent(args);

                HttpResponseMessage resp = client.PostAsync("https://accounts.spotify.com/api/token", content).Result;
                string msg = await resp.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Token>(msg);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
