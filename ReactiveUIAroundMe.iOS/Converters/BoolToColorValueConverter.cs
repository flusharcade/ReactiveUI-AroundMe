// --------------------------------------------------------------------------------------------------
//  <copyright file="BoolToColorValueConverter.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Converters
{
	using System;
	using System.Linq;

	using ReactiveUI;
	using UIKit;

	using ReactiveUIAroundMe.iOS.Extensions;

	/// <summary>
	/// Bool to color value converter.
	/// </summary>
	public class BoolToColorValueConverter : IBindingTypeConverter
	{
		/// <summary>
		/// Gets the affinity for objects.
		/// </summary>
		/// <returns>The affinity for objects.</returns>
		/// <param name="fromType">From type.</param>
		/// <param name="toType">To type.</param>
		public int GetAffinityForObjects(Type fromType, Type toType)
		{
			if (fromType == typeof(string))
			{
				return 100;
			}

			return 0;
		}

		/// <summary>
		/// Tries the convert.
		/// </summary>
		/// <returns><c>true</c>, if convert was tryed, <c>false</c> otherwise.</returns>
		/// <param name="from">From.</param>
		/// <param name="toType">To type.</param>
		/// <param name="conversionHint">Conversion hint.</param>
		/// <param name="result">Result.</param>
		public bool TryConvert(object from, Type toType, object conversionHint, out object result)
		{
			try
			{
				// split string by ',', convert to int and store in case list
				//var cases = str.Split(',').Select(x => (x.Contains("Transparent")) ? UIColor.Clear
				//								  : UIColor.Clear.FromHex(x.Replace(" ", ""))).ToList();

				//result = (bool)from ? cases[0] : cases[1];
				//return true;
			}
			catch (Exception)
			{
			}

			result = false;

			return false;
		}
	}
}