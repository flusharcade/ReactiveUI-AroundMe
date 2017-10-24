// --------------------------------------------------------------------------------------------------
//  <copyright file="TileCollectionViewCell.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS
{
	using System;
	using System.Linq;

	using Foundation;
	using UIKit;
	using ReactiveUI;

	using ReactiveUIAroundMe.iOS.Extras;
	using ReactiveUIAroundMe.Portable.ViewModels;
	using System.Reactive.Linq;

	/// <summary>
	/// Umpire collection view cell.
	/// </summary>
	public partial class TileCollectionViewCell : BaseCollectionViewCell, IViewFor<TileViewModel>
	{
		/// <summary>
		/// The key.
		/// </summary>
		public static readonly NSString Key = new NSString("TileCollectionViewCell");

		TileViewModel _ViewModel;
		public TileViewModel ViewModel
		{
			get { return _ViewModel; }
			set { this.RaiseAndSetIfChanged(ref _ViewModel, value); }
		}

		object IViewFor.ViewModel
		{
			get { return _ViewModel; }
			set { ViewModel = (TileViewModel)value; }
		}

		/// <summary>
		/// The home image view.
		/// </summary>
		private UIImageView _tileImageView;

		/// <summary>
		/// The venue image view.
		/// </summary>
		private UIImageView _bannerImageView;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.TileCollectionViewCell"/> class.
		/// </summary>
		/// <param name="handle">Handle.</param>
		protected TileCollectionViewCell(IntPtr handle) : base(handle)
		{
			ContentView.Layer.BorderWidth = 1;
			ContentView.Layer.BorderColor = iOSColorPalette.GradientStroke3.CGColor;

			AllowSelectedStyle = false;

			_bannerImageView = new UIImageView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				ContentMode = UIViewContentMode.ScaleToFill,
			};

			_tileImageView = new UIImageView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				ContentMode = UIViewContentMode.ScaleAspectFit,
				Image = UIImage.FromFile("profile_image.jpeg"),
			};
			_tileImageView.Layer.CornerRadius = 22.5f;
			_tileImageView.Layer.MasksToBounds = true;

			var stateLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Font = UIFont.FromName("Helvetica", 12f),
				TextAlignment = UITextAlignment.Right,
				TextColor = iOSColorPalette.Red
			};

			var firstNameLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Font = UIFont.FromName("Helvetica", 16f),
				TextColor = iOSColorPalette.DarkGray1
			};

			var lastNameLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Font = UIFont.FromName("Helvetica", 20f),
				TextColor = iOSColorPalette.DarkGray1
			};

			var titleGradientView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};

			var titleLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Font = UIFont.FromName("Helvetica", 20f),
				TextAlignment = UITextAlignment.Center,
				UserInteractionEnabled = false,
			};

			CreateViewGradient(titleGradientView, 50);

			var views = new DictionaryViews()
			{
				{"stateLabel", stateLabel},
				{"firstNameLabel", firstNameLabel},
				{"lastNameLabel", lastNameLabel},
				{"tileImageView", _tileImageView},
				{"bannerImageView", _bannerImageView},
				{"titleLabel", titleLabel},
				{"titleGradientView", titleGradientView},
			};

			ContentView.Add(stateLabel);
			ContentView.Add(firstNameLabel);
			ContentView.Add(lastNameLabel);
			ContentView.Add(_bannerImageView);
			//ContentView.Add(_tileImageView);
			ContentView.Add(titleGradientView);
			ContentView.Add(titleLabel);

			ContentView.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[bannerImageView(70)]-2-[stateLabel(20)]-5-[firstNameLabel(25)][lastNameLabel(35)]-5-[titleLabel(50)]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|[bannerImageView(70)]-2-[stateLabel(20)]-5-[firstNameLabel(25)][lastNameLabel(35)]-5-[titleGradientView(50)]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views))
				//.Concat(NSLayoutConstraint.FromVisualFormat("V:|-45-[tileImageView(45)]", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[bannerImageView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				//.Concat(NSLayoutConstraint.FromVisualFormat("H:|-10-[tileImageView(45)]", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-10-[firstNameLabel]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-10-[lastNameLabel]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[stateLabel]-10-|", NSLayoutFormatOptions.AlignAllTop, null, views))
			    .Concat(NSLayoutConstraint.FromVisualFormat("H:|[titleLabel]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[titleGradientView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views))
				.ToArray());

			this.WhenAny(x => x.ViewModel, x => x.Value)
				.Where(x => x != null)
				.Subscribe(x =>
				{
					titleLabel.Text = x.Title;
					//set.Bind(_bannerImageViewLoader).For(i => i.DefaultImagePath)
				   	//	.To(vm => vm.BannerImage);
				});
		}
	}
}
