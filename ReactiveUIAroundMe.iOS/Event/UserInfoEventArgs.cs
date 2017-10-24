using System;

using Foundation;

namespace ReactiveUIAroundMeNative.iOS.Event
{
	public class UserInfoEventArgs : EventArgs
	{
		public NSDictionary UserInfo { get; set; }
	}
}
