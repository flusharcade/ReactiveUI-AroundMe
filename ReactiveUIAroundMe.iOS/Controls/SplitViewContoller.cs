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
	/// Split view controller.
	/// </summary>
	public class SplitViewController : ReactiveSplitViewController
	{
		/// <summary>
		/// The master view.
		/// </summary>
		private UIViewController _masterView;

		/// <summary>
		/// The detail view.
		/// </summary>
		private UIViewController _detailView;

		/// <summary>
		/// Splits the view contoller.
		/// </summary>
		/// <param name="masterViewController">Master view controller.</param>
		/// <param name="detailViewController">Detail view controller.</param>
		public SplitViewController(UIViewController masterViewController,
		                          UIViewController detailViewController) 
			: base()
		{
			// create our master and detail views
			_masterView = masterViewController;
			_detailView = detailViewController;
			// create an array of controllers from them and then
			// assign it to the controllers property
			ViewControllers = new UIViewController[]
			{ 
				_masterView, 
				_detailView 
			}; // order is important
		}

		/// <summary>
		/// Shoulds the autorotate to interface orientation.
		/// </summary>
		/// <returns><c>true</c>, if autorotate to interface orientation was shoulded, <c>false</c> otherwise.</returns>
		/// <param name="toInterfaceOrientation">To interface orientation.</param>
		public override bool ShouldAutorotateToInterfaceOrientation
			(UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}
