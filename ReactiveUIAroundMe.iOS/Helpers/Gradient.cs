// --------------------------------------------------------------------------------------------------
//  <copyright file="DictionaryViews.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Extras
{
	using System;
	using System.Collections;

	using CoreAnimation;
	using CoreGraphics;
	using Foundation;
	using UIKit;

	/// <summary>
	/// Gradient.
	/// </summary>
	public static class Gradient 
	{
		/// <summary>
		/// Creates the view gradient.
		/// </summary>
		/// <returns>The view gradient.</returns>
		/// <param name="View">View.</param>
		/// <param name="view">View.</param>
		/*public static CAGradientLayer CreateViewGradient(this UIView View)
		{
			var gradient = new CAGradientLayer();
			gradient.Frame = View.Bounds;
			gradient.NeedsDisplayOnBoundsChange = true;
			gradient.MasksToBounds = true;

			View.BackgroundColor = UIColor.Black;

			gradient.Colors = new CGColor[]
			{
				UIColor.FromRGB(253, 253, 253).CGColor,
				UIColor.FromRGB(227, 227, 228).CGColor
			};

			View.Layer.AddSublayer(gradient);

			return gradient;
		}*/
	}
}