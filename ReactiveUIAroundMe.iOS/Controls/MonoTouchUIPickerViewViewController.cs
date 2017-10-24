using System;
using System.Collections.Generic;
using System.Drawing;

using Foundation;
using UIKit;
using ReactiveUIAroundMe.iOS.Controls;

namespace ReactiveUIAroundMe.iOS.Controls
{
	/// <summary>
	/// Mono touch UIP icker view view controller.
	/// </summary>
	public partial class MonoTouchUIPickerViewViewController : UIViewController
	{
		/// <summary>
		/// The selected.
		/// </summary>
		private int _selected;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MonoTouchUIPickerView.MonoTouchUIPickerViewViewController"/> class.
		/// </summary>
		public MonoTouchUIPickerViewViewController() : base("MonoTouchUIPickerViewViewController", null)
		{
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Perform any additional setup after loading the view, typically from a nib.
			this.SetupPicker();
		}

		/// <summary>
		/// Shoulds the autorotate to interface orientation.
		/// </summary>
		/// <returns><c>true</c>, if autorotate to interface orientation was shoulded, <c>false</c> otherwise.</returns>
		/// <param name="toInterfaceOrientation">To interface orientation.</param>
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		/// <summary>
		/// Setups the picker.
		/// </summary>
		private void SetupPicker()
		{
			var weeksNumbers = new List<int>();
			for (int i = 1; i < 20; i++)
			{
				weeksNumbers.Add(i);
			}

			// Setup the picker and model
			var model = new WeeksPickerViewModel<int>(weeksNumbers);
			model.PickerChanged += (sender, e) =>
			{
				_selected = (int)e.SelectedValue;
			};

			UIPickerView picker = new UIPickerView();
			picker.ShowSelectionIndicator = true;
			picker.Model = model;

			// Setup the toolbar
			UIToolbar toolbar = new UIToolbar();
			toolbar.BarStyle = UIBarStyle.Black;
			toolbar.Translucent = true;
			toolbar.SizeToFit();

			// Create a 'done' button for the toolbar and add it to the toolbar
			UIBarButtonItem doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done,
															 (s, e) =>
															 {
																 //this.ColorTextField.Text = selectedColor;
																 //this.ColorTextField.ResignFirstResponder();
															 });
			toolbar.SetItems(new UIBarButtonItem[] { doneButton }, true);

			//// Tell the textbox to use the picker for input
			//this.ColorTextField.InputView = picker;

			//// Display the toolbar over the pickers
			//this.ColorTextField.InputAccessoryView = toolbar;
		}
	}
}