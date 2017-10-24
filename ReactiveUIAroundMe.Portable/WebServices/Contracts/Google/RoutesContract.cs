// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleGeocodeContract.cs" company="Flush Arcade">
//   Copyright (c) 2015 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Routes contract.
	/// </summary>
	public sealed class RoutesContract : BaseContract
	{
		/// <summary>
		/// Gets or sets the bounds.
		/// </summary>
		/// <value>The bounds.</value>
		[JsonProperty(PropertyName = "bounds", NullValueHandling = NullValueHandling.Ignore)]
		public BoundsContract Bounds { get; set; }

		/// <summary>
		/// Gets or sets the copyrights.
		/// </summary>
		/// <value>The copyrights.</value>
		[JsonProperty(PropertyName = "copyrights", NullValueHandling = NullValueHandling.Ignore)]
		public string Copyrights { get; set; }

		/// <summary>
		/// Gets or sets the legs.
		/// </summary>
		/// <value>The legs.</value>
		[JsonProperty(PropertyName = "legs", NullValueHandling = NullValueHandling.Ignore)]
		public LegContract[] Legs { get; set; }

		/// <summary>
		/// Gets or sets the overview polyline.
		/// </summary>
		/// <value>The overview polyline.</value>
		[JsonProperty(PropertyName = "overview_polyline", NullValueHandling = NullValueHandling.Ignore)]
		public PolylineContract OverviewPolyline { get; set; }

		/// <summary>
		/// Gets or sets the summary.
		/// </summary>
		/// <value>The summary.</value>
		[JsonProperty(PropertyName = "summary", NullValueHandling = NullValueHandling.Ignore)]
		public string Summary { get; set; }
	}
}