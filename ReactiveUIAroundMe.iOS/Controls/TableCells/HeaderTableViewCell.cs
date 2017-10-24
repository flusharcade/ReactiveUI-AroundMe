// --------------------------------------------------------------------------------------------------
//  <copyright file="HeaderTableViewCell.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Controls
{
	using System;
	using System.Linq;
	using System.Reactive.Linq;

	using Foundation;
	using UIKit;
	using CoreGraphics;

	using ReactiveUI;

	using ReactiveUIAroundMe.iOS.Extras;
	using ReactiveUIAroundMe.iOS.Extensions;

	using ReactiveUIAroundMe.Portable.ViewModels;

	/// <summary>
	/// EReactiveUIAroundMe list item table view cell.
	/// </summary>
	public partial class HeaderTableViewCell : BaseTableViewCell, IViewFor<HeaderListItemViewModel>
	{
		/// <summary>
		/// The key.
		/// </summary>
		public static readonly NSString Key = new NSString("HeaderTableViewCell");

		/// <summary>
		/// The title label.
		/// </summary>
        private UILabel _titleLabel;

		/// <summary>
		/// The left view.
		/// </summary>
		private UIView _leftView;

		/// <summary>
		/// The view model.
		/// </summary>
		private HeaderListItemViewModel _viewModel;

		/// <summary>
		/// Gets or sets the view model.
		/// </summary>
		/// <value>The view model.</value>
		public HeaderListItemViewModel ViewModel
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
			set { ViewModel = value as HeaderListItemViewModel; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MatchDayManagement.iOS.InjurySearchResultTableViewCell"/> class.
		/// </summary>
		/// <param name="handle">Handle.</param>
		protected HeaderTableViewCell(IntPtr handle) : base(handle)
		{
			_titleLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Font = UIFont.FromName("Helvetica", 14f),
				TextColor = UIColor.Clear.FromHex("#444444"),
			};

			_leftView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = UIColor.Clear.FromHex("#ca505d")
			};

			var views = new DictionaryViews()
			{
				{"titleLabel", _titleLabel},
			};

            ContentView.BackgroundColor = UIColor.Clear.FromHex("#444444");

			ContentView.Add(_titleLabel);

			ContentView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|-5-[titleLabel]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
                .Concat(NSLayoutConstraint.FromVisualFormat("H:|-18-[titleLabel]-15-|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.ToArray());

			// selected view
			SelectedBackgroundView = new UIView(ContentView.Bounds)
			{
				BackgroundColor = UIColor.Clear.FromHex("#fefafa"),
			};

			SelectedBackgroundView.Add(_leftView);

			var selectedViews = new DictionaryViews()
			{
				{"leftView", _leftView}
			};

			SelectedBackgroundView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|[leftView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, selectedViews)
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[leftView(3)]", NSLayoutFormatOptions.AlignAllTop, null, selectedViews))
				.ToArray());
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public override void Initialize()
		{
			this.WhenAnyValue(v => v.ViewModel.Title).BindTo(this,
				v => v._titleLabel.Text);
		}
	}
}
