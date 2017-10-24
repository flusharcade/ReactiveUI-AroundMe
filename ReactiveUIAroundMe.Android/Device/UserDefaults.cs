// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserDefaults.cs" company="Flush Arcade">
//   Copyright (c) 2015 Flush Arcade All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Droid.Device
{
	using System.Security.Cryptography;
	using System.Text;
	using System;

	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.Resources;

    public class UserDefaults : IUserDefaults
	{
		#region Private Fields

		/// <summary>
		/// The _settings.
		/// </summary>
		private readonly ISettings _settings;

		/// <summary>
		/// The md5.
		/// </summary>
		private readonly MD5 _md5;

		#endregion

		#region IUserDefaults implementation

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MyCareManager.Droid.Device.UserDefaults"/> is authenticated.
		/// </summary>
		/// <value><c>true</c> if authenticated; otherwise, <c>false</c>.</value>
		public bool Authenticated {
			get
			{
				return _settings.GetValueOrDefault("Authenticated", false);
			}
			set
			{
				_settings.AddOrUpdateValue("Authenticated", value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MyCareManager.Droid.Device.UserDefaults"/> record
		/// identifier set.
		/// </summary>
		/// <value><c>true</c> if record identifier set; otherwise, <c>false</c>.</value>
		public string Host {
			get
			{
				return _settings.GetValueOrDefault("Host", string.Empty);
			}
			set
			{
				_settings.AddOrUpdateValue("Host", value);
			}
		}

		/// <summary>
		/// Gets the md5 hash.
		/// </summary>
		/// <returns>The md5 hash.</returns>
		/// <param name="input">Input.</param>
		public string GetMd5Hash(string input)
		{
			// Convert the input string to a byte array and compute the hash.
			byte[] data = _md5.ComputeHash(Encoding.UTF8.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}

		/// <summary>
		/// Gets the md5 value.
		/// </summary>
		/// <returns>The md5 value.</returns>
		/// <param name="hash">Hash.</param>
		public bool VerifyMd5Hash(string input, string hash)
		{
			// Hash the input.
			string hashOfInput = GetMd5Hash(input);

			// Create a StringComparer an compare the hashes.
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			if (0 == comparer.Compare(hashOfInput, hash))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Removes from defaults.
		/// </summary>
		/// <param name="key">Key.</param>
		public void RemoveFromDefaults(string key)
		{
			_settings.Remove(key);
		}


		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Droid.Device.UserDefaults"/> class.
		/// </summary>
		/// <param name="settings">Settings.</param>
		public UserDefaults(ISettings settings)
		{
			_settings = settings;
			_md5 = MD5.Create();
		}
	}
}