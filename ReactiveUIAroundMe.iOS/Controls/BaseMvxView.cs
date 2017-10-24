
namespace ReactiveUIAroundMe.iOS
{
	using System.Linq;
	using System.Collections.Generic;

	using CoreAnimation;
	using CoreGraphics;
	using UIKit;

	using ReactiveUI;

	using ReactiveUIAroundMe.iOS.Extras;
	using ReactiveUIAroundMe.Portable.ViewModels;

	/// <summary>
	/// Base mvx view controller.
	/// </summary>
	public class BaseMvxView : ReactiveView
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
		/// Views the will layout subviews.
		/// </summary>
		public virtual void ViewWillLayoutSubviews()
		{
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

			gradient.Colors = new CGColor[]
			{
				UIColor.FromRGB(253, 253, 253).CGColor,
				UIColor.FromRGB(227, 227, 228).CGColor
			};

			gradients.Add(gradient);
			gradientViews.Add(view);

			Layer.AddSublayer(gradient);
		}

		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public virtual void ViewWillAppear(bool animated)
		{
			//(DataContext as ViewModelBase)?.OnShow(new Dictionary<string, object>());
		}

		/// <summary>
		/// Views the will disappear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public virtual void ViewWillDisappear(bool animated)
		{
			//(DataContext as ViewModelBase)?.OnHide();
		}
	}
}