// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoundsContract.cs" company="Flush Arcade">
//   Copyright (c) 2015 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Bounds contract.
	/// </summary>
	public sealed class BoundsContract : BaseContract
	{
		/// <summary>
		/// Gets or sets the north east.
		/// </summary>
		/// <value>The north east.</value>
		[JsonProperty(PropertyName = "northeast", NullValueHandling = NullValueHandling.Ignore)]
		public LocationContract NorthEast { get; set; }

		/// <summary>
		/// Gets or sets the south west.
		/// </summary>
		/// <value>The south west.</value>
		[JsonProperty(PropertyName = "southwest", NullValueHandling = NullValueHandling.Ignore)]
		public LocationContract SouthWest { get; set; }
	}
}