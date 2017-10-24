
namespace ReactiveUIAroundMe.iOS
{
	using System.Linq;
	using System.Collections.Generic;
    using System.Reactive.Disposables;
    using System;

	using CoreAnimation;
	using CoreGraphics;
	using UIKit;

    using ReactiveUI;

	using ReactiveUIAroundMe.iOS.Extras;
    using ReactiveUIAroundMe.iOS.Extensions;
	using ReactiveUIAroundMe.Portable.ViewModels;

	/// <summary>
	/// Base mvx view controller.
	/// </summary>
	public class BaseViewController : ReactiveViewController
	{
		/// <summary>
		/// The gradients.
		/// </summary>
		private IList<CAGradientLayer> gradients = new List<CAGradientLayer>();

		/// <summary>
		/// The gradient views.
		/// </summary>
		private IList<UIView> gradientViews = new List<UIView>();

		/// <summary>
		/// The view model.
		/// </summary>
		private ViewModelBase _viewModel;

		/// <summary>
		/// Gets or sets the view model.
		/// </summary>
		/// <value>The view model.</value>
		public ViewModelBase ViewModel
		{
			get { return _viewModel; }
			set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
		}

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        /// <value>The subscriptions.</value>
        public CompositeDisposable Subscriptions { get; private set; }

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = UIColor.White;

			Subscriptions = new CompositeDisposable();
		}

		/// <summary>
		/// Views the will layout subviews.
		/// </summary>
		public override void ViewWillLayoutSubviews()
		{
			base.ViewWillLayoutSubviews();

			// because we are using auto layout, gradient layers do not update width and height properties
			// we must hard set the frame of each gradient to its parent view everytime time this function is called
			for (int i = 0; i < gradients.Count; i++)
			{
				gradients[i].Frame = new CGRect(0, 0, gradientViews[i].Frame.Width, gradientViews[i].Frame.Height);
			}
		}

		/// <summary>
		/// Styles the navigation bar.
		/// </summary>
		public void StyleNavigationBar()
		{
			NavigationController.SetNavigationBarHidden(false, true);

			NavigationController.NavigationBarHidden = false;
			NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
			NavigationController.NavigationBar.TintColor = UIColor.White;

			// customise back button
			NavigationItem.SetHidesBackButton(true, false);
			NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(
				UIImage.FromFile("back.png"), UIBarButtonItemStyle.Plain, (sender, args) =>
				{
					NavigationController.PopViewController(true);
				}), true);

			var gradient = new CAGradientLayer();
			gradient.Frame = NavigationController.NavigationBar.Bounds;
			gradient.NeedsDisplayOnBoundsChange = true;
			gradient.MasksToBounds = true;

			gradient.Colors = new CGColor[]
			{
				UIColor.Clear.FromHex("c14d60").CGColor,
				UIColor.Clear.FromHex("d55b5f").CGColor
			};

			UIGraphics.BeginImageContext(gradient.Bounds.Size);
			gradient.RenderInContext(UIGraphics.GetCurrentContext());
			UIImage backImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			NavigationController.NavigationBar.SetBackgroundImage(backImage, UIBarMetrics.Default);

			UITextAttributes myTextAttrib = new UITextAttributes();
			myTextAttrib.Font = UIFont.FromName("Helvetica", 14f);
			myTextAttrib.TextColor = UIColor.FromRGB(105, 105, 105);

			UINavigationBar.Appearance.SetTitleTextAttributes(myTextAttrib);
		}

		/// <summary>
		/// Creates the view gradient.
		/// </summary>
		/// <param name="view">View.</param>
		public void CreateViewGradient(UIView view, CGColor[] colors)
		{
			var gradient = new CAGradientLayer();
			gradient.Frame = view.Bounds;
			gradient.NeedsDisplayOnBoundsChange = true;
			gradient.MasksToBounds = true;

			gradient.Colors = colors;

			gradients.Add(gradient);
			gradientViews.Add(view);

			view.Layer.AddSublayer(gradient);
		}

		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ViewModel?.OnShow(new Dictionary<string, object>());
		}

		/// <summary>
		/// Views the will disappear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			ViewModel?.OnHide();
		}

		/// <summary>
		/// Posts the load subscriptions.
		/// </summary>
		/// <returns>The load subscriptions.</returns>
		public virtual IEnumerable<IDisposable> LoadSubscriptions()
		{
			yield break;
		}

		/// <summary>
		/// Load this instance.
		/// </summary>
		public virtual void Load()
		{
			foreach (var item in this.LoadSubscriptions())
			{
				this.Subscriptions.Add(item);
			}
		}

		/// <summary>
		/// Disposes the page.
		/// </summary>
		public virtual void DisposePage()
		{
			Subscriptions.Dispose();
			Subscriptions = new CompositeDisposable();
		}
	}
}