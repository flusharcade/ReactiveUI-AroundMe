// --------------------------------------------------------------------------------------------------
//  <copyright file="LocationUpdatedEventArgs.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Location
{
	using System;

	using ReactiveUIAroundMe.Portable.Location;

	/// <summary>
	/// Location updated event arguments.
	/// </summary>
	public class LocationUpdatedEventArgs : EventArgs
	{
		/// <summary>
		/// The location.
		/// </summary>
		private Location _location;

		/// <summary>
		/// Gets the location.
		/// </summary>
		/// <value>The location.</value>
		public Location Location
		{
			get
			{
				return _location;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReactiveUIAroundMe.iOS.Location.LocationUpdatedEventArgs"/> class.
		/// </summary>
		/// <param name="location">Location.</param>
		public LocationUpdatedEventArgs(Location location)
		{
			_location = location;
		}
	}
}
