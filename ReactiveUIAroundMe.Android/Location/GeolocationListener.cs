// --------------------------------------------------------------------------------------------------
//  <copyright file="GeolocationListener.cs" company="Health Connex">
//    Copyright (c) 2015 Health Connex All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using Android.Locations;
using Android.OS;

using Object = Java.Lang.Object;

namespace ReactiveUIAroundMe.Droid.Location
{
    /// <summary>
    /// Geolocation listener.
    /// </summary>
	internal class GeolocationListener : Object, ILocationListener
    {
        /// <summary>
        /// The providers.
        /// </summary>
        private readonly List<string> _providers;

        /// <summary>
        /// The locations.
        /// </summary>
        private readonly List<Android.Locations.Location> _locations;

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>The providers.</value>
        public List<string> Providers
        {
            get
            {
                return _providers;
            }
        }

        /// <summary>
        /// Gets the locations.
        /// </summary>
        /// <value>The locations.</value>
        public List<Android.Locations.Location> Locations
        {
            get
            {
                return _locations;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Droid.Location.GeolocationListener"/> class.
        /// </summary>
		public GeolocationListener()
		{
            _providers = new List<string>();
            _locations = new List<Android.Locations.Location>();
		}

        /// <summary>
        /// Ons the location changed.
        /// </summary>
        /// <param name="location">Location.</param>
		public void OnLocationChanged(Android.Locations.Location location)
		{
            _locations.Add(location);
		}

        /// <summary>
        /// Ons the provider disabled.
        /// </summary>
        /// <param name="provider">Provider.</param>
        public void OnProviderDisabled(string provider)
        {
            _providers.Remove(provider);
        }

        /// <summary>
        /// Ons the provider enabled.
        /// </summary>
        /// <param name="provider">Provider.</param>
        public void OnProviderEnabled(string provider)
		{
            if (!_providers.Contains(provider))
            {
                _providers.Add(provider);
            }
		}

        /// <summary>
        /// Ons the status changed.
        /// </summary>
        /// <param name="provider">Provider.</param>
        /// <param name="status">Status.</param>
        /// <param name="extras">Extras.</param>
		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			switch (status)
			{
				case Availability.Available:
					OnProviderEnabled(provider);
					break;

				case Availability.OutOfService:
					OnProviderDisabled(provider);
					break;
			}
		}
	}
}