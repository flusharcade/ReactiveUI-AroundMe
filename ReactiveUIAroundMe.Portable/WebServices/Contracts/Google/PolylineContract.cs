// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolylineContract.cs" company="Flush Arcade">
//   Copyright (c) 2015 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Polyline contract.
	/// </summary>
	public sealed class PolylineContract : BaseContract
	{
		/// <summary>
		/// Gets or sets the points.
		/// </summary>
		/// <value>The points.</value>
		[JsonProperty(PropertyName = "points", NullValueHandling = NullValueHandling.Ignore)]
		public string Points { get; set; }
	}
}