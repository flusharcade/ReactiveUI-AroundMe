// --------------------------------------------------------------------------------------------------
//  <copyright file="HomePage.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Droid.Views
{
	using System.Linq;
	using System.Reactive.Linq;
	using System.Collections.Generic;
	using System;

	using Android.Widget;

	using ReactiveUI;

	using ReactiveUIAroundMe.Droid.Controls;

	using ReactiveUIAroundMe.Portable.ViewModels;
	using ReactiveUIAroundMe.Portable.Location;

    /// <summary>
    /// Home page.
    /// </summary>
    public class HomeActivity : BaseActivity, IViewFor<HomePageViewModel>
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

		///// <summary>
		///// The location view.
		///// </summary>
		//private LocationView _locationView;

		///// <summary>
		///// The progress view.
		///// </summary>
		//private UIActivityIndicatorView _progressView;

		/// <summary>
		/// The buttons.
		/// </summary>
		private List<FlipButton> _buttons;

		/// <summary>
		/// The view model.
		/// </summary>
		private HomePageViewModel _viewModel;

		/// <summary>
		/// Gets or sets the view model.
		/// </summary>
		/// <value>The view model.</value>
		public new HomePageViewModel ViewModel
		{
			get { return _viewModel; }
			set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
		}

		/// <summary>
		/// Gets or sets the reactive user interface . IV iew for. view model.
		/// </summary>
		/// <value>The reactive user interface . IV iew for. view model.</value>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (HomePageViewModel)value; }
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.HomeLayout);

			_buttons = new List<FlipButton>();

            Title = "Home";
			
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
			yield return Observable.FromEventPattern<Location>(ViewModel, "LocationUpdate")
				.Window(() => Observable.Interval(TimeSpan.FromSeconds(2)))
				.SelectMany(x => x.Take(1))
				.Subscribe(e =>
				{
					var location = e.EventArgs;

					//var x = (location.Longitude + 180) / 360 * _locationView.Frame.Width;
					//var y = ((1 - Math.Log(Math.Tan(location.Latitude * Math.PI / 180) + 1 / Math.Cos(location.Latitude * Math.PI / 180)) / Math.PI) / 2 * Math.Pow(2, 0))
						//* _locationView.Frame.Height;

					//_locationView.FocusLocation(new CoreGraphics.CGPoint(x, y));
				});

			yield return ViewModel.WhenAnyValue(vm => vm.LocationLoading)
				.SubscribeOn(ViewModel.Scheduler)
				.Subscribe(loading =>
				{
					if (loading)
					{
						//_progressView.StartAnimating();
					}
					else
					{
						//_progressView.StopAnimating();
					}
				});

			yield return Observable.FromEventPattern(ViewModel, "Flip")
							.Subscribe(e => FlipButtons());
		}

		///// <summary>
		///// Views the will appear.
		///// </summary>
		///// <param name="animated">If set to <c>true</c> animated.</param>
		//public override void ViewWillAppear(bool animated)
		//{
		//	// hide nav bar everytime this page shows
		//	NavigationController.SetNavigationBarHidden(true, false);

		//	ViewModel?.UpdateLocation();

		//	base.Load();

		//	base.ViewWillAppear(animated);
		//}

		///// <summary>
		///// Views the will disappear.
		///// </summary>
		///// <param name="animated">If set to <c>true</c> animated.</param>
		//public override void ViewWillDisappear(bool animated)
		//{
		//	base.DisposePage();

		//	base.ViewWillDisappear(animated);
		//}

		/// <summary>
		/// Flips the buttons.
		/// </summary>
		private void FlipButtons()
		{
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
							break;
						case 1:
							FlipY(button);
							break;
					}
				}

				index++;
			}
		}

		/// <summary>
        /// Flips the x.
        /// </summary>
        /// <param name="button">Button.</param>
		private void FlipX(ImageButton button)
		{
			//UIView.BeginAnimations(button.ToString());

			//UIView.Animate(1, 1, UIViewAnimationOptions.CurveEaseIn, () =>
			//{
			//	var transform = CGAffineTransform.MakeTranslation(0f, button.Frame.Width);

			//	if (button.IsFlipped)
			//	{
			//		transform = CGAffineTransform.MakeScale(1.0f, 1.0f);
			//	}
			//	else
			//	{
			//		transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
			//	}

			//	button.Transform = transform;

			//	button.IsFlipped = !button.IsFlipped;
			//}, () =>
			//{

			//});

			//UIView.CommitAnimations();
		}

		/// <summary>
        /// Flips the y.
        /// </summary>
        /// <param name="button">Button.</param>
		private void FlipY(ImageButton button)
		{
			//UIView.BeginAnimations(button.ToString());

			//UIView.Animate(1, 1, UIViewAnimationOptions.CurveEaseIn, () =>
			//{
			//	var transform = CGAffineTransform.MakeTranslation(0f, button.Frame.Height);

			//	if (button.IsFlipped)
			//	{
			//		transform = CGAffineTransform.MakeScale(1.0f, 1.0f);
			//	}
			//	else
			//	{
			//		transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
			//	}

			//	button.Transform = transform;

			//	button.IsFlipped = !button.IsFlipped;
			//}, () =>
			//{

			//});

			//UIView.CommitAnimations();
		}

		/// <summary>
		/// Styles the button.
		/// </summary>
		/// <param name="button">Button.</param>
		private void StyleButton(ImageButton button, string imagePath, string title)
		{
			//button.SetImage(UIImage.FromFile(string.Format("{0}.png", imagePath)), UIControlState.Normal);
			//button.SetTitle(title.ToUpper(), UIControlState.Normal);
			//button.SetTitleColor(UIColor.White, UIControlState.Normal);

			//button.TintColor = UIColor.White;
		}
	}
}