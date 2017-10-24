// --------------------------------------------------------------------------------------------------
//  <copyright file="AddressUpdatedEventArgs.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Location
{
	using System;

	/// <summary>
	/// Address updated event arguments.
	/// </summary>
	public class AddressUpdatedEventArgs : EventArgs
	{
		/// <summary>
		/// The addresses.
		/// </summary>
		public string Address;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.Location.AddressUpdatedEventArgs"/> class.
		/// </summary>
		/// <param name="address">Address.</param>
		public AddressUpdatedEventArgs(string address)
		{
			Address = address;
		}
	}
}
