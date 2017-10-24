// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebServiceControllerBase.cs" company="Champion Data">
//   Copyright (c) 2015 Champion Data All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Reactive.Linq;
    using System.Net.Http.Headers;
    using System.Reactive.Concurrency;
	using System.Linq;

    using Newtonsoft.Json;

    using ReactiveUIAroundMe.Portable.DataAccess;
    using ReactiveUIAroundMe.Portable.Resources;
    using ReactiveUIAroundMe.Portable.Logging;
	using ReactiveUIAroundMe.Portable.Common;
    using ReactiveUIAroundMe.Data;

    /// <summary>
    /// Base web service controller.
    /// </summary>
    public class WebServiceControllerBase
    {
        /// <summary>
        /// The scheduler.
        /// </summary>
        public readonly IScheduler Scheduler;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.WebServices.WebServiceControllerBase"/> class.
        /// </summary>
        /// <param name="storage">Storage.</param>
        /// <param name="log">Log.</param>
        /// <param name="clientHandler">Client handler.</param>
        public WebServiceControllerBase(ISQLiteStorage storage, ILogger log, HttpClientHandler clientHandler, IScheduler scheduler, 
            IUserDefaults userDefaults)
        {
            Log = log;
            Tag = string.Format("{0} ", GetType());

            Storage = storage;
            Scheduler = scheduler;

            ClientHandler = clientHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The storage
        /// </summary>
        protected ISQLiteStorage Storage { get; private set; }

        /// <summary>
        /// The debug.
        /// </summary>
        protected ILogger Log { get; private set; }

        /// <summary>
        /// The tag.
        /// </summary>
        protected string Tag { get; private set; }

        /// <summary>
        /// The web service URL.
        /// </summary>
        protected string BaseUrl { get; set; }

        /// <summary>
        /// The client handler.
        /// </summary>
        protected HttpClientHandler ClientHandler { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sends the async.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task<T> Send<T>(string apiUrl, string body, HttpMethod method, IWebServiceErrorHandler errorHandler) where T : BaseContract
        {
            try
            {
                var resultContract = await SendInternal<T>(apiUrl, body, method);
                /*if (resultContract.ErrorCode == McmErrorCode.ExpiredToken) {
					if (errorHandler.RefreshAndRetryOnExpiration) {
						await RefreshAppAuthToken ();
						resultContract = await SendInternal<T> (apiUrl, body);
					}
				}
				if (resultContract.ErrorCode == McmErrorCode.InvalidAppAuthToken || resultContract.ErrorCode == McmErrorCode.InvalidAuthToken) {
					throw new AuthorizationExpiredException ();
				}*/
                return resultContract;

            }
            catch (Exception error)
            {
                if (!errorHandler.HandleException(error))
                {
                    Log.WriteLineTime(Tag + "\n" +
                        "Send() failed to retrieve contracts." + "\n" +
                        "ErrorMessage: \n" +
                        error.Message + "\n" +
                        "Stacktrace: \n " +
                        error.StackTrace);
                }
            }

            return default(T);
        }


        /// <summary>
        /// Gets the identity async.
        /// </summary>
        /// <returns>The identity async.</returns>
        protected async Task<string> RefresWSO2Token()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0:HH:mm:ss} REFRESH WSO2 TOKEN", DateTime.Now));

            var identity = await Storage.GetObject<IdentityStorable>(StorableKeys.Identity.ToString());
            // this is our first time installing and running the app
            identity = identity ?? new IdentityStorable();

            var httpMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(Config.TokenEndPointUrl));
            httpMessage.Headers.Add("Authorization", "Basic " + Base64Encode(string.Format("{0}:{1}", Config.ConsumerKeyProduction, Config.ConsumerSecretProduction)));
            httpMessage.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var auth = await SendInternal<TokenContract>(httpMessage);
            if (auth != null)
            {
                identity.WSO2Token = auth.AccessToken;
                await Storage.InsertObject(identity);
                return auth.AccessToken;
            }

            // Unable to refresh token and/or find site, log user out
            throw new AuthorizationExpiredException();
        }

        /// <summary>
        /// Refreses the access token.
        /// </summary>
        /// <returns>The access token.</returns>
        protected async Task<string> RefresAccessToken()
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0:HH:mm:ss} REFRESH ACCESS TOKEN", DateTime.Now));

            var identity = await Storage.GetObject<IdentityStorable>(StorableKeys.Identity.ToString());
            // this is our first time installing and running the app
            identity = identity ?? new IdentityStorable();

            // todo: on invalidation of access token, we want an alert to appear for the user to retype password
            //var hash = MD5.GetHash(identity.Password);
            var password = identity.Password;
            var authObj = new
            {
                Email = identity.Username,
                Password = password,
                DeviceId = "a22aaf4c9ff8a673be5bade4b8622fa7"
            };

            var body = JsonConvert.SerializeObject(authObj);
            var auth = await SendInternal<AuthContract>(Config.AuthDeviceUrl, body, HttpMethod.Post, true);
            if (auth != null)
            {
                identity.AccessToken = auth.AccessKey;
                await Storage.InsertObject(identity);
                return auth.AccessKey;
            }

            // Unable to refresh token and/or find site, log user out
            throw new AuthorizationExpiredException();
        }

        /// <summary>
        /// Base64s the encode.
        /// </summary>
        /// <returns>The encode.</returns>
        /// <param name="plainText">Plain text.</param>
        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Sends the internal.
        /// </summary>
        /// <returns>The internal.</returns>
        /// <param name="url">API URL.</param>
        /// <param name="body">Body.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected IObservable<T> SendInternal<T>(string url, string body, HttpMethod method, bool refreshToken = false, CancellationToken? cancellationToken = null) where T : class
        {
            return Observable.FromAsync(() => Storage.GetObject<IdentityStorable>(StorableKeys.Identity.ToString()))
                 .SelectMany((identity) =>
                 {
                     identity = identity ?? new IdentityStorable();

                     // TODO: refresh token and WSO2 addition
                     /*if (string.IsNullOrEmpty(identity.WSO2Token) || refreshToken)
                     {
                         identity.WSO2Token = await RefresWSO2Token();
                     }*/

                     // don't use base url
                     var uri = refreshToken ? new Uri(url) : new Uri(string.Format("{0}{1}", BaseUrl, url));
                     var httpMessage = new HttpRequestMessage(method, uri);

                     /*if (!string.IsNullOrEmpty(identity.WSO2Token))
                     {
                         httpMessage.Headers.Add("Authorization", string.Format("Bearer {0}", identity.WSO2Token ?? string.Empty));
                     }

                     if (!string.IsNullOrEmpty(identity.AccessToken))
                     {
                         httpMessage.Headers.Add("x-UserToken", identity.AccessToken ?? string.Empty);
                     }*/

                     if (!string.IsNullOrEmpty(body))
                     {
						httpMessage.Content = new StringContent(body, Encoding.UTF8, "application/json");
                     }

                     return SendInternal<T>(httpMessage, cancellationToken);
                 }).Select(x => x);
        }

        /// <summary>
        /// Sends the internal.
        /// </summary>
        /// <returns>The internal.</returns>
        /// <param name="httpMessage">Http message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected IObservable<T> SendInternal<T>(HttpRequestMessage httpMessage, CancellationToken? cancellationToken = null)
            where T : class
        {
            var client = new HttpClient(ClientHandler);

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return Observable.FromAsync(() => client.SendAsync(httpMessage, cancellationToken ?? new CancellationToken(false)))
                    .SelectMany(async response =>
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.NotFound:
                                throw new ConnectionException("Host Not Found");
                            case HttpStatusCode.Unauthorized:
                                // token has expired, refresh token and recall send
                                /*var wso2Token = await RefresWSO2Token();
                                var accessToken = await RefresAccessToken();

                                // we have to create a new http message, we can't resend or copy the same message
                                var newHttpMessage = new HttpRequestMessage(httpMessage.Method, httpMessage.RequestUri);
                                if (!string.IsNullOrEmpty(wso2Token) && !string.IsNullOrEmpty(accessToken))
                                {
                                    httpMessage.Headers.Add("Authorization", string.Format("Bearer {0}", wso2Token ?? string.Empty));
                                    httpMessage.Headers.Add("x-UserToken", accessToken ?? string.Empty);
                                }

                                // recall on client to return json after adding new token
                                return await (await client.SendAsync(newHttpMessage, cancellationToken ?? new CancellationToken(false)))?.Content.ReadAsStringAsync();*/
								throw new ConnectionException("Unauthorized");
                        }

                        return await response.Content.ReadAsStringAsync();
                    })
                    .Select(response =>
                    {
                        if (typeof(T) == typeof(string))
                            return (T)(object)response;

                        if (!string.IsNullOrWhiteSpace(response))
                        {
                            return JsonConvert.DeserializeObject<T>(response) as T;
                        }

                        return default(T);
                    });
        }
    }

    #endregion
}