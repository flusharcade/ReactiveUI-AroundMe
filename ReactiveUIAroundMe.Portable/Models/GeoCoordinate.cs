// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoCoordinate.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Models
{
	using System;

	/// <summary>
	/// Geo coordinate.
	/// </summary>
	public class GeoCoordinate
	{
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
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.Models.GeoCoordinate"/> class.
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		public GeoCoordinate(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:ReactiveUIAroundMe.Portable.Models.GeoCoordinate"/>.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:ReactiveUIAroundMe.Portable.Models.GeoCoordinate"/>.</returns>
		public override string ToString()
		{
			return string.Format("{0},{1}", Latitude, Longitude);
		}
	}
}
