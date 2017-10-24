// --------------------------------------------------------------------------------------------------
//  <copyright file="FlyoutMenuActivity.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Droid.Views
{
	using System.Linq;
	using System.Reactive.Linq;
	using System.Collections.Generic;
	using System;
	using System.Reactive.Disposables;

	using ReactiveUI;

	using ReactiveUIAroundMe.Droid.Controls;

	using ReactiveUIAroundMe.Portable.ViewModels;

    /// <summary>
    /// Search results page.
    /// </summary>
    public class FlyoutMenuActivity : BaseActivity, IViewFor<FlyoutMenuPageViewModel>
	{
		/*/// <summary>
		/// The player list.
		/// </summary>
		private UITableView _searchResultsTableView;

		/// <summary>
		/// Gets the subscriptions.
		/// </summary>
		/// <value>The subscriptions.</value>
		private SerialDisposable _serialDisposable;

		/// <summary>
		/// The source.
		/// </summary>
		private ReactiveTableViewSource<EReactiveUIAroundMeListItemViewModel> _tableSource
		{
			get
			{
				return _searchResultsTableView?.Source as ReactiveTableViewSource<EReactiveUIAroundMeListItemViewModel>;;
			}
		}*/

		/// <summary>
		/// The view model.
		/// </summary>
		private FlyoutMenuPageViewModel _viewModel;

		/// <summary>
		/// Gets or sets the view model.
		/// </summary>
		/// <value>The view model.</value>
		public new FlyoutMenuPageViewModel ViewModel
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
			set { ViewModel = (FlyoutMenuPageViewModel)value; }
		}

		/*/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_serialDisposable = new SerialDisposable();

			//this.StyleNavigationBar();

			Title = "Menu";

			_searchResultsTableView = new UITableView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			Add(_searchResultsTableView);

			var views = new DictionaryViews()
			{
				{"searchResultsTableView", _searchResultsTableView},
			};

			View.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|[searchResultsTableView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[searchResultsTableView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.ToArray());
			
			this.WhenActivated(d =>
			{
				ViewModel.WhenAnyValue(vm => vm.Results).BindTo<EReactiveUIAroundMeListItemViewModel,
					EReactiveUIAroundMeListItemTableViewCell>(_searchResultsTableView, 80, cell => cell.Initialize());

				this.WhenAnyValue(v => v._tableSource.ElementSelected)
				    .Subscribe(x =>
					{
						// when ever this value is updated we want to make sure we only ever have
						// the latest subscription
						_serialDisposable.Disposable = x.Subscribe(vm => ViewModel.SelectAsync());
					});
				                  
				// HACK: Clean this up with proper navigation
				ViewModel.OnShow(new Dictionary<string, object>());
			});
		}*/
	}
}