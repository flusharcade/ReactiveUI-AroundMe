// --------------------------------------------------------------------------------------------------
//  <copyright file="ILocationManager.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Location
{
	using System;
	using System.Reactive.Subjects;
	using System.Threading.Tasks;

	/// <summary>
	/// The location managaer interface.
	/// </summary>
	public interface ILocationManager
	{
		/// <summary>
		/// Gets or sets the updates.
		/// </summary>
		/// <value>The updates.</value>
		IDisposable Updates { get; set; }

		/// <summary>
		/// The location updates.
		/// </summary>
		Subject<LocationUpdatedEventArgs> LocationUpdates { get; set; }

        /// <summary>
        /// Gets or sets the address updates.
        /// </summary>
        /// <value>The address updates.</value>
        Subject<AddressUpdatedEventArgs> AddressUpdates { get; set; }

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start();

		/// <summary>
		/// Stops the location updates.
		/// </summary>
		void Stop();

		/// <summary>
		/// Gets the latest location.
		/// </summary>
		void GetLocation();

		/// <summary>
		/// Gets the location address.
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		Task<string> GetLocationAddress(double latitude, double longitude);
	}
}
