
namespace ReactiveUIAroundMe.iOS
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using CoreGraphics;
	using Foundation;
	using ReactiveUI;
	using UIKit;

	/// <summary>
	/// Tetrix collection source.
	/// </summary>
	public class TetrixCollectionSource : ReactiveCollectionViewSource<BaseCollectionViewCell>
	{
		/// <summary>
		/// The create cell.
		/// </summary>
		private Func<object, UICollectionView, NSIndexPath, BaseCollectionViewCell> _createCell;

		/// <summary>
		/// The cell.
		/// </summary>
		private List<object> Cells;

		/// <summary>
		/// The selected cells.
		/// </summary>
		private int _selectedCells;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.CollectionSource"/> class.
		/// </summary>
		/// <param name="collectionView">Collection view.</param>
		/// <param name="defaultCellIdentifier">Default cell identifier.</param>
		public TetrixCollectionSource(UICollectionView collectionView, NSString defaultCellIdentifier, 
		                              Func<object, UICollectionView, NSIndexPath, BaseCollectionViewCell> createCell)
            : base(collectionView)
        {
			_createCell = createCell;

			Cells = new List<object>();
		}

		/// <summary>
		/// Gets the or create cell for.
		/// </summary>
		/// <returns>The or create cell for.</returns>
		/// <param name="collectionView">Collection view.</param>
		/// <param name="indexPath">Index path.</param>
		/// <param name="item">Item.</param>
		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = _createCell(null, collectionView, indexPath);

			if (!Cells.Contains(cell))
			{
				Cells.Add(cell);
			}

			cell.UpdateSubViews();

			return cell;
		}

		/// <summary>
		/// Gets the touching cell view by bounds.
		/// </summary>
		/// <returns>The touching cell view.</returns>
		/// <param name="viewBounds">View bounds.</param>
		public BaseCollectionViewCell GetTouchingCellView<T>(CGRect viewBounds) where T : BaseCollectionViewCell
		{
			var touchingCell = default(BaseCollectionViewCell);

			foreach (var cell in Cells.OfType<T>())
			{
				if (viewBounds.IntersectsWith(cell.Frame) && touchingCell == null)
				{
					touchingCell = cell;
				}
			}

			_selectedCells = 0;

			return touchingCell;
		}

		/// <summary>
		/// Gets the touching cell view by point.
		/// </summary>
		/// <returns>The touching cell view.</returns>
		/// <param name="point">Point.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public BaseCollectionViewCell GetTouchingCellView<T>(CGPoint point) where T : BaseCollectionViewCell
		{
			var touchingCell = default(BaseCollectionViewCell);

			foreach (var cell in Cells.OfType<T>())
			{
				var frame = cell.Frame;
				//frame.Y -= CollectionView.ContentOffset.Y;

				if(frame.Contains(point))
				{
					touchingCell = cell;
				}
			}

			_selectedCells = 0;

			return touchingCell;
		}

		/// <summary>
		/// Checks the touching cell.
		/// </summary>
		/// <param name="viewBounds">View bounds.</param>
		public object GetTouchingCellBindingContext<T>(CGRect viewBounds) where T : BaseCollectionViewCell
		{
			var touchingCellBindingContext = default(object);

			foreach (var cell in Cells.OfType<T>())
			{
				if (viewBounds.IntersectsWith(cell.Frame) && touchingCellBindingContext == null)
				{
					//touchingCellBindingContext = cell.DataContext;
				}

				cell.Selected = false;
			}

			_selectedCells = 0;

			return touchingCellBindingContext;
		}

		/// <summary>
		/// Highlights the touching cell.
		/// </summary>
		/// <returns>The touching cell.</returns>
		/// <param name="viewBounds">View bounds.</param>
		public void HighlightTouchingCell<T>(CGRect viewBounds) where T : BaseCollectionViewCell
		{
			_selectedCells = 0;

			foreach (var cell in Cells.OfType<T>())
			{
				//cell.ViewModel.IsSelected = (_selectedCells > 0) ? false : viewBounds.IntersectsWith(cell.Frame);
				//_selectedCells += cell.ViewModel.IsSelected ? 1 : 0;
			}
		}
	}
}
