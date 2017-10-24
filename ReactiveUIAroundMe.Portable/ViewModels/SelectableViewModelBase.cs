// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectableBase.cs" company="Champion Data Pty Ltd.">
//   Copyright (c) 2015 Champion Data Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using ReactiveUIAroundMe.Portable.Location;

namespace ReactiveUIAroundMe.Portable.ViewModels
{
	using System;
	using System.Reactive.Concurrency;
	using System.Reactive;
	using System.Reactive.Linq;
	using System.Reactive.Disposables;

	using ReactiveUI;

	
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Logging;
	using UI;
	using WebServices;

	/// <summary>
	/// </summary>
	public abstract class SelectableViewModelBase : ViewModelBase
	{
		#region Constructors and Destructors

		// here we simulate logins by randomly passing/failing
		private IObservable<Unit> _selectAsync;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.ViewModels.TetrixViewModelBase"/> class.
		/// </summary>
		protected SelectableViewModelBase(ISQLiteStorage storage, IScheduler scheduler, ILogger log,
							 ApplicationStateHandler applicationStateHandler, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
		                     IPathLocator pathLocator, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, googleMapsWebServiceController, pathLocator, hostScreen, locationManager)
		{
			// todo:  working on this logic
			_selectAsync = Observable.Create((IObserver<bool> observer) =>
				   {
					   IsSelected = !IsSelected;
					   return Disposable.Empty;
				   }).Select(x => Unit.Default);

			var canExecute = this.WhenAnyValue(x => x.IsSelectable,
				(selectable) => !selectable);

			_selectCommand = ReactiveCommand.CreateFromObservable(() => _selectAsync, canExecute);
		}

		#endregion

		#region Private Properties

		/// <summary>
		/// The object lock.
		/// </summary>
		object objectLock = new Object();

		#endregion

		#region Public Properties


		/// <summary>
		/// Occurs when selected.
		/// </summary>
		public virtual event EventHandler<bool> Selected;

		/// <summary>
		/// Occurs when cell selected.
		/// </summary>
		public event EventHandler<bool> CellSelected
		{
			add
			{
				lock (objectLock)
				{
					Selected += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					Selected -= value;
				}
			}
		}

		/// <summary>
		/// The select command.
		/// </summary>
		private ReactiveCommand<Unit, Unit> _selectCommand;

		/// <summary>
		/// 
		/// </summary>
		public ReactiveCommand<Unit, Unit> SelectCommand => _selectCommand;

		/// <summary>
		/// Inits the selection action.
		/// </summary>
		public void InitSelectionAction(IObservable<Unit> select)
		{
			_selectAsync = select;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="select"></param>
		public void InitSelectionCommand(Action<object> select)
		{
			//_selectCommand = ReactiveCommand.Create<object>(select);
		}

		#endregion
	}
}