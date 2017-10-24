using System;
namespace ReactiveUIAroundMe.iOS
{
	public class PickerChangedEventArgs : EventArgs
	{
		public object SelectedValue { get; set; }
	}
}
