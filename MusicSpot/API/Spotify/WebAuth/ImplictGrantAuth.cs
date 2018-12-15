using MusicSpot.API.Spotify.Web.Enums;
using MusicSpot.API.Spotify.Web.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

#if NETSTANDARD2_0
using System.Net.Http;
#endif
#if NET46
using System.Net.Http;
using HttpListenerContext = Unosquare.Net.HttpListenerContext;
#endif

namespace MusicSpot.API.Spotify.WebAuth
{
    public class ImplictGrantAuth : SpotifyAuthServer<Token>
    {
        public ImplictGrantAuth(string clientId, string redirectUri, string serverUri, Scope scope = Scope.None, string state = "") :
            base("token", "ImplicitGrantAuth", redirectUri, serverUri, scope, state)
        {
            ClientId = clientId;
        }

        protected override WebServer AdaptWebServer(WebServer webServer)
        {
            Console.WriteLine("Hello");
            webServer.Module<WebApiModule>().RegisterController<ImplictGrantAuthController>();
            return webServer;
        }
    }

    public class ImplictGrantAuthController : WebApiController
    {
        [WebApiHandler(HttpVerbs.Get, "/auth")]
        public Task<bool> GetAuth(WebServer server, HttpListenerContext context)
        {
            string state = context.Request.QueryString["state"];
            SpotifyAuthServer<Token> auth = ImplictGrantAuth.GetByState(state);
            if (auth == null)
                return context.StringResponseAsync(
                    $"Failed - Unable to find auth request with state \"{state}\" - Please retry");

            Token token;
            string error = context.Request.QueryString["error"];
            if (error == null)
            {
                string accessToken = context.Request.QueryString["access_token"];
                string tokenType = context.Request.QueryString["token_type"];
                string expiresIn = context.Request.QueryString["expires_in"];
                token = new Token
                {
                    AccessToken = accessToken,
                    ExpiresIn = double.Parse(expiresIn),
                    TokenType = tokenType
                };
            }
            else
            {
                token = new Token()
                {
                    Error = error
                };
            }

            Task.Factory.StartNew(() => auth.TriggerAuth(token));
            return context.StringResponseAsync("OK - This window can be closed now");
        }
    }
}
