// --------------------------------------------------------------------------------------------------
//  <copyright file="LocationView.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Controls
{
	using System.Linq;

	using UIKit;
	using CoreGraphics;

	using ReactiveUIAroundMe.iOS.Extras;

    /// <summary>
    ///  This is the control used to embed into Xamarin Forms that will yield a custom rendered for tapping focus
    /// </summary>
    public sealed class LocationView : UIView
	{
		/// <summary>
		/// The image target bound.
		/// </summary>
		const int IMG_TARGET_BOUND = 40;

		/// <summary>
		/// The focal target.
		/// </summary>
		readonly UIImageView _locationTargetImageView;

        /// <summary>
		/// The location text holder image view.
		/// </summary>
		readonly UIImageView _locationPlaceholderImageView;

        /// <summary>
		/// The location placeholder label.
		/// </summary>
		readonly UILabel _locationPlaceholderLabel;

        /// <summary>
		/// The touch point.
		/// </summary>
		private CGPoint _touchPoint;

		/// <summary>
		/// The is animating.
		/// </summary>
		private bool _isAnimating;

		/// <summary>
		/// Animates the focal target.
		/// </summary>
		/// <param name="touchPoint">Touch point.</param>
		private void AnimateFocalTarget(CGPoint touchPoint)
		{
            var changed = !_touchPoint.Equals(touchPoint);

			_touchPoint = touchPoint;

			BeginAnimations("AnimateFocalTarget");

			if (changed)
            {
				_locationTargetImageView.Alpha = 0.0f;
				_locationPlaceholderImageView.Alpha = 0.0f;
				_locationPlaceholderImageView.Frame = new CGRect(0, 0, 200, IMG_TARGET_BOUND);
			}

			_locationTargetImageView.Frame = new CGRect(touchPoint.X - (IMG_TARGET_BOUND / 2), 
                                                        touchPoint.Y - (IMG_TARGET_BOUND / 2), 
                                                        IMG_TARGET_BOUND, IMG_TARGET_BOUND);

			Animate(1, 0, UIViewAnimationOptions.CurveEaseIn, () =>
			{
				_locationTargetImageView.Alpha = 1.0f;
			},
			() =>
			{
				Animate(0.25, 0, UIViewAnimationOptions.CurveEaseIn, () =>
				{
					var targetFrame = new CGRect(touchPoint.X - (IMG_TARGET_BOUND / 4), touchPoint.Y - (IMG_TARGET_BOUND / 4), (IMG_TARGET_BOUND / 2), (IMG_TARGET_BOUND / 2));
					// add placeholder frame to the right of target with spacing
					_locationTargetImageView.Frame = targetFrame;

					var placeholderFrame = _locationPlaceholderImageView.Frame;

					if (changed)
                    {
						// if target is to the left of the middle, place placeholder to the right,
						// and vice versa
						if (targetFrame.X < (this.Bounds.Width / 2))
						{
							placeholderFrame.X += targetFrame.X + targetFrame.Width + 20;
							_locationPlaceholderImageView.Transform = CGAffineTransform.MakeScale(1, 1);
						}
						else
						{
							placeholderFrame.X += targetFrame.X - (placeholderFrame.Width + 25);
							_locationPlaceholderImageView.Transform = CGAffineTransform.MakeScale(-1, 1);
						}

						placeholderFrame.Y += targetFrame.Y - (targetFrame.Height / 2);

						var labelFrame = placeholderFrame;
						labelFrame.X += 27;
						labelFrame.Width -= 27;

						_locationPlaceholderImageView.Frame = placeholderFrame;
						_locationPlaceholderLabel.Frame = labelFrame;

						_locationPlaceholderLabel.Alpha = 1.0f;
						_locationPlaceholderImageView.Alpha = 1.0f;
					}
				}, null);
			});

			CommitAnimations();

			_isAnimating = false;
		}

		/// <summary>
		/// Focuses the location.
		/// </summary>
		/// <param name="locationPoint">Location point.</param>
		public void FocusLocation(CGPoint locationPoint)
		{
			if (_isAnimating)
			{
				return;
			}

			_locationTargetImageView.Alpha = 0.0f;
			_isAnimating = true;

			InvokeOnMainThread(() => AnimateFocalTarget(locationPoint));
		}

        /// <summary>
        /// Sets the address.
        /// </summary>
        /// <param name="address">Address.</param>
        public void SetAddress(string address)
        {
            InvokeOnMainThread(() => _locationPlaceholderLabel.Text = address);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.Controls.LocationView"/> class.
		/// </summary>
		public LocationView()
		{
			_locationTargetImageView = new UIImageView(new CGRect(0, 0, IMG_TARGET_BOUND, IMG_TARGET_BOUND))
			{
				ContentMode = UIViewContentMode.ScaleAspectFill,
				Image = UIImage.FromFile("location_target.png"),
				Alpha = 0.0f
			};

			_locationPlaceholderImageView = new UIImageView(new CGRect(0, 0, 200, IMG_TARGET_BOUND))
			{
				ContentMode = UIViewContentMode.ScaleAspectFill,
				Image = UIImage.FromFile("location_placeholder.png"),
				Alpha = 0.0f
			};

            _locationPlaceholderLabel = new UILabel()
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.FromName("Helvetica", 14f),
                TextColor = iOSColorPalette.Maroon,
                Alpha = 0.0f
            };

			var topImageView = new UIImageView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				ContentMode = UIViewContentMode.ScaleAspectFill,
				Image = UIImage.FromFile("bg-top.jpg"),
			};

			var mapImageView = new UIImageView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				ContentMode = UIViewContentMode.ScaleAspectFill,
				Image = UIImage.FromFile("map.png"),
			};

			Add(topImageView);
			Add(mapImageView);
			Add(_locationTargetImageView);
			Add(_locationPlaceholderImageView);
            Add(_locationPlaceholderLabel);

			var views = new DictionaryViews()
			{
				{"topImageView", topImageView},
				{"mapImageView", mapImageView},
			};

			AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[topImageView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|[mapImageView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[topImageView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[mapImageView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.ToArray());
		}
	}
}