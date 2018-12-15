using MusicSpot.API.Spotify.Web.Enums;
using MusicSpot.API.Spotify.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

#if NET46
using HttpListenerContext = Unosquare.Net.HttpListenerContext;
#endif

namespace MusicSpot.API.Spotify.WebAuth
{
    public class AuthorizationCodeAuth : SpotifyAuthServer<AuthorizationCode>
    {
        public string SecretId { get; set; }

        public AuthorizationCodeAuth(string redirectUri, string serverUri, Scope scope = Scope.None, string state = "")
            : base("code", "AuthorizationCodeAuth", redirectUri, serverUri, scope, state)
        {
        }

        public AuthorizationCodeAuth(string clientId, string secretId, string redirectUri, string serverUri, Scope scope = Scope.None, string state = "")
            : this(redirectUri, serverUri, scope, state)
        {
            ClientId = clientId;
            SecretId = secretId;
        }

        private bool ShouldRegisterNewApp()
        {
            return string.IsNullOrEmpty(SecretId) || string.IsNullOrEmpty(ClientId);
        }

        public override string GetUri()
        {
            return ShouldRegisterNewApp() ? $"{RedirectUri}/start.html#{State}" : base.GetUri();
        }

        protected override WebServer AdaptWebServer(WebServer webServer)
        {
            return webServer.WithWebApiController<AuthorizationCodeAuthController>();
        }

        private string GetAuthHeader() => $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientId + ":" + SecretId))}";

        public async Task<Token> RefreshToken(string refreshToken)
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            };

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", GetAuthHeader());
            HttpContent content = new FormUrlEncodedContent(args);

            HttpResponseMessage resp = await client.PostAsync("https://accounts.spotify.com/api/token", content);
            string msg = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Token>(msg);
        }
        public async Task<Token> ExchangeCode(string code)
        {
            List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", RedirectUri)
            };

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", GetAuthHeader());
            HttpContent content = new FormUrlEncodedContent(args);

            HttpResponseMessage resp = await client.PostAsync("https://accounts.spotify.com/api/token", content);
            string msg = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Token>(msg);
        }
    }

    public class AuthorizationCode
    {
        public string Code { get; set; }

        public string Error { get; set; }
    }

    internal class AuthorizationCodeAuthController : WebApiController
    {
        [WebApiHandler(HttpVerbs.Get, "/")]
        public Task<bool> GetEmpty(WebServer server, HttpListenerContext context)
        {
            string state = context.Request.QueryString["state"];
            AuthorizationCodeAuth.Instances.TryGetValue(state, out SpotifyAuthServer<AuthorizationCode> auth);

            string code = null;
            string error = context.Request.QueryString["error"];
            if (error == null)
                code = context.Request.QueryString["code"];

            Task.Factory.StartNew(() => auth?.TriggerAuth(new AuthorizationCode
            {
                Code = code,
                Error = error
            }));

            return context.StringResponseAsync("OK - This window can be closed now");
        }

        [WebApiHandler(HttpVerbs.Post, "/")]
        public bool PostValues(WebServer server, HttpListenerContext context)
        {
            Dictionary<string, object> formParams = context.RequestFormDataDictionary();

            string state = (string)formParams["state"];
            AuthorizationCodeAuth.Instances.TryGetValue(state, out SpotifyAuthServer<AuthorizationCode> authServer);

            AuthorizationCodeAuth auth = (AuthorizationCodeAuth)authServer;
            auth.ClientId = (string)formParams["clientId"];
            auth.SecretId = (string)formParams["secretId"];

            string uri = auth.GetUri();
            return context.Redirect(uri, false);
        }
    }
}
