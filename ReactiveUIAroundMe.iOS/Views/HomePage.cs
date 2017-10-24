// --------------------------------------------------------------------------------------------------
//  <copyright file="HomePage.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Views
{
	using System.Linq;
	using System.Reactive.Linq;
	using System.Collections.Generic;
	using System;

	using UIKit;
	using CoreGraphics;

	using ReactiveUI;

	using ReactiveUIAroundMe.iOS.Extras;
	using ReactiveUIAroundMe.iOS.Controls;
	using ReactiveUIAroundMe.iOS.Converters;

	using ReactiveUIAroundMe.Portable.ViewModels;
	using ReactiveUIAroundMe.Portable.Location;

	/// <summary>
	/// Home page.
	/// </summary>
	public class HomePage : BaseViewController, IViewFor<HomePageViewModel>
	{
		#region Constants

		/// <summary>
		/// The button image paths.
		/// </summary>
		private static string[] _buttonImagePaths = new string[]
		{
			"search_icon",
			"rates_icon",
			"feedback_icon",
			"blog_icon",
			"settings_icon",
			"share_icon",
		};

		/// <summary>
		/// The button titles.
		/// </summary>
		private static string[] _buttonTitles = new string[]
		{
			"Search",
			"Rates",
			"Feedback",
			"Blog",
			"Settings",
			"Share",
		};

		#endregion

		/// <summary>
		/// The location view.
		/// </summary>
		private LocationView _locationView;

		/// <summary>
		/// The location view.
		/// </summary>
		private UIView _screenSaverView;

		/// <summary>
		/// The progress view.
		/// </summary>
        private UIActivityIndicatorView _progressView;

		/// <summary>
		/// The buttons.
		/// </summary>
		private List<ImageButton> _buttons;

        /// <summary>
        /// The screen saver showing.
        /// </summary>
        private bool _screenSaverShowing;

		/// <summary>
		/// Gets or sets the view model.
		/// </summary>
		/// <value>The view model.</value>
		public new HomePageViewModel ViewModel
		{
			get { return (HomePageViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		/// <summary>
		/// Gets or sets the reactive user interface . IV iew for. view model.
		/// </summary>
		/// <value>The reactive user interface . IV iew for. view model.</value>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (HomePageViewModel)value;}
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.NavigationController.SetNavigationBarHidden(true, false);

			_buttons = new List<ImageButton>();

			Title = "Home";

			var mainView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};

			var buttonView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};

			Add(mainView);

			/* ----- image views ----- */

			_locationView = new LocationView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};

			var logoImageView = new UIImageView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				ContentMode = UIViewContentMode.ScaleAspectFit,
				Image = UIImage.FromFile("logo.png"),
			};

			var bottomImageView = new UIImageView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				ContentMode = UIViewContentMode.ScaleAspectFill,
				Image = UIImage.FromFile("bg-bottom.jpg"),
			};

			var transparentBackgroundView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Black,
				Alpha = 0.05f
			};

			_progressView = new UIActivityIndicatorView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Color = UIColor.Black,
			};

            _screenSaverView = new UIView()
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Alpha = 0.0f,
                Hidden = false
			};

			var screenSaverTapGesture = new UITapGestureRecognizer(() =>
            {
                _screenSaverView.Alpha = 0.0f;
                _screenSaverView.Hidden = true;
                _screenSaverShowing = false;

                ViewModel.ResetActions();
            });

            _screenSaverView.AddGestureRecognizer(screenSaverTapGesture);

			mainView.Add(_locationView);
			mainView.Add(_progressView);
			mainView.Add(logoImageView);

			mainView.Add(buttonView);

			buttonView.Add(bottomImageView);
			buttonView.Add(transparentBackgroundView);

            mainView.Add(_screenSaverView);

			/* ----- image views ----- */

			/* ----- buttons ----- */

			var buttonViews = new DictionaryViews()
			{
				{"bottomImageView", bottomImageView},
				{"transparentBackgroundView", transparentBackgroundView},
			};

			for (int i = 0; i < _buttonTitles.Length; i++) 
			{
				var button = new ImageButton()
				{
					TranslatesAutoresizingMaskIntoConstraints = false,
				};

				var title = _buttonTitles[i];

				StyleButton(button, _buttonImagePaths[i], _buttonTitles[i]);
				buttonViews.Add(title.ToLower() + "Button", button);
				buttonView.Add(button);
				// add to button list so we can perform flips
				_buttons.Add(button);
			}

			/* ----- buttons ----- */

			var views = new DictionaryViews()
			{
				{"mainView", mainView},
			};

			var mainViews = new DictionaryViews()
			{
				{"locationView", _locationView},
				{"progressView", _progressView},
				{"logoImageView", logoImageView},
				{"buttonView", buttonView},
                {"screenSaverView", _screenSaverView},
			};

			View.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|[mainView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[mainView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.ToArray());

			mainView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|[locationView][logoImageView][buttonView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, mainViews)
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|[progressView][logoImageView][buttonView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, mainViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|[screenSaverView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, mainViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[screenSaverView]|", NSLayoutFormatOptions.AlignAllTop, null, mainViews))
                .Concat(NSLayoutConstraint.FromVisualFormat("H:|[progressView]|", NSLayoutFormatOptions.AlignAllTop, null, mainViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[locationView]|", NSLayoutFormatOptions.AlignAllTop, null, mainViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[buttonView]|", NSLayoutFormatOptions.AlignAllTop, null, mainViews))
				.Concat(new[] { NSLayoutConstraint.Create(buttonView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, _locationView, NSLayoutAttribute.Height, 0.8f, 0) })
				.Concat(new[] { NSLayoutConstraint.Create(logoImageView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, mainView, NSLayoutAttribute.Width, 0.3f, 0) })
				// height is proportional to width
				.Concat(new[] { NSLayoutConstraint.Create(logoImageView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, logoImageView, NSLayoutAttribute.Width, 1f, 0) })
				.Concat(new[] { NSLayoutConstraint.Create(logoImageView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, mainView, NSLayoutAttribute.CenterX, 1f, 0) })
				.ToArray());

			buttonView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|[bottomImageView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, buttonViews)
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|[transparentBackgroundView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, buttonViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-1-[searchButton]-1-[blogButton(searchButton)]-1-|", NSLayoutFormatOptions.DirectionLeftToRight, null, buttonViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-1-[ratesButton]-1-[settingsButton(ratesButton)]-1-|", NSLayoutFormatOptions.DirectionLeftToRight, null, buttonViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-1-[feedbackButton]-1-[shareButton(feedbackButton)]-1-|", NSLayoutFormatOptions.DirectionLeftToRight, null, buttonViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[bottomImageView]|", NSLayoutFormatOptions.AlignAllTop, null, buttonViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[transparentBackgroundView]|", NSLayoutFormatOptions.AlignAllTop, null, buttonViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-1-[searchButton]-1-[ratesButton(searchButton)]-1-[feedbackButton(searchButton)]-1-|", NSLayoutFormatOptions.AlignAllTop, null, buttonViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-1-[blogButton]-1-[settingsButton(blogButton)]-1-[shareButton(blogButton)]-1-|", NSLayoutFormatOptions.AlignAllTop, null, buttonViews))
				.ToArray());
			
			// search button
			this.BindCommand(ViewModel, x => x.SearchCommand, x => x._buttons[0]);

			this.WhenActivated(d =>
			{
				// HACK: Clean this up with proper navigation
				ViewModel.OnShow(new Dictionary<string, object>());
			});
		}

		/// <summary>
		/// Posts the load subscriptions.
		/// </summary>
		/// <returns>The load subscriptions.</returns>
		public override IEnumerable<IDisposable> LoadSubscriptions()
		{
			yield return Observable.FromEventPattern<string>(ViewModel, "AddressUpdate")
				.Window(() => Observable.Interval(TimeSpan.FromSeconds(2)))
				.SelectMany(x => x.Take(1))
				.Subscribe(e =>
				{
					var address = e.EventArgs;
					_locationView.SetAddress(address);
				});

			yield return Observable.FromEventPattern<Location>(ViewModel, "LocationUpdate")
				.Window(() => Observable.Interval(TimeSpan.FromSeconds(2)))
				.SelectMany(x => x.Take(1))
				.Subscribe(e =>
                {
	                var location = e.EventArgs;

					var x = (location.Longitude + 180) / 360 * _locationView.Frame.Width;
					var y = ((1 - Math.Log(Math.Tan(location.Latitude * Math.PI / 180) + 1 / Math.Cos(location.Latitude * Math.PI / 180)) / Math.PI) / 2 * Math.Pow(2, 0))
						* _locationView.Frame.Height;

	                _locationView.FocusLocation(new CGPoint(x, y));
				});

			yield return ViewModel.WhenAnyValue(vm => vm.LocationLoading)
				.SubscribeOn(ViewModel.Scheduler)
				.Subscribe(loading =>
				{
					if (loading)
					{
						_progressView.StartAnimating();
					}
					else
					{
						_progressView.StopAnimating();
					}
				});

			yield return Observable.FromEventPattern(ViewModel, "Flip")
							.Subscribe(e => FlipButtons());

			yield return Observable.FromEventPattern(ViewModel, "FlipReset")
							.Subscribe(e => ResetFlipButtons());
		}

		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear(bool animated)
		{
			// hide nav bar everytime this page shows
			NavigationController.SetNavigationBarHidden(true, false);

			ViewModel?.UpdateLocation();

			base.Load();

			base.ViewWillAppear(animated);
		}

		/// <summary>
		/// Views the will disappear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillDisappear(bool animated)
		{
			base.DisposePage();

			base.ViewWillDisappear(animated);
		}

        /// <summary>
        /// Resets the flip buttons.
        /// </summary>
        private void ResetFlipButtons()
        {
            foreach (var button in _buttons)
            {
                if (button.FlippedX)
                {
                    FlipX(button);
                }
                else if (button.FlippedY)
                {
                    FlipY(button);
                }
            }
        }

		/// <summary>
		/// Flips the buttons.
		/// </summary>
		private void FlipButtons()
		{
            if (!_screenSaverShowing)
            {
                _screenSaverView.Hidden = false;
                _screenSaverView.Alpha = 1.0f;
            }

			var index = 0;

			foreach (var button in _buttons)
			{
				var rnd = new Random();

				// one randomize flip direction, and one randomize should flip
				var flipIndex = rnd.Next(0, 2);
				var shouldFlipIndex = rnd.Next(0, 2);

				if (button.IsFlipped)
				{
					flipIndex = button.FlipIndex;
					shouldFlipIndex = 1;
				}

				button.FlipIndex = flipIndex;

				// if we should flip button
				if (shouldFlipIndex == 1)
				{
					switch (flipIndex)
					{
						case 0:
							FlipX(button);
                            button.FlippedX = true;
                            button.FlippedY = false;
							break;
						case 1:
							FlipY(button);
                            button.FlippedX = false;
                            button.FlippedY = true;
							break;
					}
				}

				index++;
			}
		}

		/// <summary>
		/// Flips the x.
		/// </summary>
		/// <param name="view">View.</param>
		private void FlipX(ImageButton button)
		{
			UIView.BeginAnimations(button.ToString());

			UIView.Animate(1, 1, UIViewAnimationOptions.CurveEaseIn, () =>
			{
				var transform = CGAffineTransform.MakeTranslation(0f, button.Frame.Width);

				if (button.IsFlipped)
				{
					transform = CGAffineTransform.MakeScale(1.0f, 1.0f);
				}
				else
				{
					transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
				}

				button.Transform = transform;
                button.FlippedX = !button.FlippedX;
			}, () =>
			{

			});

			UIView.CommitAnimations();
		}

		/// <summary>
		/// Flips the y.
		/// </summary>
		/// <param name="view">View.</param>
		private void FlipY(ImageButton button)
		{
			UIView.BeginAnimations(button.ToString());

			UIView.Animate(1, 1, UIViewAnimationOptions.CurveEaseIn, () =>
			{
				var transform = CGAffineTransform.MakeTranslation(0f, button.Frame.Height);

				if (button.IsFlipped)
				{
					transform = CGAffineTransform.MakeScale(1.0f, 1.0f);
				}
				else
				{
					transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
				}

				button.Transform = transform;
                button.FlippedY = !button.FlippedY;
			}, () =>
			{

			});

			UIView.CommitAnimations();
		}

		/// <summary>
		/// Styles the button.
		/// </summary>
		/// <param name="button">Button.</param>
		private void StyleButton(UIButton button, string imagePath, string title)
		{
			button.SetImage(UIImage.FromFile(string.Format("{0}.png", imagePath)), UIControlState.Normal);
			button.SetTitle(title.ToUpper(), UIControlState.Normal);
			button.SetTitleColor(UIColor.Black, UIControlState.Normal);

            button.TintColor = UIColor.Black;
		}
	}
}