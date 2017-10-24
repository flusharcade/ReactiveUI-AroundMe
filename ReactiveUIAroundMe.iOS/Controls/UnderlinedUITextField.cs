// --------------------------------------------------------------------------------------------------
//  <copyright file="UnderlinedUITextField.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Controls
{
	using System;

	using UIKit;
	using CoreGraphics;

	using ReactiveUIAroundMe.iOS.Extensions;

	/// <summary>
	/// Underlined UIT ext field.
	/// </summary>
	public class UnderlinedUITextField : UITextField
	{
		/// <summary>
		/// Draw the specified rect.
		/// </summary>
		/// <returns>The draw.</returns>
		/// <param name="rect">Rect.</param>
		public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);

			Layer.BackgroundColor = UIColor.White.CGColor;
			Layer.MasksToBounds = false;
			Layer.ShadowColor = UIColor.Clear.FromHex("#999999").CGColor;
			Layer.ShadowOffset = new CGSize(0, 1);
			Layer.ShadowOpacity = 1.0f;
			Layer.ShadowRadius = 0.0f;
		}
	}
}
