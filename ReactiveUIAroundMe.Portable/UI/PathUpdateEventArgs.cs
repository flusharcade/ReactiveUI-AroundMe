// --------------------------------------------------------------------------------------------------
//  <copyright file="PathUpdateEventArgs.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.UI
{
	using System;
	using System.Collections.Generic;

	using ReactiveUIAroundMe.Portable.Models;

	/// <summary>
	/// Path update event arguments.
	/// </summary>
	public class PathUpdateEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the start.
		/// </summary>
		/// <value>The start.</value>
		public GeoCoordinate StartCoordinate { get; set; }

		/// <summary>
		/// Gets or sets the end.
		/// </summary>
		/// <value>The end.</value>
		public GeoCoordinate EndCoordinate { get; set; }

		/// <summary>
		/// Gets or sets the encoded points.
		/// </summary>
		/// <value>The encoded points.</value>
		public List<GeoCoordinate> Path { get; set; }
	}
}
