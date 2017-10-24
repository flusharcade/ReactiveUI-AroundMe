
namespace ReactiveUIAroundMe.iOS.Extras
{
	using System;

	using Foundation;
	using UIKit;

	using ReactiveUIAroundMe.Portable.UI;

	public class OrientationHandleriOS : IOrientationHandler
	{
		public void ForceLandscape()
		{
			UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));
		}

		public void ForcePortrait()
		{
			UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
		}
	}
}