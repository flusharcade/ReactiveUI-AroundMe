
namespace ReactiveUIAroundMe.Droid.Controls
{
	using System.Collections.Generic;
	using System.Reactive.Disposables;
	using System;

	using ReactiveUI;

	using ReactiveUIAroundMe.Portable.ViewModels;

	/// <summary>
	/// Base mvx view controller.
	/// </summary>
	public class BaseActivity : ReactiveActivity
	{
		///// <summary>
		///// The gradients.
		///// </summary>
		//private IList<CAGradientLayer> gradients = new List<CAGradientLayer>();

		///// <summary>
		///// The gradient views.
		///// </summary>
		//private IList<UIView> gradientViews = new List<UIView>();

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
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
			
			Subscriptions = new CompositeDisposable();
		}

		/// <summary>
		/// Views the will layout subviews.
		/// </summary>
		//public override void ViewWillLayoutSubviews()
		//{
		//	base.ViewWillLayoutSubviews();

		//	// because we are using auto layout, gradient layers do not update width and height properties
		//	// we must hard set the frame of each gradient to its parent view everytime time this function is called
		//	for (int i = 0; i < gradients.Count; i++)
		//	{
		//		gradients[i].Frame = new CGRect(0, 0, gradientViews[i].Frame.Width, gradientViews[i].Frame.Height);
		//	}
		//}

		/// <summary>
		/// Creates the view gradient.
		/// </summary>
		/// <param name="view">View.</param>
		//public void CreateViewGradient(UIView view, CGColor[] colors)
		//{
		//	var gradient = new CAGradientLayer();
		//	gradient.Frame = view.Bounds;
		//	gradient.NeedsDisplayOnBoundsChange = true;
		//	gradient.MasksToBounds = true;

		//	gradient.Colors = colors;

		//	gradients.Add(gradient);
		//	gradientViews.Add(view);

		//	view.Layer.AddSublayer(gradient);
		//}

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