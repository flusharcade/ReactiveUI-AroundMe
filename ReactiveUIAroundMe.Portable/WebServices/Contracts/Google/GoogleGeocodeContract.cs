// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleGeocodeContract.cs" company="Flush Arcade">
//   Copyright (c) 2015 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Google geocode contract.
	/// </summary>
	public sealed class GoogleGeocodeContract : BaseContract
	{
		/// <summary>
		/// Gets or sets the routes.
		/// </summary>
		/// <value>The routes.</value>
		[JsonProperty(PropertyName = "routes", NullValueHandling = NullValueHandling.Ignore)]
		public RoutesContract[] Routes { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		[JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
		public string Status { get; set; }
	}
}