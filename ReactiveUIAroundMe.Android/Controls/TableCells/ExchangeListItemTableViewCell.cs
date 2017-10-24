//// --------------------------------------------------------------------------------------------------
////  <copyright file="EReactiveUIAroundMeListItemTableViewCell.cs" company="Flush Arcade.">
////    Copyright (c) 2014 Flush Arcade. All rights reserved.
////  </copyright>
//// --------------------------------------------------------------------------------------------------

//namespace ReactiveUIAroundMe.Droid.Controls
//{
//	using System;
//	using System.Linq;
//	using System.Reactive.Linq;

//	using Foundation;
//	using UIKit;
//	using CoreGraphics;

//	using ReactiveUI;

//	using ReactiveUIAroundMe.Droid.Extras;
//	using ReactiveUIAroundMe.Droid.Extensions;

//	using ReactiveUIAroundMe.Portable.ViewModels;

//	/// <summary>
//	/// EReactiveUIAroundMe list item table view cell.
//	/// </summary>
//	public partial class EReactiveUIAroundMeListItemTableViewCell : BaseTableViewCell, IViewFor<EReactiveUIAroundMeListItemViewModel>
//	{
//		/// <summary>
//		/// The key.
//		/// </summary>
//		public static readonly NSString Key = new NSString("EReactiveUIAroundMeListItemTableViewCell");

//		/// <summary>
//		/// The title label.
//		/// </summary>
//		private UILabel _nameLabel;

//		/// <summary>
//		/// The sub title label.
//		/// </summary>
//		private UILabel _addressLabel;

//		/// <summary>
//		/// The distance label.
//		/// </summary>
//		private UILabel _distanceLabel;

//		/// <summary>
//		/// The arrow image view.
//		/// </summary>
//		private UIImageView _arrowImageView;

//		/// <summary>
//		/// The left view.
//		/// </summary>
//		private UIView _leftView;

//		/// <summary>
//		/// The view model.
//		/// </summary>
//		private EReactiveUIAroundMeListItemViewModel _viewModel;

//		/// <summary>
//		/// Gets or sets the view model.
//		/// </summary>
//		/// <value>The view model.</value>
//		public EReactiveUIAroundMeListItemViewModel ViewModel
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
//			set { ViewModel = value as EReactiveUIAroundMeListItemViewModel; }
//		}

//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:MatchDayManagement.iOS.InjurySearchResultTableViewCell"/> class.
//		/// </summary>
//		/// <param name="handle">Handle.</param>
//		protected EReactiveUIAroundMeListItemTableViewCell(IntPtr handle) : base(handle)
//		{
//			_nameLabel = new UILabel()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				Font = UIFont.FromName("Helvetica", 16f),
//				TextColor = UIColor.Clear.FromHex("#444444"),
//			};

//			_addressLabel = new UILabel()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				Font = UIFont.FromName("Helvetica", 12f),
//				TextColor = UIColor.Clear.FromHex("#666666")
//			};

//			_distanceLabel = new UILabel()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				Font = UIFont.FromName("Helvetica", 12f),
//				TextColor = UIColor.Clear.FromHex("#666666"),
//				TextAlignment = UITextAlignment.Right,
//			};

//			_arrowImageView = new UIImageView()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				ContentMode = UIViewContentMode.ScaleAspectFit,
//			};

//			_leftView = new UIView()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				BackgroundColor = UIColor.Clear.FromHex("#ca505d")
//			};

//			var views = new DictionaryViews()
//			{
//				{"nameLabel", _nameLabel},
//				{"addressLabel", _addressLabel},
//				{"distanceLabel", _distanceLabel},
//				{"arrowImageView", _arrowImageView},
//			};

//			ContentView.Add(_nameLabel);
//			ContentView.Add(_addressLabel);
//			ContentView.Add(_distanceLabel);
//			ContentView.Add(_arrowImageView);

//			ContentView.AddConstraints(
//				NSLayoutConstraint.FromVisualFormat("V:|-10-[nameLabel][addressLabel]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
//				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-10-[arrowImageView(20)][distanceLabel]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, views))
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-18-[nameLabel]-5-[arrowImageView(20)]-15-|", NSLayoutFormatOptions.AlignAllTop, null, views))
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-18-[addressLabel]-5-[distanceLabel(60)]-15-|", NSLayoutFormatOptions.AlignAllTop, null, views))
//				.ToArray());

//			// selected view
//			SelectedBackgroundView = new UIView(ContentView.Bounds)
//			{
//				BackgroundColor = UIColor.Clear.FromHex("#fefafa"),
//			};

//			SelectedBackgroundView.Add(_leftView);

//			var selectedViews = new DictionaryViews()
//			{
//				{"leftView", _leftView}
//			};

//			SelectedBackgroundView.AddConstraints(
//				NSLayoutConstraint.FromVisualFormat("V:|[leftView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, selectedViews)
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[leftView(3)]", NSLayoutFormatOptions.AlignAllTop, null, selectedViews))
//				.ToArray());
//		}

//		/// <summary>
//		/// Updates the selected style.
//		/// </summary>
//		/// <param name="selected">If set to <c>true</c> selected.</param>
//		/*protected override void UpdateSelectedStyle(bool selected)
//		{
//			_nameLabel.TextColor = selected ? UIColor.Clear.FromHex("#ca505d") :
//				UIColor.Clear.FromHex("#727272");

//			BackgroundColor = selected ? UIColor.Clear.FromHex("#fefafa") 
//			                                    : UIColor.White;
			
//			base.UpdateSelectedStyle(selected);
//		}*/

//		/// <summary>
//		/// Initialize this instance.
//		/// </summary>
//		public override void Initialize()
//		{
//			this.WhenAnyValue(v => v.ViewModel.CompanyName).BindTo(this,
//				v => v._nameLabel.Text);
			
//			this.WhenAnyValue(v => v.ViewModel.Address).BindTo(this,
//				v => v._addressLabel.Text);
			
//			this.WhenAnyValue(v => v.ViewModel.DistanceDisplay).BindTo(this,
//				v => v._distanceLabel.Text);

//			ViewModel.WhenAnyValue(x => x.Bearing)
//				 .Select(t => RotateImage(UIImage.FromFile("directional-arrow.png"), t))
//				 .BindTo(this, x => x._arrowImageView.Image);			
//		}

//		/// <summary>
//		/// Rotates the image.
//		/// </summary>
//		/// <returns>The image.</returns>
//		/// <param name="originalImage">Original image.</param>
//		/// <param name="rotationAngle">Rotation angle.</param>
//		public UIImage RotateImage(UIImage originalImage, double rotationAngle)
//		{
//			UIImage rotatedImage = originalImage;

//			if (rotationAngle > 0)
//			{
//				CGSize rotatedSize;
//				float angle = Convert.ToSingle((Math.PI / 180) * rotationAngle);

//				using (UIView rotatedViewBox = new UIView(new CGRect(0, 0, originalImage.Size.Width, originalImage.Size.Height)))
//				{
//					CGAffineTransform t = CGAffineTransform.MakeRotation(angle);
//					rotatedViewBox.Transform = t;
//					rotatedSize = new CGSize(originalImage.Size.Width, originalImage.Size.Height);

//					UIGraphics.BeginImageContext(rotatedSize);
//					CGContext context = UIGraphics.GetCurrentContext();

//					context.TranslateCTM(rotatedSize.Width / 2, rotatedSize.Height / 2);
//					context.RotateCTM(angle);
//					context.ScaleCTM((nfloat)1.0, -(nfloat)1.0);

//					context.DrawImage(new CGRect(-originalImage.Size.Width / 2, -originalImage.Size.Height / 2, originalImage.Size.Width, originalImage.Size.Height), originalImage.CGImage);

//					rotatedImage = UIGraphics.GetImageFromCurrentImageContext();

//					UIGraphics.EndImageContext();
//				}

//			}

//			return rotatedImage;
//		}
//	}
//}
