// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDevice.cs" company="Champion Data.">
//   Copyright (c) 2015 Champion Data. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable
{
	public interface IDevice
	{
		/// <summary>
		/// Gets or sets the name of the device.
		/// </summary>
		/// <value>The name of the device.</value>
		string DeviceName { get; set; }

		/// <summary>
		/// Gets or sets the device identifier.
		/// </summary>
		/// <value>The device identifier.</value>
		string DeviceId { get; set; }

		/// <summary>
		/// Gets the platform.
		/// </summary>
		/// <value>The platform.</value>
		string Platform { get;}

		/// <summary>
		/// Gets or sets the OS version.
		/// </summary>
		/// <value>The OS version.</value>
		string OSVersion { get; set; }

		/// <summary>
		/// Gets or sets the app version.
		/// </summary>
		/// <value>The app version.</value>
		string AppVersion { get; set; }

		/// <summary>
		/// Gets or sets the name of the app.
		/// </summary>
		/// <value>The name of the app.</value>
		string AppName { get; set; }

		/// <summary>
		/// Gets or sets the culture.
		/// </summary>
		/// <value>The culture.</value>
		string Culture { get; set; }

		/// <summary>
		/// Gets or sets the resolution scale.
		/// </summary>
		/// <value>The resolution scale.</value>
		double ResolutionScale { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		/// <value>The height.</value>
		int Height { get; set; }

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		/// <value>The width.</value>
		int Width { get; set; }
	}
}