// --------------------------------------------------------------------------------------------------
//  <copyright file="GeolocationSingleListener.cs" company="Health Connex">
//    Copyright (c) 2015 Health Connex All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Reactive.Subjects;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;

using Object = Java.Lang.Object;

using ReactiveUIAroundMe.Portable.Location;

namespace ReactiveUIAroundMe.Droid.Location
{
    /// <summary>
    ///     Class GeolocationSingleListener.
    /// </summary>
    public class LocationManager : ILocationManager
    {
        /// <summary>
        /// Gets or sets the updates.
        /// </summary>
        /// <value>The updates.</value>
        public IDisposable Updates { get; set; }

        /// <summary>
        /// Occurs when location updated.
        /// </summary>
        public Subject<LocationUpdatedEventArgs> LocationUpdates { get; set; }

        /// <summary>
        /// Gets or sets the address updates.
        /// </summary>
        /// <value>The address updates.</value>
        public Subject<AddressUpdatedEventArgs> AddressUpdates { get; set; }

		/// <summary>
        /// The location mgr.
        /// </summary>
		private readonly Android.Locations.LocationManager _locMgr;

        /// <summary>
        /// The geolocation listener.
        /// </summary>
        private readonly GeolocationListener _geolocationListener;

		/// <summary>
        /// The providers.
        /// </summary>
		private readonly string[] _providers;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MyCareManager.Droid.Geolocation.LocationManager"/> class.
		/// </summary>
		public LocationManager()
        {
            LocationUpdates = new Subject<LocationUpdatedEventArgs>();

			_locMgr = (Android.Locations.LocationManager)Application.Context.GetSystemService(Context.LocationService);
			_providers = _locMgr.GetProviders(false)
                                .Where(s => s != Android.Locations.LocationManager.PassiveProvider)
                                .ToArray();

            _geolocationListener = new GeolocationListener();
        }

        /// <summary>
        /// Start this instance.
        /// </summary>
        public void Start()
        {
			//_locMgr.RequestLocationUpdates(_providers[i], 0, 0, _geolocationListener, Looper.MainLooper);
		}

		/// <summary>
		/// Stops the location updates.
		/// </summary>
		public void Stop()
		{
			if (Updates != null)
			{
				Updates.Dispose();
			}

			_locMgr.Dispose();
		}

        /// <summary>
        /// Gets the latest location.
        /// </summary>
        public void GetLocation()
        {
            //_locMgr.RequestLocationUpdates(_providers[i], 0, 0, _geolocationListener, Looper.MainLooper);
            
        }

        /// <summary>
        /// Gets the location address.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        public async Task<string> GetLocationAddress(double latitude, double longitude)
        {

            return string.Empty;
        }

        /// <summary>
        /// Notifies the address update.
        /// </summary>
        /// <param name="address">Address.</param>
        private void NotifyAddressUpdate(string address)
        {
            AddressUpdates.OnNext(new AddressUpdatedEventArgs(address));
        }

        /// <summary>
        /// Notifies the update.
        /// </summary>
        /// <param name="location">Location.</param>
        private void NotifyLocationUpdate(Android.Locations.Location location)
        {
            LocationUpdates.OnNext(
                new LocationUpdatedEventArgs
                (new Portable.Location.Location()
                {
                    Longitude = location.Longitude,
                    Latitude = location.Latitude,
                }));
        }

    }
}