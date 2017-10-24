// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOSDevice.cs" company="Champion Data.">
//   Copyright (c) 2015 Champion Data. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Device 
{
	using System.Globalization;

	using UIKit;
	using Foundation;

	using ReactiveUIAroundMe.Portable;

	/// <summary>
	/// Device iOS.
	/// </summary>
	public class Device : IDevice
	{
		/// <summary>
		/// Gets the device details.
		/// </summary>
		public Device()
		{
			Culture = CultureInfo.CurrentCulture.ToString();

			AppVersion = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();

			Width = (int)UIScreen.MainScreen.Bounds.Width;
			Height = (int)UIScreen.MainScreen.Bounds.Height;

			DeviceId = UIDevice.CurrentDevice.IdentifierForVendor.ToString();

			ResolutionScale = 1;
		}

		#region Fields

		/// <summary>
		/// Gets or sets the name of the device.
		/// </summary>
		/// <value>The name of the device.</value>
		public string DeviceName { get; set; }

		/// <summary>
		/// Gets or sets the device identifier.
		/// </summary>
		/// <value>The device identifier.</value>
		public string DeviceId { get; set; }

		/// <summary>
		/// Gets the platform.
		/// </summary>
		/// <value>The platform.</value>
		public string Platform
		{
			get
			{
				return "iOS";
			}
		}

		/// <summary>
		/// Gets or sets the OS version.
		/// </summary>
		/// <value>The OS version.</value>
		public string OSVersion { get; set; }

		/// <summary>
		/// Gets or sets the app version.
		/// </summary>
		/// <value>The app version.</value>
		public string AppVersion { get; set; }

		/// <summary>
		/// Gets or sets the name of the app.
		/// </summary>
		/// <value>The name of the app.</value>
		public string AppName { get; set; }

		/// <summary>
		/// Gets or sets the culture.
		/// </summary>
		/// <value>The culture.</value>
		public string Culture { get; set; }

		/// <summary>
		/// Gets or sets the resolution scale.
		/// </summary>
		/// <value>The resolution scale.</value>
		public double ResolutionScale { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		/// <value>The height.</value>
		public int Height { get; set; }

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		/// <value>The width.</value>
		public int Width { get; set; }

		#endregion

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="iOS_Shared.Hardware.IOSDevice"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="iOS_Shared.Hardware.IOSDevice"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[IOSDevice: DeviceName={0}, OSVersion={1}, AppVersion={2}, AppName={3}, Culture={4}, ResolutionScale={5}, Height={6}, Width={7}, Platform={8}]", DeviceName, OSVersion, AppVersion, AppName, Culture, ResolutionScale, Height, Width, Platform);
		}
	}
}