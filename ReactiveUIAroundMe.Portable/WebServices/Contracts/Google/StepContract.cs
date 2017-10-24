// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StepContract.cs" company="Flush Arcade">
//   Copyright (c) 2015 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Step contract.
	/// </summary>
	public sealed class StepContract : BaseContract
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
		/// Gets or sets the end location.
		/// </summary>
		/// <value>The end location.</value>
		[JsonProperty(PropertyName = "end_location", NullValueHandling = NullValueHandling.Ignore)]
		public LocationContract EndLocation { get; set; }

		/// <summary>
		/// Gets or sets the start address.
		/// </summary>
		/// <value>The start address.</value>
		[JsonProperty(PropertyName = "html_instructions", NullValueHandling = NullValueHandling.Ignore)]
		public string HtmlInstructions { get; set; }

		/// <summary>
		/// Gets or sets the start location.
		/// </summary>
		/// <value>The start location.</value>
		[JsonProperty(PropertyName = "polyline", NullValueHandling = NullValueHandling.Ignore)]
		public PolylineContract Polyline { get; set; }

		/// <summary>
		/// Gets or sets the start location.
		/// </summary>
		/// <value>The start location.</value>
		[JsonProperty(PropertyName = "start_location", NullValueHandling = NullValueHandling.Ignore)]
		public LocationContract StartLocation { get; set; }

		/// <summary>
		/// Gets or sets the travel mode.
		/// </summary>
		/// <value>The travel mode.</value>
		[JsonProperty(PropertyName = "travel_mode", NullValueHandling = NullValueHandling.Ignore)]
		public string TravelMode { get; set; }
	}
}