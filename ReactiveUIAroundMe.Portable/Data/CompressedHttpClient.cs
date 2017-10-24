namespace ReactiveUIAroundMe.Data
{
    using System.Net;
    using System.Net.Http;

    /// <summary>
    /// A http client that supports compressed http transfers.
    /// </summary>
    /// <remarks>Code adapted from <c>http://pastebin.com/N1jV30bM</c> by 
    /// </remarks>
    public class CompressedHttpClient : HttpClient
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressedHttpClient"/> class.
        /// </summary>
        public CompressedHttpClient()
            : base(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip })
        {
        }

        #endregion
    }
}