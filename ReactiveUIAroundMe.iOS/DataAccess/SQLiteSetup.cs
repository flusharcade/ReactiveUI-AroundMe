// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SQLiteSetup.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.DataAccess
{
	using System.IO;
	using System;

	using SQLite.Net.Interop;

	using ReactiveUIAroundMe.Portable.DataAccess;

	/// <summary>
	/// The SQLite setup object.
	/// </summary>
	public class SQLiteSetup : ISQLiteSetup
	{
		public string DatabasePath { get; set; }

		public object Platform { get; set; }

		public SQLiteSetup(object platform)
		{
			DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ReactiveUIAroundMe.db3");
			Platform = platform;
		}
	}
}