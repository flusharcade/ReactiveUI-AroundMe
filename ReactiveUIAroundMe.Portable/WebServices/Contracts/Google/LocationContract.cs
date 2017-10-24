// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocationContract.cs" company="Flush Arcade">
//   Copyright (c) 2015 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Location contract.
	/// </summary>
	public sealed class LocationContract : BaseContract
	{
		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		/// <value>The latitude.</value>
		[JsonProperty(PropertyName = "lat", NullValueHandling = NullValueHandling.Ignore)]
		public decimal Latitude { get; set; }

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		/// <value>The longitude.</value>
		[JsonProperty(PropertyName = "lng", NullValueHandling = NullValueHandling.Ignore)]
		public decimal Longitude { get; set; }
	}
}