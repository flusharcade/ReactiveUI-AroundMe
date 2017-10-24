using System;
using UIKit;

namespace ReactiveUIAroundMe.iOS.Controls
{
	public class CustomIndicatorView : UIActivityIndicatorView
	{
		private bool _isRunning;
		public bool IsRunning
		{
			get { return _isRunning; }
			set { 
				_isRunning = value;

				if (value)
				{
					StartAnimating();
				}
				else 
				{
					StopAnimating();
				}

				Hidden = !value; 
			}
		}
	}
}
