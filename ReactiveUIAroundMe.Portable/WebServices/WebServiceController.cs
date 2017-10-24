// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebServiceController.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.WebServices
{
	using System;
	using System.Net.Http;
	using System.Reactive.Concurrency;
	using System.Collections.Generic;

	using Common;

	using Newtonsoft.Json;

	using ReactiveUIAroundMe.Portable.Resources;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Logging;
	using ReactiveUIAroundMe.Portable.Location;

	/// <summary>
	/// Web service controller.
	/// </summary>
	public class WebServiceController : WebServiceControllerBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Medibio.Portable.WebServices.AuthWebServiceController"/> class.
		/// </summary>
		/// <param name="clientHandler">Client handler.</param>
		public WebServiceController(ISQLiteStorage storage, ILogger log, HttpClientHandler clientHandler, 
            IScheduler scheduler, IUserDefaults userDefaults) : base(storage, log, clientHandler, scheduler, userDefaults)
		{
			BaseUrl = userDefaults.Host;
		}

		#region GET

		#endregion

		#region POST

		/// <summary>
		/// Authorization this instance.
		/// </summary>
		public IObservable<AuthContract> AuthorizeDevice(string email, string password, string deviceId)
		{
			var auth = new {
				Email = email,
				Password = password,
				DeviceId = "a22aaf4c9ff8a673be5bade4b8622fa7"
			};

			var body = JsonConvert.SerializeObject(auth);
			return SendInternal<AuthContract>(Config.AuthDeviceUrl, body, HttpMethod.Post);
		}

		/// <summary>
		/// Searchs the outlets by location.
		/// </summary>
		/// <returns>The outlets by location.</returns>
		/// <param name="location">Location.</param>
		public IObservable<IEnumerable<EReactiveUIAroundMeListItemContract>> GetEReactiveUIAroundMesByLocation(Location location)
		{
			var geolocation = new
			{
				Latitude = location.Latitude,
				Longitude = location.Longitude,
				Range = 500
			};

			var body = JsonConvert.SerializeObject(geolocation);
			return SendInternal<IEnumerable<EReactiveUIAroundMeListItemContract>>(Config.GetEReactiveUIAroundMesByLocationUrl, body, HttpMethod.Post);
		}

		/// <summary>
		/// Gets the eReactiveUIAroundMe details.
		/// </summary>
		/// <returns>The eReactiveUIAroundMe details.</returns>
		public IObservable<object> GetEReactiveUIAroundMeById(string id)
		{
			var eReactiveUIAroundMeDetails = new
			{
				Id = id
			};

			var body = JsonConvert.SerializeObject(eReactiveUIAroundMeDetails);
			return SendInternal<object>(Config.GetEReactiveUIAroundMeByIdUrl, body, HttpMethod.Post);
		}

		#endregion
	}
}