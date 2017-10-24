// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WrapLayoutPageViewModelBase.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using ReactiveUIAroundMe.Portable.Location;

namespace ReactiveUIAroundMe.Portable.ViewModels
{
	using System.Collections.ObjectModel;
	using System.Reactive.Subjects;
	using System.Reactive.Concurrency;
	using System.Threading.Tasks;
	using System.Collections.Generic;

	using ReactiveUI;

	using ReactiveUIAroundMe.Portable.UI;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Common;
	
	using ReactiveUIAroundMe.Portable.Logging;
	using System;
	using WebServices;

	/// <summary>
	/// Tetrix grid page view model base.
	/// </summary>
	public abstract class TetrixPageViewModelBase : ViewModelBase
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.TetrixPageViewModelBase"/> class.
		/// </summary>
		/// <param name="storage">Storage.</param>
		/// <param name="scheduler">Scheduler.</param>
		/// <param name="signalRClient">Signal RC lient.</param>
		/// <param name="log">Log.</param>
		/// <param name="applicationStateHandler">Application state handler.</param>
		protected TetrixPageViewModelBase(ISQLiteStorage storage, IScheduler scheduler, ILogger log,
							 ApplicationStateHandler applicationStateHandler, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
		                     IPathLocator pathLocator, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, googleMapsWebServiceController,
			       pathLocator, hostScreen, locationManager)
		{
			Cells = new ObservableRangeCollection<TetrixViewModelBase>();

			DataChanges = new Subject<DataChange>();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the cells.
		/// </summary>
		/// <value>The cells.</value>
		public virtual ObservableRangeCollection<TetrixViewModelBase> Cells { get; set; }

		/// <summary>
		/// Gets the data changes.
		/// </summary>
		/// <value>The data changes.</value>
		public Subject<DataChange> DataChanges { get; private set; }

		/// <summary>
		/// The cells selected.
		/// </summary>
		private int _cellsSelected;

		/// <summary>
		/// Gets or sets the cells selected.
		/// </summary>
		/// <value>The cells selected.</value>
		public int CellsSelected
		{
			get { return _cellsSelected; }
			set { this.RaiseAndSetIfChanged(ref _cellsSelected, value); }
		}

		/// <summary>
		/// The max cells selectable.
		/// </summary>
		private int _maxCellsSelectable = 4;

		/// <summary>
		/// Gets or sets the max cells selectable.
		/// </summary>
		/// <value>The max cells selectable.</value>
		public int MaxCellsSelectable
		{
			get { return _maxCellsSelectable; }
			set { this.RaiseAndSetIfChanged(ref _maxCellsSelectable, value); }
		}

		/// <summary>
		/// The selected.
		/// </summary>
		private TetrixViewModelBase _selectedItem;

		/// <summary>
		/// Gets or sets the selected.
		/// </summary>
		/// <value>The selected.</value>
		public virtual TetrixViewModelBase SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				this.RaiseAndSetIfChanged(ref _selectedItem, value);

				//value?.SelectCommand?.Execute(value?.SelectParameters);

				// after we execute, reset selected
				_selectedItem = null;
			}
		}

		/// <summary>
		/// The selected.
		/// </summary>
		private ReactiveCommand _selectCommand;

		/// <summary>
		/// Gets or sets the selected.
		/// </summary>
		/// <value>The selected.</value>
		public virtual ReactiveCommand SelectCommand => _selectCommand;

		#endregion

		#region Private Properties

		/// <summary>
		/// The cells empty.
		/// </summary>
		private bool _cellsEmpty;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether this
		/// <see cref="T:TelstraHealth.Portable.TetrixGrid.TetrixPageViewModelBase"/> cells empty.
		/// </summary>
		/// <value><c>true</c> if cells empty; otherwise, <c>false</c>.</value>
		public virtual bool CellsEmpty
		{
			get { return _cellsEmpty; }
			set { this.RaiseAndSetIfChanged(ref _cellsEmpty, value); }
		}

		#endregion

		/// <summary>
		/// Raises the hide event.
		/// </summary>
		/// <param name="disposeCells">If set to <c>true</c> dispose cells.</param>
		public void OnHide(bool disposeCells = true)
		{
			if (disposeCells)
			{
				Cells.Clear();
				CellsEmpty = true;
			}
		}

		/// <summary>
		/// Recycle the specified newItem, cells and position.
		/// </summary>
		/// <param name="newItem">New item.</param>
		/// <param name="cells">Cells.</param>
		/// <param name="position">Position.</param>
		public void Recycle(TetrixViewModelBase newItem, IList<TetrixViewModelBase> cells,
							int position, bool recalculatePositions = true)
		{
			// TODO: linear search yuck, lets add a binary search here for fast insertion
			bool found = false;

			newItem.Position = position;

			foreach (var cell in cells)
			{
				if (cell.CellId.Equals(newItem.CellId))
				{
					cell.Apply(newItem);
					found = true;
					break;
				}
			}

			if (!found)
			{
				// reset event handler for cell selected
				//newItem.Selected -= CellSelected;
				//newItem.Selected += CellSelected;

				if (position <= cells.Count - 1 && position >= 0)
				{
					cells.Insert(position, newItem);
				}
				else
				{
					cells.Add(newItem);
				}
			}

			if (recalculatePositions)
			{
				RecalculatePositions(cells);
			}
		}

		/// <summary>
		/// Recalculates the positions.
		/// </summary>
		public void RecalculatePositions(IList<TetrixViewModelBase> cells)
		{
			// after insertion, recount positions
			var index = 0;

			foreach (var cell in cells)
			{
				cell.Position = index;
				index++;
			}
		}

		/// <summary>
		/// Notifies the data change.
		/// </summary>
		public void NotifyDataChange()
		{
			DataChanges.OnNext(new DataChange()
			{
				SizeChanged = false
			});
		}

		/// <summary>
		/// Cells the selected.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">If set to <c>true</c> e.</param>
		private void CellSelected(object sender, bool e)
		{
			SelectedItem = sender as TetrixViewModelBase;

			CellsSelected += e ?
				// if we have four selected add no more
				CellsSelected == MaxCellsSelectable ? 0 : 1
				// if more than 0 selected, we can deduct
				: CellsSelected > 0 ? -1 : 0;

			// disable all cells selections if we have selected 4
			SetSelectable(CellsSelected < MaxCellsSelectable);
		}

		/// <summary>
		/// Sets the selectable.
		/// </summary>
		/// <param name="allowSelection">If set to <c>true</c> allow selection.</param>
		private void SetSelectable(bool allowSelection)
		{
			foreach (var cell in Cells)
			{
				// if cell is selected it is always selectable
				cell.IsSelectable = cell.IsSelected ? true : allowSelection;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		public void InitSelectCommand(Action<object> action)
		{
			_selectCommand = ReactiveCommand.Create<object>(action);
		}
	}
}