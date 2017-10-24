//// --------------------------------------------------------------------------------------------------
////  <copyright file="LoginPage.cs" company="Flush Arcade.">
////    Copyright (c) 2014 Flush Arcade. All rights reserved.
////  </copyright>
//// --------------------------------------------------------------------------------------------------

//namespace ReactiveUIAroundMe.Droid.Views
//{
//	using System.Linq;
//	using System.Drawing;
//	using System.Reactive;
//	using System.Reactive.Linq;

//	using UIKit;
//	using CoreGraphics;

//	using ReactiveUI;

//	using ReactiveUIAroundMe.Droid.Extras;
//	using ReactiveUIAroundMe.Droid.Controls;

//	using ReactiveUIAroundMe.Portable.ViewModels;
//	using ReactiveUIAroundMe.Portable.Common;

//	/// <summary>
//	/// Injury manager page.
//	/// </summary>
//	public class LoginPage : BaseMvxViewController, IViewFor<LoginPageViewModel>
//	{
//		/// <summary>
//		/// The view model.
//		/// </summary>
//		LoginPageViewModel _viewModel;

//		/// <summary>
//		/// Gets or sets the view model.
//		/// </summary>
//		/// <value>The view model.</value>
//		public new LoginPageViewModel ViewModel
//		{
//			get { return _viewModel; }
//			set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
//		}

//		/// <summary>
//		/// Gets or sets the reactive user interface . IV iew for. view model.
//		/// </summary>
//		/// <value>The reactive user interface . IV iew for. view model.</value>
//		object IViewFor.ViewModel
//		{
//			get { return ViewModel; }
//			set { ViewModel = (LoginPageViewModel)value; }
//		}

//		/// <summary>
//		/// The username text field.
//		/// </summary>
//		private UITextField _usernameTextField;

//		/// <summary>
//		/// The password text field.
//		/// </summary>
//		private UITextField _passwordTextField;

//		/// <summary>
//		/// The login button.
//		/// </summary>
//		private UIButton _loginButton;

//		/// <summary>
//		/// Views the did load.
//		/// </summary>
//		public override void ViewDidLoad()
//		{
//			base.ViewDidLoad();

//			//base.StyleNavigationBar();

//			View.BackgroundColor = UIColor.White;

//			Title = "Login";

//			var mainView = new UIView()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//			};

//			Add(mainView);

//			_usernameTextField = new UITextField()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				Placeholder = "Username",
//			};

//			_passwordTextField = new UITextField()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				Placeholder = "Password",
//				SecureTextEntry = true,
//			};

//			var errorLabel = new UILabel()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				TextAlignment = UITextAlignment.Center,
//				Font = UIFont.FromName("Helvetica", 20f),
//				TextColor = iOSColorPalette.Red,
//			};

//			_loginButton = new UIButton()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				Font = UIFont.FromName("Alfa Slab One", 20f),
//				BackgroundColor = iOSColorPalette.Blue
//			};
//			_loginButton.SetTitle("Login", UIControlState.Normal);
//			_loginButton.SetTitleColor(UIColor.White, UIControlState.Normal);

//			var activityIndicatorView = new CustomIndicatorView()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				Color = UIColor.Black,
//				Hidden = true,
//			};

//			mainView.Add(_usernameTextField);
//			mainView.Add(_passwordTextField);
//			mainView.Add(errorLabel);
//			mainView.Add(_loginButton);
//			mainView.Add(activityIndicatorView);

//			var views = new DictionaryViews()
//			{
//				{"mainView", mainView},
//			};

//			var mainViews = new DictionaryViews()
//			{
//				{"usernameTextField", _usernameTextField},
//				{"passwordTextField", _passwordTextField},
//				{"errorLabel", errorLabel},
//				{"loginButton", _loginButton},
//				{"activityIndicatorView", activityIndicatorView}
//			};

//			View.AddConstraints(
//				NSLayoutConstraint.FromVisualFormat("V:|[mainView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[mainView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
//				.ToArray());

//			mainView.AddConstraints(
//				NSLayoutConstraint.FromVisualFormat("V:|-140-[usernameTextField]-40-[passwordTextField]-40-[loginButton]-40-[errorLabel]-40-[activityIndicatorView(60)]", NSLayoutFormatOptions.DirectionLeftToRight, null, mainViews)
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:[usernameTextField(300)]", NSLayoutFormatOptions.AlignAllTop, null, mainViews))
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:[passwordTextField(300)]", NSLayoutFormatOptions.AlignAllTop, null, mainViews))
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:[loginButton(300)]", NSLayoutFormatOptions.AlignAllTop, null, mainViews))
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:[errorLabel(300)]", NSLayoutFormatOptions.AlignAllTop, null, mainViews))
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:[activityIndicatorView(60)]", NSLayoutFormatOptions.AlignAllTop, null, mainViews))
//				.Concat(new[] { NSLayoutConstraint.Create(_usernameTextField, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, mainView, NSLayoutAttribute.CenterX, 1f, 0) })
//				.Concat(new[] { NSLayoutConstraint.Create(_passwordTextField, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, mainView, NSLayoutAttribute.CenterX, 1f, 0) })
//				.Concat(new[] { NSLayoutConstraint.Create(_loginButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, mainView, NSLayoutAttribute.CenterX, 1f, 0) })
//				.Concat(new[] { NSLayoutConstraint.Create(errorLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, mainView, NSLayoutAttribute.CenterX, 1f, 0) })
//				.Concat(new[] { NSLayoutConstraint.Create(activityIndicatorView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, mainView, NSLayoutAttribute.CenterX, 1f, 0) })
//				.ToArray());

//			// create the binding set
//			//var set = this.CreateBindingSet<LoginPage, LoginPageViewModel>();
//			//set.Bind(usernameTextField).To(vm => vm.Username);
//			//set.Bind(passwordTextField).To(vm => vm.Password);
//			//set.Bind(_loginButton).To(vm => vm.LoginCommand);
//			//set.Bind(errorLabel).To(vm => vm.ErrorMessage);
//			//set.Bind(errorLabel).For("Hidden").To(vm => vm.IsError).WithConversion("NotValueConverter", false);
//			//set.Bind(activityIndicatorView).For("IsRunning").To(vm => vm.IsLoading);
//			//set.Apply();

//			this.WhenActivated(d =>
//			{
//				this.Bind(ViewModel, x => x.Username, x => x._usernameTextField.Text);
//				this.Bind(ViewModel, x => x.Password, x => x._passwordTextField.Text);

//				this.BindCommand(ViewModel, x => x.LoginCommand, x => x._loginButton);
//			});
//		}
//	}
//}