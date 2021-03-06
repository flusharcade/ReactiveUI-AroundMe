﻿// --------------------------------------------------------------------------------------------------
//  <copyright file="SearchResultsPage.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Views
{
	using System.Linq;
	using System.Reactive.Linq;
	using System.Collections.Generic;
	using System;
	using System.Reactive.Disposables;

	using UIKit;

	using ReactiveUI;

	using ReactiveUIAroundMe.iOS.Extras;
	using ReactiveUIAroundMe.iOS.Controls;
	using ReactiveUIAroundMe.iOS.UI;

	using ReactiveUIAroundMe.Portable.ViewModels;

	/// <summary>
	/// Search results page.
	/// </summary>
	public class SearchResultsPage : BaseViewController, IViewFor<SearchResultsPageViewModel>
	{
		/// <summary>
		/// The player list.
		/// </summary>
		private UITableView _searchResultsTableView;

		/// <summary>
		/// The progress view.
		/// </summary>
		private UIActivityIndicatorView _progressView;

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
				return _searchResultsTableView?.Source as 
                    ReactiveTableViewSource<EReactiveUIAroundMeListItemViewModel>;;
			}
		}

		/// <summary>
		/// Gets or sets the view model.
		/// </summary>
		/// <value>The view model.</value>
		public new SearchResultsPageViewModel ViewModel
		{
			get { return (SearchResultsPageViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		/// <summary>
		/// Gets or sets the reactive user interface . IV iew for. view model.
		/// </summary>
		/// <value>The reactive user interface . IV iew for. view model.</value>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (SearchResultsPageViewModel)value; }
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_serialDisposable = new SerialDisposable();

			this.StyleNavigationBar();

			Title = "Results";

			_searchResultsTableView = new UITableView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};

			_progressView = new UIActivityIndicatorView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Color = UIColor.White,
			};

			Add(_searchResultsTableView);
			Add(_progressView);

			var views = new DictionaryViews()
			{
				{"searchResultsTableView", _searchResultsTableView},
                {"progressView", _progressView},
			};

			View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[searchResultsTableView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
                .Concat(NSLayoutConstraint.FromVisualFormat("V:|[progressView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
                .Concat(NSLayoutConstraint.FromVisualFormat("H:|[searchResultsTableView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
                .Concat(NSLayoutConstraint.FromVisualFormat("H:|[progressView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.ToArray());

			this.Bind(ViewModel, x => x.IsLoading, x => x._searchResultsTableView.Hidden);

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
			yield return ViewModel.WhenAnyValue(vm => vm.Results).BindTo<EReactiveUIAroundMeListItemViewModel,
					EReactiveUIAroundMeListItemTableViewCell>(_searchResultsTableView, 70, cell => cell.Initialize());

			yield return this.WhenAnyValue(v => v._tableSource.ElementSelected)
				.Subscribe(x =>
				{
					// when ever this value is updated we want to make sure we only ever have
					// the latest subscription
					_serialDisposable.Disposable = x.Subscribe(vm => ViewModel.SelectAsync(vm as EReactiveUIAroundMeListItemViewModel));
				});

			yield return ViewModel.WhenAnyValue(vm => vm.IsLoading)
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
		}

		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear(bool animated)
		{
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
	}
}