//// --------------------------------------------------------------------------------------------------
////  <copyright file="FeedbackListItemTableViewCell.cs" company="Flush Arcade.">
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

//	using ReactiveUIAroundMe.Portable.ViewModels;

//	/// <summary>
//	/// Feedback list item table view cell.
//	/// </summary>
//	public partial class FeedbackListItemTableViewCell : BaseTableViewCell, IViewFor<FeedbackListItemViewModel>
//	{
//		/// <summary>
//		/// The key.
//		/// </summary>
//		public static readonly NSString Key = new NSString("FeedbackListItemTableViewCell");

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

//		private FeedbackListItemViewModel _viewModel;
//		public FeedbackListItemViewModel ViewModel
//		{
//			get { return _viewModel; }
//			set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
//		}

//		object IViewFor.ViewModel
//		{
//			get { return ViewModel; }
//			set { ViewModel = value as FeedbackListItemViewModel; }
//		}

//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:MatchDayManagement.iOS.InjurySearchResultTableViewCell"/> class.
//		/// </summary>
//		/// <param name="handle">Handle.</param>
//		protected FeedbackListItemTableViewCell(IntPtr handle) : base(handle)
//		{
//			_nameLabel = new UILabel()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				Font = UIFont.FromName("Helvetica", 18f),
//			};

//			_addressLabel = new UILabel()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				Font = UIFont.FromName("Helvetica", 14f),
//			};

//			_distanceLabel = new UILabel()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				Font = UIFont.FromName("Helvetica", 14f),
//			};

//			_arrowImageView = new UIImageView()
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				ContentMode = UIViewContentMode.ScaleAspectFit,
//			};

//			var views = new DictionaryViews()
//			{
//				{"nameLabel", _nameLabel},
//				{"addressLabel", _addressLabel},
//				{"distanceLabel", _distanceLabel},
//				{"arrowImageView", _arrowImageView}
//			};

//			ContentView.Add(_nameLabel);
//			ContentView.Add(_addressLabel);
//			ContentView.Add(_distanceLabel);
//			ContentView.Add(_arrowImageView);

//			ContentView.AddConstraints(
//				NSLayoutConstraint.FromVisualFormat("V:|-5-[nameLabel]-[addressLabel]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
//				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-5-[arrowImageView(25)]-[distanceLabel]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, views))
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-10-[nameLabel]-10-[arrowImageView(25)]-10-|", NSLayoutFormatOptions.AlignAllTop, null, views))
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-10-[addressLabel]-10-[distanceLabel(40)]-10-|", NSLayoutFormatOptions.AlignAllTop, null, views))
//				.ToArray());
//		}

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
