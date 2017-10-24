// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UmpireEditcs" company="Champion Data Pty Ltd.">
//   Copyright (c) 2015 Champion Data Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS
{
	using System;
	using System.Linq;
	using System.Collections.Generic;

	using Foundation;
	using UIKit;

	using ReactiveUIAroundMe.iOS.Extras;
	using ReactiveUIAroundMe.Portable.ViewModels;

	/// <summary>
	/// Umpire edit view.
	/// </summary>
	public class AddUmpireAlertView : BaseMvxView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.AddUmpireAlertView"/> class.
		/// </summary>
		public AddUmpireAlertView()
		{
			Layer.BorderWidth = 1;
			Layer.BorderColor = iOSColorPalette.GradientStroke3.CGColor;
		
			BackgroundColor = UIColor.White;

			var topView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};

			CreateViewGradient(topView);

			var cancelButton = new UIButton()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};
			cancelButton.SetTitle("Cancel", UIControlState.Normal);
			cancelButton.SetTitleColor(iOSColorPalette.Red, UIControlState.Normal);

			var saveButton = new UIButton()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};
			saveButton.SetTitle("Save", UIControlState.Normal);
			saveButton.SetTitleColor(iOSColorPalette.Red, UIControlState.Normal);

			var retireButton = new UIButton()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = iOSColorPalette.Red
			};
			retireButton.SetTitle("Retire", UIControlState.Normal);
			retireButton.SetTitleColor(UIColor.White, UIControlState.Normal);

			var titleLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.FromName("Helvetica", 16f),
				Text = "Edit",
			};

			var firstNameTextField = new UITextField()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Placeholder = "First Name"
			};

			var surnameTextField = new UITextField()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Placeholder = "Surname"
			};

			topView.Add(saveButton);
			topView.Add(titleLabel);
			topView.Add(cancelButton);

			Add(topView);
			Add(retireButton);
			Add(firstNameTextField);
			Add(surnameTextField);

			var topViews = new DictionaryViews()
			{
				{"cancelButton", cancelButton},
				{"saveButton", saveButton},
				{"titleLabel", titleLabel},
			};

			var views = new DictionaryViews()
			{
				{"topView", topView},
				{"retireButton", retireButton},
				{"firstNameTextField", firstNameTextField},
				{"surnameTextField", surnameTextField},
			};

			topView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("H:|-5-[cancelButton]-2-[titleLabel(cancelButton)]-2-[saveButton(cancelButton)]-5-|", NSLayoutFormatOptions.AlignAllTop, null, topViews)
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|[cancelButton]|", NSLayoutFormatOptions.DirectionLeftToRight, null, topViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|[titleLabel]|", NSLayoutFormatOptions.DirectionLeftToRight, null, topViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|[saveButton]|", NSLayoutFormatOptions.DirectionLeftToRight, null, topViews))
				.ToArray());
			
			AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|[topView(40)]-[firstNameTextField][surnameTextField][retireButton(35)]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[topView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-5-[firstNameTextField]-5-|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-5-[surnameTextField]-5-|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[retireButton]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.ToArray());

			// create the binding set
			//var set = this.CreateBindingSet<AddUmpireAlertView, UmpireViewModel>();
			//set.Bind(firstNameTextField).To(vm => vm.FirstName);
			//set.Bind(surnameTextField).To(vm => vm.Surname);
			//set.Bind(cancelButton).To(vm => vm.CancelCommand);
			//set.Bind(saveButton).To(vm => vm.SaveCommand);
			//set.Bind(retireButton).To(vm => vm.RetireCommand);

			//set.Apply();
		}
	}
}
