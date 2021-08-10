using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Manager
{
    public delegate void OnChatMesssageRecieved(ChatMessage message);

    public class TwitchManager : MonoBehaviour
    {
        public SettingsManager _settingsManager;

        public Client _Client;

        private const string TwitchApiAddress = "https://id.twitch.tv/oauth2";
        private const string AuthorizationEndpoint = "/authorize";
        private const string RedirectUri = "http://127.0.0.1:4471";
        private const string Scopes = "scope=chat:edit chat:read whispers:read whispers:edit bits:read channel:read:subscriptions channel:read:hype_train channel:read:redemptions channel:manage:redemptions";
        private const string TokenEndpoint = "/token";

        public event OnChatMesssageRecieved OnChatMessageRecieved;

        public void ConnectTwitchChat()
        {
            if (_Client != null && _Client.IsConnected)
            {
                return;
            }

            var credentials = new ConnectionCredentials(_settingsManager.CurrentGameSettings.Username,
                _settingsManager.CurrentGameSettings.AccessToken);

            Debug.Log("Initialize Twitch Connection.");
            _Client = new Client();
            _Client.Initialize(credentials, _settingsManager.CurrentGameSettings.Username);

            _Client.OnConnected += _Client_OnConnected;
            _Client.OnJoinedChannel += _Client_OnJoinedChannel;
            _Client.OnMessageReceived += _Client_OnMessageReceived;
            _Client.OnConnectionError += _Client_OnConnectionError;

            _Client.Connect();
        }

        private void _Client_OnConnectionError(object sender, TwitchLib.Client.Events.OnConnectionErrorArgs e)
        {
            Debug.Log(e.Error);
        }

        public void SendMessageToChat(string message)
        {
            if (_Client.IsConnected)
            {
                _Client.SendMessage(_settingsManager.CurrentGameSettings.Username, message);
            }
            else
            {
                Debug.Log("Could not send Message. Client not Connected!");
            }
        }

        private void _Client_OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            Debug.Log($"Joined Channel {_settingsManager.CurrentGameSettings.Username}.");
            //("Hallo Ihrse da!");
        }

        private void _Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            Debug.Log(e.ChatMessage.Username + ": " + e.ChatMessage.Message);
            OnChatMessageRecieved?.Invoke(e.ChatMessage);
        }

        private void _Client_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            Debug.Log("Connected to Twitch Chat Server.");
            Debug.Log("Joining Channel ...");
            _Client.JoinChannel(_settingsManager.CurrentGameSettings.Username);
        }

        public async void LinkTwitchAccount()
        {
            var authObject = await GetAuthToken();
            _settingsManager.CurrentGameSettings.AccessToken = authObject.Access_Token;
            _settingsManager.CurrentGameSettings.RefreshToken = authObject.Refresh_Token;
        }

        public async Task<OAuth> GetAuthToken()
        {
            //Source from: https:github.com/googlesamples/oauth-apps-for-windows/blob/master/OAuthDesktopApp/OAuthDesktopApp/MainWindow.xaml.cs
            //Creates an HttpListener to listen for requests on that redirect URI.

            var httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://127.0.0.1:4471/");

            httpListener.Start();

            //Creates the OAuth 2.0 authorization request.
            var state = RandomDataBase64Url(32);
            var authorizationRequest = "";

            authorizationRequest = $"{TwitchApiAddress}{AuthorizationEndpoint}?client_id={Secrets.cliend_id}&redirect_uri={Uri.EscapeDataString(RedirectUri)}&response_type=code&{Scopes}&force_verify=true&state={state}";

            //Opens request in the browser.
            Process.Start(authorizationRequest);

            //Waits for the OAuth authorization response.

            var context = await httpListener.GetContextAsync();

            //Brings this app back to the foreground.
            //Activate();

            //Sends an HTTP response to the browser.
            const string responseString = "<html><head><meta http-equiv='refresh' content='10;url=https:www.nanotwitchleafs.com/'></head><body>Please return to the app.</body></html>";
            var buffer = Encoding.UTF8.GetBytes(responseString);

            var response = context.Response;
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            await responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith(task =>
            {
                responseOutput.Close();
                httpListener.Stop();
            });

            //Checks for errors.
            if (context.Request.QueryString.Get("error") != null)
            {
                UnityEngine.Debug.Log($"OAuth authorization error: {context.Request.QueryString.Get("error")}.");
                return null;
            }

            if (context.Request.QueryString.Get("code") == null || context.Request.QueryString.Get("state") == null)
            {
                UnityEngine.Debug.Log("Malformed authorization response. " + context.Request.QueryString);
                return null;
            }

            //extracts the code
            string code = context.Request.QueryString.Get("code");
            string incomingState = context.Request.QueryString.Get("state");

            //compares the receieved state to the expected value, to ensure that this app made the request which resulted in authorization.
            if (incomingState != state)
            {
                UnityEngine.Debug.Log($"Received request with invalid state ({incomingState})");
                return null;
            }

            UnityEngine.Debug.Log("Authorization code: " + code);

            return await PerformCodeExchange(code);
        }

        private async Task<OAuth> PerformCodeExchange(string code, bool isRefresh = false)
        {
            UnityEngine.Debug.Log("Exchanging code for tokens...");
            string tokenRequestBody = "";

            if (!isRefresh)
            {
                //builds the  request
                tokenRequestBody = $"code={code}&redirect_uri={Uri.EscapeDataString(RedirectUri)}&client_id={Secrets.cliend_id}&client_secret={Secrets.cliend_id}&grant_type=authorization_code";
            }
            else
            {
                //builds the  request
                tokenRequestBody = $"refresh_token={code}&client_id={Secrets.cliend_id}&client_secret={Secrets.cliend_id}&grant_type=refresh_token";
            }

            //sends the request
            var tokenRequest = (HttpWebRequest)WebRequest.Create($"{TwitchApiAddress}{TokenEndpoint}");
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            tokenRequest.ProtocolVersion = HttpVersion.Version10;

            var byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody);
            tokenRequest.ContentLength = byteVersion.Length;
            var stream = tokenRequest.GetRequestStream();
            await stream.WriteAsync(byteVersion, 0, byteVersion.Length);
            stream.Close();

            try
            {
                //gets the response
                var tokenResponse = await tokenRequest.GetResponseAsync();
                using (var reader = new StreamReader(tokenResponse.GetResponseStream()))
                {
                    //reads response body
                    string responseText = await reader.ReadToEndAsync();
                    UnityEngine.Debug.Log("Response: " + responseText);

                    responseText = responseText.Remove(136) + "}";

                    //converts to dictionary
                    dynamic tokenEndpointDecoded = JsonConvert.DeserializeObject(responseText);

                    return new OAuth { Access_Token = tokenEndpointDecoded["access_token"], Refresh_Token = tokenEndpointDecoded["refresh_token"], Expires_In = tokenEndpointDecoded["expires_in"] };
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Response is HttpWebResponse response)
                    {
                        UnityEngine.Debug.Log("HTTP: " + response.StatusCode);
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            //reads response body
                            string responseText = await reader.ReadToEndAsync();
                            UnityEngine.Debug.Log("Response: " + responseText);
                        }
                    }
                }
            }

            return null;
        }

        public static string RandomDataBase64Url(uint length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);
            return Base64UrlEncodeNoPadding(bytes);
        }

        //    /// <summary>
        //    /// Base64url no-padding encodes the given input buffer.
        //    /// </summary>
        //    /// <param name="buffer"></param>
        //    /// <returns></returns>
        public static string Base64UrlEncodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            //Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            //Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }
    }
}