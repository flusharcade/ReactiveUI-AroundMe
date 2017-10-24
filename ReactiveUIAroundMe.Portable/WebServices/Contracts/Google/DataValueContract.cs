// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataValueContract.cs" company="Flush Arcade">
//   Copyright (c) 2015 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Data value contract.
	/// </summary>
	public sealed class DataValueContract : BaseContract
	{
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		[JsonProperty(PropertyName = "text", NullValueHandling = NullValueHandling.Ignore)]
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		[JsonProperty(PropertyName = "value", NullValueHandling = NullValueHandling.Ignore)]
		public decimal Value { get; set; }
	}
}