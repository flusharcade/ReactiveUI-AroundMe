
namespace ReactiveUIAroundMe.iOS.Controls
{
	using System.Linq;
	using System.Collections.Generic;
	using System;

	using CoreAnimation;
	using CoreGraphics;
	using ReactiveUI;
	using UIKit;

	using ReactiveUIAroundMe.iOS.Extras;
	using ReactiveUIAroundMe.iOS.Tab;

	using ReactiveUIAroundMe.Portable.ViewModels;

	/// <summary>
	/// Navigation mvx view controller.
	/// </summary>
	public class BaseMvxTabBarViewController : ReactiveTabBarController, IViewFor<ViewModelBase>
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
		/// The created so far count.
		/// </summary>
		protected int CreatedSoFarCount = 0;

		/// <summary>
		/// The time remaining title label.
		/// </summary>
		private UILabel _timeRemainingTitleLabel;

		/// <summary>
		/// The time remaining label.
		/// </summary>
		private UILabel _timeRemainingLabel;

		/// <summary>
		/// The time.
		/// </summary>
		private UILabel _titleLabel;

		/// <summary>
		/// The view model.
		/// </summary>
		ViewModelBase _viewModel;

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
		/// Gets or sets the reactive user interface . IV iew for. view model.
		/// </summary>
		/// <value>The reactive user interface . IV iew for. view model.</value>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (ViewModelBase)value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.BaseMvxTabBarViewController"/> class.
		/// </summary>
		public BaseMvxTabBarViewController()
		{
			//Mvx.Resolve<ITabBarPresenterHost>().TabBarPresenter = this;
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var titleView = new UIView(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 44));
			this.NavigationItem.TitleView = titleView;

			var subTitleLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Center,
				Text = "Match Day Management",
				Font = UIFont.FromName("Helvetica", 12f),
				TextColor = UIColor.FromRGB(105, 105, 105),
			};

			var titleLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.FromName("Alfa Slab One", 20f),
				TextColor = UIColor.FromRGB(60, 60, 59),
			};

			_timeRemainingTitleLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Center,
				Text = "Time Remaining",
				Font = UIFont.FromName("Helvetica", 12f),
				TextColor = UIColor.FromRGB(105, 105, 105),
			};

			_timeRemainingLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.FromName("Alfa Slab One", 20f),
				TextColor = UIColor.FromRGB(60, 60, 59),
			};

			var imageView = new UIImageView(UIImage.FromFile("afl_logo.png"))
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				ContentMode = UIViewContentMode.ScaleAspectFit,
				UserInteractionEnabled = true
			};

			UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(() =>
			{
				NavigationController.PopViewController(true);
			});

			imageView.AddGestureRecognizer(tapGesture);

			var views = new DictionaryViews()
			{
				{"subTitleLabel", subTitleLabel},
				{"titleLabel", titleLabel},
				{"imageView", imageView},
				{"timeRemainingTitleLabel", _timeRemainingTitleLabel},
				{"timeRemainingLabel", _timeRemainingLabel},
			};

			titleView.Add(subTitleLabel);
			titleView.Add(_titleLabel);
			titleView.Add(imageView);
			titleView.Add(_timeRemainingTitleLabel);
			titleView.Add(_timeRemainingLabel);

			titleView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|-5-[subTitleLabel(15)][titleLabel(18)]", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-5-[timeRemainingTitleLabel(15)][timeRemainingLabel(18)]", NSLayoutFormatOptions.DirectionLeftToRight, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|[imageView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:[subTitleLabel]", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:[titleLabel]", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[imageView(40)]", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:[timeRemainingTitleLabel]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:[timeRemainingLabel]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(new[] { NSLayoutConstraint.Create(subTitleLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, titleView, NSLayoutAttribute.CenterX, 1f, 0) })
				.Concat(new[] { NSLayoutConstraint.Create(titleLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, titleView, NSLayoutAttribute.CenterX, 1f, 0) })
				.ToArray());
			
			this.WhenActivated(d =>
			{
				// We need to bind the ViewModel property to the DataContext in order to be able to
				// use WPF Bindings. Let's use WPF bindings for the UserName property.
				this.Bind(ViewModel, vm => vm.Title, v => v._titleLabel.Text);
				//OnAppear(new Dictionary<string, object>());
			});

			NavigationItem.SetHidesBackButton(true, false);

			UITextAttributes myTextAttrib = new UITextAttributes();
			myTextAttrib.Font = UIFont.FromName("Alfa Slab One", 20);
			myTextAttrib.TextColor = UIColor.FromRGB(105, 105, 105);

			UINavigationBar.Appearance.SetTitleTextAttributes(myTextAttrib);
		}


		/// <summary>
		/// Creates the tab for.
		/// </summary>
		/// <returns>The tab for.</returns>
		/// <param name="title">Title.</param>
		/// <param name="imageName">Image name.</param>
		/// <param name="viewModel">View model.</param>
		protected UIViewController CreateTabFor(string title, string imageName, ViewModelBase viewModel)
		{
			//var tab = this.CreateViewControllerFor(viewModel) as ReactiveViewController;
			//SetTitleAndTabBarItem(tab, title, imageName);
			//return tab;
			return null;
		}

		/// <summary>
		/// Sets the title and tab bar item.
		/// </summary>
		/// <param name="screen">Screen.</param>
		/// <param name="title">Title.</param>
		/// <param name="imageName">Image name.</param>
		protected void SetTitleAndTabBarItem(UIViewController screen, string title, string imageName)
		{
			//screen.Title = title;
			screen.TabBarItem = new UITabBarItem(title, UIImage.FromBundle(imageName + ".png"), CreatedSoFarCount);
			CreatedSoFarCount++;
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
				gradients[i].Frame = new CGRect(0, 0, gradientViews[i].Frame.Width,gradientViews[i].Frame.Height);
			}
		}

		/// <summary>
		/// Creates the view gradient.
		/// </summary>
		/// <param name="view">View.</param>
		public void CreateViewGradient(UIView view)
		{
			var gradient = new CAGradientLayer();
			gradient.Frame = view.Bounds;
			gradient.NeedsDisplayOnBoundsChange = true;
			gradient.MasksToBounds = true;

			View.BackgroundColor = UIColor.Black;

			gradient.Colors = new CGColor[]
			{
				UIColor.FromRGB(253, 253, 253).CGColor,
				UIColor.FromRGB(227, 227, 228).CGColor
			};

			gradients.Add(gradient);
			gradientViews.Add(view);

			view.Layer.AddSublayer(gradient);
		}

		/// <summary>
		/// Hides the time from nav.
		/// </summary>
		protected void HideTimeFromNav()
		{
			_timeRemainingLabel.Hidden = true;
			_timeRemainingTitleLabel.Hidden = true;
		}

		/// <summary>
		/// Styles the tab bar.
		/// </summary>
		protected void StyleTabBar()
		{
			TabBar.TintColor = UIColor.FromRGB(180, 6, 16);
			TabBar.BarTintColor = UIColor.FromRGB(60, 60, 59);
			TabBar.ItemSpacing = 100;

			var gradient = new CAGradientLayer();
			gradient.Frame = TabBar.Bounds;
			gradient.NeedsDisplayOnBoundsChange = true;
			gradient.MasksToBounds = true;

			gradient.Colors = new CGColor[] {
				UIColor.FromRGB(253, 253, 253).CGColor,
				UIColor.FromRGB(227, 227, 228).CGColor
			};

			TabBar.Layer.AddSublayer(gradient);
		}

		/// <summary>
		/// Only allow iPad application to rotate, iPhone is always portrait
		/// </summary>
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
				return true;
			else
				return toInterfaceOrientation == UIInterfaceOrientation.Portrait;
		}

		/// <summary>
		/// Gos the back.
		/// </summary>
		/// <returns><c>true</c>, if back was gone, <c>false</c> otherwise.</returns>
		public bool GoBack(bool animated)
		{
			var subNavigation = this.ParentViewController as UINavigationController;
			if (subNavigation == null)
				return false;

			if (subNavigation.ViewControllers.Length <= 1)
				return false;

			subNavigation.PopViewController(animated);
			return true;
		}

		/// <summary>
		/// Shows the view.
		/// </summary>
		/// <returns><c>true</c>, if view was shown, <c>false</c> otherwise.</returns>
		/// <param name="view">View.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public bool ShowView(UIView view, bool animated)
		{
			//if (TryShowViewInCurrentTab(view, animated))
			//	return true;

			return false;
		}

		/// <summary>
		/// Tries the show view in current tab.
		/// </summary>
		/// <returns><c>true</c>, if show view in current tab was tryed, <c>false</c> otherwise.</returns>
		/// <param name="view">View.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		private bool TryShowViewInCurrentTab(ReactiveViewController view, bool animated)
		{
			// are we navigating to a tab?
			var mvxViewController = ViewControllers.OfType<BaseViewController>()
						   .FirstOrDefault(x => x.GetType() == view.GetType());

			if (mvxViewController != null)
			{
				SelectedViewController = mvxViewController;
			}
			else
			{
				var navigationController = (UINavigationController)this.ParentViewController;
				navigationController.PushViewController(view, animated);
			}

			return true;
		}

		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			ViewModel.OnShow(new Dictionary<string, object>());
		}

		/// <summary>
		/// Views the will disappear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			ViewModel?.OnHide();

			//Mvx.Resolve<ITabBarPresenterHost>().TabBarPresenter = null;
		}
	}
}