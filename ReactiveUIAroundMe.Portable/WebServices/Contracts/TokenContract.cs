namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Token contract.
	/// </summary>
	public sealed class TokenContract : BaseContract
	{
		/// <summary>
		/// Gets or sets the access token.
		/// </summary>
		/// <value>The access token.</value>
		[JsonProperty(PropertyName = "access_token", NullValueHandling = NullValueHandling.Ignore)]
		public string AccessToken { get; set; }

		/// <summary>
		/// Gets or sets the scope.
		/// </summary>
		/// <value>The scope.</value>
		[JsonProperty(PropertyName = "scope", NullValueHandling = NullValueHandling.Ignore)]
		public string Scope { get; set; }

		/// <summary>
		/// Gets or sets the type of the token.
		/// </summary>
		/// <value>The type of the token.</value>
		[JsonProperty(PropertyName = "token_type", NullValueHandling = NullValueHandling.Ignore)]
		public string TokenType { get; set; }

		/// <summary>
		/// Gets or sets the expires in.
		/// </summary>
		/// <value>The expires in.</value>
		[JsonProperty(PropertyName = "expires_in", NullValueHandling = NullValueHandling.Ignore)]
		public int ExpiresIn { get; set; }
	}
}