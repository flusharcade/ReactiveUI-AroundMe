// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthContract.cs" company="Champion Data">
//   Copyright (c) 2015 Champion Data All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Away player contract.
	/// </summary>
	public sealed class AuthContract : BaseContract
	{
		//[JsonProperty(PropertyName = "userConfig", NullValueHandling = NullValueHandling.Ignore)]
		//public UserConfigContract UserConfig { get; set; }

		[JsonProperty(PropertyName = "accessKey", NullValueHandling = NullValueHandling.Ignore)]
		public string AccessKey { get; set; }

		[JsonProperty(PropertyName = "squadId", NullValueHandling = NullValueHandling.Ignore)]
		public int SquadId { get; set; }

		[JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
		public string Email { get; set; }

		[JsonProperty(PropertyName = "group", NullValueHandling = NullValueHandling.Ignore)]
		public string Group { get; set; }

		[JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "userId", NullValueHandling = NullValueHandling.Ignore)]
		public string UserId { get; set; }
	}
}