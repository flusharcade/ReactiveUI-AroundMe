// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoWebServiceController.cs" company="Flush Arcade Pty Ltd.">
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
	using ReactiveUIAroundMe.Portable.Models;
	using ReactiveUIAroundMe.Portable.Enums;

	/// <summary>
	/// Google maps web service controller.
	/// </summary>
	public class GoogleMapsWebServiceController : WebServiceControllerBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Medibio.Portable.WebServices.AuthGeoWebServiceController"/> class.
		/// </summary>
		/// <param name="clientHandler">Client handler.</param>
		public GoogleMapsWebServiceController(ISQLiteStorage storage, ILogger log, HttpClientHandler clientHandler, 
            IScheduler scheduler, IUserDefaults userDefaults) : base(storage, log, clientHandler, scheduler, userDefaults)
		{
			BaseUrl = Config.GoogleMapsApiUrl;
		}

		#region GET

		#endregion

		#region POST

		/// <summary>
		/// Gets the directions poly line.
		/// </summary>
		/// <returns>The directions poly line.</returns>
		/// <param name="startCoordinate">Start coordinate.</param>
		/// <param name="destinationCoordinate">Destination coordinate.</param>
		/// <param name="mode">Mode.</param>
		public IObservable<GoogleGeocodeContract> GetDirectionsPolyLine(GeoCoordinate startCoordinate, 
		                                                 GeoCoordinate destinationCoordinate,
		                                                 GoogleMapsTravelModes mode)
		{
			var startLatString = startCoordinate.Latitude.ToString().Replace(',', '.');
			var startLongString = startCoordinate.Longitude.ToString().Replace(',', '.');
			var destLatString = destinationCoordinate.Latitude.ToString().Replace(',', '.');
			var destLongString = destinationCoordinate.Longitude.ToString().Replace(',', '.');
			var startString = startLatString + "," + startLongString;
			var destinationString = destLatString + "," + destLongString;

			string param = string.Format(Config.DirectionParamsUrl, startString, destinationString, Config.GoogleApiKey);
			param += string.Format("&mode={0}", mode.ToString().ToLower());

			var directionsUrl = string.Format("/{0}", Config.DirectionsUrl + param);

			return SendInternal<GoogleGeocodeContract>(directionsUrl, string.Empty, HttpMethod.Get);
		}

		#endregion
	}
}