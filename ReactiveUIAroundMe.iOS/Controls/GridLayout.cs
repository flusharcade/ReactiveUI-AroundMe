
namespace ReactiveUIAroundMe.iOS
{
	using System;

	using CoreGraphics;
	using Foundation;
	using UIKit;

	public class GridLayout : UICollectionViewFlowLayout
	{
		public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath path)
		{
			return base.LayoutAttributesForItem(path);
		}

		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
		{
			return base.LayoutAttributesForElementsInRect(rect);
		}
	}
}
