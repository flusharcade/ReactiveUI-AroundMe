// --------------------------------------------------------------------------------------------------
//  <copyright file="BoolToColorValueConverter.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Droid.Converters
{
	using System;
	using System.Linq;

	using ReactiveUI;

	/// <summary>
	/// Bool to color value converter.
	/// </summary>
	public class NotConverter : IBindingTypeConverter
	{
		/// <summary>
		/// Gets the affinity for objects.
		/// </summary>
		/// <returns>The affinity for objects.</returns>
		/// <param name="fromType">From type.</param>
		/// <param name="toType">To type.</param>
		public int GetAffinityForObjects(Type fromType, Type toType)
		{
			if (fromType == typeof(bool))
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
				result = !(bool)from;
			}
			catch (Exception ex)
			{
				//this.Log().WarnException("Couldn't convert object to type: " + toType, ex);
				result = null;
				return false;
			}

			return true;
		}
	}
}