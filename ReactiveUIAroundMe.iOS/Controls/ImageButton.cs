// --------------------------------------------------------------------------------------------------
//  <copyright file="ImageButton.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Controls
{
	using System;

	using UIKit;

	/// <summary>
	/// Image button.
	/// </summary>
	public class ImageButton : UIButton
	{
		/// <summary>
		/// The transparent background.
		/// </summary>
		private UIView _transparentBackground;

		/// <summary>
		/// The is flipped.
		/// </summary>
		public bool IsFlipped
        {
            get
            {
                return FlippedX || FlippedY;
            }
        }

		/// <summary>
		/// The index of the flip.
		/// </summary>
		public int FlipIndex;

        /// <summary>
        /// The flipped x.
        /// </summary>
        public bool FlippedX;

        /// <summary>
        /// The flipped y.
        /// </summary>
        public bool FlippedY;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.Controls.ImageButton"/> class.
		/// </summary>
		public ImageButton()
		{
			_transparentBackground = new UIView()
			{
				BackgroundColor = UIColor.White,
				UserInteractionEnabled = false,
				Alpha = 0.05f
			};

			InsertSubview(_transparentBackground, 0);
		}

		/// <summary>
		/// Layouts the subviews.
		/// </summary>
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			// update bounds of transparent background
			var backgroundFrame = _transparentBackground.Frame;
			backgroundFrame.Width = Bounds.Width;
			backgroundFrame.Height = Bounds.Height;
			_transparentBackground.Frame = backgroundFrame;

			var imgFrame = ImageView.Frame;
			var imgSize = ImageView.Frame.Size;

			imgSize.Height = Bounds.Height / 4f;
			imgSize.Width = Bounds.Width / 4f;
			ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			imgFrame.X = (Bounds.Width / 2.0f) - (imgSize.Width / 2);
			imgFrame.Y = (Bounds.Height / 2.0f) - (Bounds.Height / 4.0f);

			imgFrame.Size = imgSize;
			ImageView.Frame = imgFrame;

			var titleFrame = TitleLabel.Frame;
			var titleFrameSize = titleFrame.Size;

			TitleLabel.TextAlignment = UITextAlignment.Center;
			TitleLabel.Font = UIFont.FromName("Helvetica", 11f);

			titleFrame.X = 0;
			titleFrame.Y = (imgFrame.Y + imgFrame.Height);
			titleFrameSize.Width = Bounds.Width;
			titleFrameSize.Height = 30;

			titleFrame.Size = titleFrameSize;
			TitleLabel.Frame = titleFrame;
		}
	}
}
