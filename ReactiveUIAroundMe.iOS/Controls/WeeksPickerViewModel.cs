using System;
using System.Collections.Generic;

using UIKit;

namespace ReactiveUIAroundMe.iOS.Controls
{
	public class WeeksPickerViewModel<T> : UIPickerViewModel
	{
		private List<T> _items;
		protected int selectedIndex = 0;

		public event EventHandler<PickerChangedEventArgs> PickerChanged;

		public WeeksPickerViewModel(List<T> items)
		{
			_items = items;
		}

		public T SelectedItem
		{
			get { return _items[selectedIndex]; }
		}

		public override nint GetComponentCount(UIPickerView picker)
		{
			return 1;
		}

		public override nint GetRowsInComponent(UIPickerView picker, nint component)
		{
			return _items.Count;
		}

		public override string GetTitle(UIPickerView picker, nint row, nint component)
		{
			return _items[(int)row].ToString();
		}

		public override void Selected(UIPickerView picker, nint row, nint component)
		{
			selectedIndex = (int)row;

			if (this.PickerChanged != null)
			{
				this.PickerChanged(this, new PickerChangedEventArgs { SelectedValue = _items[(int)row] });
			}
		}
	}
}
