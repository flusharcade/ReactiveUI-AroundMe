// --------------------------------------------------------------------------------------------------
//  <copyright file="LocationManager.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Location
{
	using System;
	using System.Reactive.Subjects;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
	
	using CoreLocation;
	using UIKit;

	using ReactiveUIAroundMe.Portable.Location;

	/// <summary>
	/// Location manager.
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
		private CLLocationManager _locMgr;

        /// <summary>
        /// The geocoder.
        /// </summary>
        private CLGeocoder _geocoder;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReactiveUIAroundMe.iOS.Location.LocationManager"/> class.
		/// </summary>
		public LocationManager ()
		{
			LocationUpdates = new Subject<LocationUpdatedEventArgs> ();
            AddressUpdates = new Subject<AddressUpdatedEventArgs>();

			_locMgr = new CLLocationManager ();
            _geocoder = new CLGeocoder();

			_locMgr.PausesLocationUpdatesAutomatically = false;

			// iOS 8 has additional permissions requirements
			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) 
			{
				_locMgr.RequestAlwaysAuthorization (); // works in background
			}

			// iOS 9 requires the following for background location updates
			// By default this is set to false and will not allow background updates
			if (UIDevice.CurrentDevice.CheckSystemVersion (9, 0)) 
			{
				_locMgr.AllowsBackgroundLocationUpdates = true;
			}
		}

		/// <summary>
		/// Gets the location mgr.
		/// </summary>
		/// <value>The location mgr.</value>
		public CLLocationManager LocMgr 
		{
			get 
			{ 
				return _locMgr; 
			}
		}

		/// <summary>
		/// Gets the latest location.
		/// </summary>
		public void GetLocation ()
		{
			// We need the user's permission for our app to use the GPS in iOS. This is done either by the user accepting
			// the popover when the app is first launched, or by changing the permissions for the app in Settings
			if (CLLocationManager.LocationServicesEnabled) 
			{
				//set the desired accuracy, in meters
				LocMgr.DesiredAccuracy = 1;
				LocMgr.StartUpdatingLocation ();

				Updates = Observable.FromEventPattern<CLLocationsUpdatedEventArgs> (LocMgr, "LocationsUpdated")
					.Window (() => Observable.Interval (TimeSpan.FromSeconds (2)))
					.SelectMany (x => x.Take (1))
					.Subscribe (e => NotifyLocationUpdate(e.EventArgs.Locations[e.EventArgs.Locations.Length - 1]));
			}
		}

		/// <summary>
		/// Gets the location address.
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		public async Task<string> GetLocationAddress(double latitude, double longitude)
		{
			var placemarks = await _geocoder.ReverseGeocodeLocationAsync(new CLLocation(latitude, longitude));
			if (placemarks?.Length > 0)
			{
				var placemark = placemarks[0];

				return string.Format("{0}, {1}, {2}, {3}", 
				              placemark.AddressDictionary["Street"], 
				              placemark.AddressDictionary["City"],
				              placemark.AddressDictionary["State"],
				              placemark.AddressDictionary["PostCodeExtension"]);
			}

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
		private void NotifyLocationUpdate(CLLocation location)
		{
            var latitude = location.Coordinate.Latitude;
            var longitude = location.Coordinate.Longitude;

			//var e.Locations [e.Locations.Length - 1]
			LocationUpdates.OnNext (new LocationUpdatedEventArgs 
				(new Location()
				{
					Longitude = longitude,
					Latitude = latitude,
				}));

            GetLocationAddress(latitude, longitude)
                .ContinueWith(_ => NotifyAddressUpdate(_.Result));
		}

        /// <summary>
        /// Start this instance.
        /// </summary>
        public void Start()
        {
            
        }

		/// <summary>
		/// Stops the location updates.
		/// </summary>
		public void Stop()
		{
			if (Updates != null)
			{
				Updates.Dispose ();
			}

			LocMgr.StopUpdatingLocation ();
		}
	}
}