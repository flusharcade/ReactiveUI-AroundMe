// --------------------------------------------------------------------------------------------------
//  <copyright file="TetrixLayout.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS
{
	using System.Linq;
	using System.Collections.Generic;

	using CoreGraphics;
	using Foundation;
	using UIKit;

	using ReactiveUIAroundMe.Portable.ViewModels;
	using ReactiveUIAroundMe.Portable.Enums;
    using ReactiveUI;

	/// <summary>
	/// Tetrix layout.
	/// </summary>
	public class TetrixLayout : UICollectionViewLayout
	{
		/// <summary>
		/// The cell count.
		/// </summary>
		private int _cellCount;

		/// <summary>
		/// The current y.
		/// </summary>
		private double _currentY;

		/// <summary>
		/// The height of the current.
		/// </summary>
		private double _currentHeight;

		/// <summary>
		/// The current x.
		/// </summary>
		private double _currentX;

		/// <summary>
		/// The current line.
		/// </summary>
		private double _currentLine;

		/// <summary>
		/// The x spacing.
		/// </summary>
		private double _itemXSpacing;

		/// <summary>
		/// The  y spacing.
		/// </summary>
		private double _itemYSpacing;

		/// <summary>
		/// The number of lines.
		/// </summary>
		private int _numberOfLines;

		/// <summary>
		/// The size of the collection view.
		/// </summary>
		private CGSize _collectionViewSize;

		/// <summary>
		/// The requires height calculation.
		/// </summary>
		private bool _requiresHeightCalculation;

		/// <summary>
		/// The cell widths.
		/// </summary>
		private IDictionary<LayoutType, double> widthScales = new Dictionary<LayoutType, double>()
		{
			{LayoutType.Fifth, 0.2},
			{LayoutType.Quarter, 0.25},
			{LayoutType.Half, 0.5},
			{LayoutType.Third, 0.33},
			{LayoutType.Fill, 1},
		};

		static NSString myDecorationViewId = new NSString("MyDecorationView");

		public TetrixLayout(int xSpacing = 15, int ySpacing = 15, bool requiresHeightCalculation = false)
		{
			_requiresHeightCalculation = requiresHeightCalculation;

			RegisterClassForDecorationView(typeof(MyDecorationView), myDecorationViewId);

			// we divide by two because we have to evenly spread spacing betwee height reduction and coordinate value
			_itemXSpacing = xSpacing / 2;
			_itemYSpacing = ySpacing / 2;
		}

		/// <summary>
		/// Prepares the layout.
		/// </summary>
		public override void PrepareLayout()
		{
			base.PrepareLayout();

			_cellCount = (int)CollectionView.NumberOfItemsInSection(0);
			_collectionViewSize = CollectionView.Frame.Size;
		}

		/// <summary>
		/// Gets the size of the collection view content.
		/// </summary>
		/// <value>The size of the collection view content.</value>
		public override CGSize CollectionViewContentSize
		{
			get
			{
				return _collectionViewSize;
			}
		}

		/// <summary>
		/// Gets the items.
		/// </summary>
		/// <value>The items.</value>
		private IList<TetrixViewModelBase> _items
		{
			get
			{
				var source = (CollectionView.DataSource as ReactiveCollectionViewSource<BaseCollectionViewCell>);
				if (source != null)
				{
					return null;
					//return source.ItemsSource.OfType<TetrixViewModelBase>().ToList();
				}

				return null;
			}
		}

		/// <summary>
		/// Shoulds the invalidate layout for bounds change.
		/// </summary>
		/// <returns><c>true</c>, if invalidate layout for bounds change was shoulded, <c>false</c> otherwise.</returns>
		/// <param name="newBounds">New bounds.</param>
		public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
		{
			return true;
		}

		/// <summary>
		/// Layouts the attributes for item.
		/// </summary>
		/// <returns>The attributes for item.</returns>
		/// <param name="path">Path.</param>
		public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath path)
		{
			var attributes = UICollectionViewLayoutAttributes.CreateForCell(path);

			var item = _items[path.Row];
			var scale = widthScales[item.Layout];
			var width = CollectionViewContentSize.Width * scale;

			// make sure top and left elements aren't spaced from borders
			// if cell wants spacing disabled, we set spacing to 0
			var xSpacing = item.UseXSpacing ? _currentX > 0 ? _itemXSpacing : 0 : 0;
			var ySpacing = item.UseYSpacing ? _currentY > 0 ? _itemYSpacing : 0 : 0;

			// check first if we have started a new, we want to record the total height from everyline to resize the content size
			if (_currentLine < 0.01)
			{
				_currentHeight += item.Height + ySpacing;
			}

			_currentLine += scale + item.XSpacingScale;

			// avoids tiny gaps when not using x spacing
			var minorAdj = item.UseXSpacing ? 0 : 0.1;

			// HeightCalculated determines whether we want the layout to calculate the height, this is used when we dont want the collection view scrollable,
			// so the layout will calculate space available and divide between all other items with HeightCalculated set to true
			attributes.Frame = new CGRect(_currentX + xSpacing - minorAdj, _currentY + ySpacing, width - xSpacing + minorAdj, item.Height);
						    
			// we only jump to a new line when we are on a cell with right or fill layout
			if (_currentLine < 0.99)
			{
				_currentX += width + (CollectionViewContentSize.Width * item.XSpacingScale);
			}
			else
			{
				_currentX = 0;
				_currentLine = 0;
			}

			// check again for a new line to increase the currentY for the starting element on a new line
			if (_currentLine < 0.01)
			{
				_currentY += item.Height + ySpacing;
				_numberOfLines++;
			}

			return attributes;
		}

		/// <summary>
		/// Calculates the height for items.
		/// </summary>
		private void CalculateHeightForItems()
		{
			var calculatedHeightItems = _items.Where(x => x.HeightCalculated);
			var calculatedHeight = CollectionViewContentSize.Height / _numberOfLines;

			foreach (var item in calculatedHeightItems)
			{
				item.Height = calculatedHeight;
			}
		}

		/// <summary>
		/// Layouts the attributes for elements in rect.
		/// </summary>
		/// <returns>The attributes for elements in rect.</returns>
		/// <param name="rect">Rect.</param>
		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
		{
			var attributes = new UICollectionViewLayoutAttributes[_cellCount + 1];

			CalculateAllAttributes(attributes);

			if (_requiresHeightCalculation)
			{
				CalculateHeightForItems();

				CalculateAllAttributes(attributes);
			}

			// we must set the content size to the total height of all cells recorded in _currentY
			var width = _collectionViewSize.Width;
			_collectionViewSize = new CGSize(width, _currentHeight);

			var decorationAttribs = UICollectionViewLayoutAttributes.CreateForDecorationView(myDecorationViewId, NSIndexPath.FromItemSection(0, 0));
			decorationAttribs.Size = new CGSize(CollectionView.Frame.Width, CollectionView.Frame.Height);
			decorationAttribs.ZIndex = -1;
			attributes[_cellCount] = decorationAttribs;

			return attributes;
		}

		/// <summary>
		/// Calculates all attributes.
		/// </summary>
		private void CalculateAllAttributes(UICollectionViewLayoutAttributes[] attributes)
		{
			_currentX = 0;
			_currentY = 0;
			_currentLine = 0;
			_numberOfLines = 0;
			_currentHeight = 0;

			for (int i = 0; i < _cellCount; i++)
			{
				NSIndexPath indexPath = NSIndexPath.FromItemSection(i, 0);
				attributes[i] = LayoutAttributesForItem(indexPath);
			}
		}
	}

	/// <summary>
	/// My decoration view.
	/// </summary>
	public class MyDecorationView : UICollectionReusableView
	{
		[Export("initWithFrame:")]
		public MyDecorationView(CGRect frame) : base(frame)
		{
		}
	}
}