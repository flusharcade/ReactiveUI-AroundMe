// --------------------------------------------------------------------------------------------------
//  <copyright file="LogiOS.cs" company="Champion Data">
//    Copyright (c) 2014 Champion Data All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Droid.Logging
{
	using System;
	using System.Diagnostics;

	using ReactiveUIAroundMe.Portable.Logging;

	/// <summary>
	/// iOS log.
	/// </summary>
	public class LoggerAndroid : ILogger
	{
		#region Public Methods

		/// <summary>
		/// Writes the line.
		/// </summary>
		/// <returns>The line.</returns>
		/// <param name="text">Text.</param>
		public void WriteLine(string text)
		{
			Debug.WriteLine(text);
		}

		/// <summary>
		/// Writes the line time.
		/// </summary>
		/// <returns>The line time.</returns>
		/// <param name="text">Text.</param>
		/// <param name="args">Arguments.</param>
		public void WriteLineTime(string text, params object[] args)
		{
			Debug.WriteLine(DateTime.Now.Ticks + " " + String.Format(text, args));
		}

		#endregion
	}
}