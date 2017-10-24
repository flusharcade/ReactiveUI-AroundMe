// --------------------------------------------------------------------------------------------------
//  <copyright file="Location.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Location
{
	using System;

	/// <summary>
	/// Class Position.
	/// </summary>
	public class Location
	{
		/// <summary>
		/// Gets or sets the timestamp.
		/// </summary>
		/// <value>The timestamp.</value>
		public DateTimeOffset Timestamp { get; set; }

		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		/// <value>The latitude.</value>
		public double Latitude { get; set; }

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		/// <value>The longitude.</value>
		public double Longitude { get; set; }

		/// <summary>
		/// Gets or sets the map x.
		/// </summary>
		/// <value>The map x.</value>
		public double MapX { get; set; }

		/// <summary>
		/// Gets or sets the map y.
		/// </summary>
		/// <value>The map y.</value>
		public double MapY { get; set; }

		/// <summary>
		/// Gets or sets the altitude in meters relative to sea level.
		/// </summary>
		/// <value>The altitude.</value>
		public double? Altitude { get; set; }

		/// <summary>
		/// Gets or sets the potential position error radius in meters.
		/// </summary>
		/// <value>The accuracy.</value>
		public double? Accuracy { get; set; }

		/// <summary>
		/// Gets or sets the potential altitude error range in meters.
		/// </summary>
		/// <value>The altitude accuracy.</value>
		/// <remarks>Not supported on Android, will always read 0.</remarks>
		public double? AltitudeAccuracy { get; set; }

		/// <summary>
		/// Gets or sets the heading in degrees relative to true North.
		/// </summary>
		/// <value>The heading.</value>
		public double? Heading { get; set; }

		/// <summary>
		/// Gets or sets the speed in meters per second.
		/// </summary>
		/// <value>The speed.</value>
		public double? Speed { get; set; }

		/// <summary>
		/// Apply the specified location.
		/// </summary>
		/// <returns>The apply.</returns>
		/// <param name="location">Location.</param>
		public void Apply(Location location)
		{
			Accuracy = location.Accuracy;
			Altitude = location.Altitude;
			AltitudeAccuracy = location.AltitudeAccuracy;
			Heading = location.Heading;
			Latitude = location.Latitude;
			Longitude = location.Longitude;
			Speed = location.Speed;
			Timestamp = location.Timestamp;
		}
	}
}