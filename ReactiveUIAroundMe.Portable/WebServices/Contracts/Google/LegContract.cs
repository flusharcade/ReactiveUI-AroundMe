// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LegContract.cs" company="Flush Arcade">
//   Copyright (c) 2015 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Leg contract.
	/// </summary>
	public sealed class LegContract : BaseContract
	{
		/// <summary>
		/// Gets or sets the distance.
		/// </summary>
		/// <value>The distance.</value>
		[JsonProperty(PropertyName = "distance", NullValueHandling = NullValueHandling.Ignore)]
		public DataValueContract Distance { get; set; }

		/// <summary>
		/// Gets or sets the duration.
		/// </summary>
		/// <value>The duration.</value>
		[JsonProperty(PropertyName = "duration", NullValueHandling = NullValueHandling.Ignore)]
		public DataValueContract Duration { get; set; }

		/// <summary>
		/// Gets or sets the end address.
		/// </summary>
		/// <value>The end address.</value>
		[JsonProperty(PropertyName = "end_address", NullValueHandling = NullValueHandling.Ignore)]
		public string EndAddress { get; set; }

		/// <summary>
		/// Gets or sets the end location.
		/// </summary>
		/// <value>The end location.</value>
		[JsonProperty(PropertyName = "end_location", NullValueHandling = NullValueHandling.Ignore)]
		public LocationContract EndLocation { get; set; }

		/// <summary>
		/// Gets or sets the start address.
		/// </summary>
		/// <value>The start address.</value>
		[JsonProperty(PropertyName = "start_address", NullValueHandling = NullValueHandling.Ignore)]
		public string StartAddress { get; set; }

		/// <summary>
		/// Gets or sets the start location.
		/// </summary>
		/// <value>The start location.</value>
		[JsonProperty(PropertyName = "start_location", NullValueHandling = NullValueHandling.Ignore)]
		public LocationContract StartLocation { get; set; }
	
		/// <summary>
		/// Gets or sets the steps.
		/// </summary>
		/// <value>The steps.</value>
		[JsonProperty(PropertyName = "steps", NullValueHandling = NullValueHandling.Ignore)]
		public StepContract[] Steps { get; set; }
	}
}