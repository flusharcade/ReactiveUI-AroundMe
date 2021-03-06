﻿
namespace ReactiveUIAroundMe.iOS
{
	using System;
	using Foundation;

	using ReactiveUIAroundMe.Portable.UI;

	/// <summary>
	/// Path locator.
	/// </summary>
	public class PathLocator : IPathLocator
	{
		/// <summary>
		/// Gets the path.
		/// </summary>
		/// <returns>The path.</returns>
		/// <param name="fileName">File name.</param>
		/// <param name="type">Type.</param>
		public string GetPath(string fileName, string type)
		{
			return NSBundle.MainBundle.PathForResource(fileName, type);
		}
	}
}
