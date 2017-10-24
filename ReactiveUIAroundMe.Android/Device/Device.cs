// --------------------------------------------------------------------------------------------------
//  <copyright file="AndroidDevice.cs" company="Flush Arcade">
//    Copyright (c) 2015 Flush Arcade All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Droid.Device
{
	using ReactiveUIAroundMe.Portable;

	public class Device : IDevice
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Droid.Device.Device"/> class.
        /// </summary>
        public Device()
        {
			//var context = Forms.Context;

			//// TODO: Search device model for tablet/phone
			//deviceType = DeviceType.Tablet;

			//Culture = CultureInfo.CurrentCulture.ToString();
			//AppVersion = context.PackageManager.GetPackageInfo (context.PackageName, 0).VersionName;

			//TelephonyManager telephonyManager = (TelephonyManager)context.GetSystemService(Context.TelephonyService);
			//DeviceId = telephonyManager.DeviceId;

			//ResolutionScale = 1;
		}

		#region IDevice implementation

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
				return "Android";
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

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Android_Shared.Controls.AndroidDevice"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Android_Shared.Controls.AndroidDevice"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[AndroidDevice: DeviceName={0}, DeviceId={1}, OSVersion={2}, AppVersion={3}, AppName={4}, Culture={5}, ResolutionScale={6}, Height={7}, Width={8}, Platform={9}]", DeviceName, DeviceId, OSVersion, AppVersion, AppName, Culture, ResolutionScale, Height, Width, Platform);
		}

		#endregion
	}
}
