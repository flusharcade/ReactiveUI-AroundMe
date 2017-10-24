// --------------------------------------------------------------------------------------------------
//  <copyright file="MapPage.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Views
{
	using System.Linq;
	using System.Reactive.Linq;
	using System.Collections.Generic;
	using System;
	using System.Reactive.Disposables;
	using System.Collections.ObjectModel;

	using UIKit;
	using MapKit;
	using CoreLocation;
	using Foundation;

	using ReactiveUI;

	using ReactiveUIAroundMe.iOS.Extras;
	using ReactiveUIAroundMe.iOS.Controls;
	using ReactiveUIAroundMe.iOS.Extensions;

	using ReactiveUIAroundMe.Portable.ViewModels;
	using ReactiveUIAroundMe.Portable.UI;
    using ReactiveUIAroundMe.Portable.Common;

    /// <summary>
    /// Search results page.
    /// </summary>
    public class MapPage : BaseViewController, IViewFor<MapPageViewModel>
	{
		#region Constants

		/// <summary>
		/// The button image paths.
		/// </summary>
		private static string[] _buttonTitles = new string[]
		{
			"direction",
			"car",
			"bus",
			"walk",
			"favourite",
			"share",
		};

		/// <summary>
		/// The button image paths.
		/// </summary>
		private static string[] _buttonNonActiveImagePaths = new string[]
		{
			"direction",
			"car",
			"bus",
			"walk",
			"favourite",
			"share",
		};

		/// <summary>
		/// The button active image paths.
		/// </summary>
		private static string[] _buttonActiveImagePaths = new string[]
		{
			"direction_active",
			"car_active",
			"bus_active",
			"walk_active",
			"favourite_active",
			"share_active",
		};

		#endregion

		/// <summary>
		/// The directions showing.
		/// </summary>
		private bool _directionsShowing = true;

		/// <summary>
		/// The gps direction selection.
		/// </summary>
		private nint gpsDirectionSelection = 3;

		/// <summary>
		/// The map view.
		/// </summary>
		private MKMapView _mapView;

		/// <summary>
		/// The map overlay.
		/// </summary>
		private MKPolyline _mkPolyLine;

		/// <summary>
		/// The feedback view.
		/// </summary>
		private UIView _feedbackView;

		/// <summary>
		/// The current address text field.
		/// </summary>
		private UnderlinedUITextField _currentAddressTextField;

		/// <summary>
		/// The progress view.
		/// </summary>
		private UIActivityIndicatorView _progressView;

		/// <summary>
		/// The destination label.
		/// </summary>
		private UILabel _destinationLabel;

		/// <summary>
		/// The phone label.
		/// </summary>
		private UILabel _phoneLabel;

		/// <summary>
		/// The email label.
		/// </summary>
		private UILabel _emailLabel;

		/// <summary>
		/// The website label.
		/// </summary>
		private UILabel _websiteLabel;

		/// <summary>
		/// The player list.
		/// </summary>
		private UITableView _feedbackTableView;

        /// <summary>
        /// The info table view.
        /// </summary>
		private UITableView _infoTableView;

		/// <summary>
		/// The buttons.
		/// </summary>
		private List<UIButton> _buttons;

		/// <summary>
		/// Gets the subscriptions.
		/// </summary>
		/// <value>The subscriptions.</value>
		private SerialDisposable _serialDisposable;

		/// <summary>
		/// The source.
		/// </summary>
		private ReactiveTableViewSource<FeedbackListItemViewModel> _tableSource
		{
			get
			{
				return _feedbackTableView?.Source as ReactiveTableViewSource<FeedbackListItemViewModel>; ;
			}
		}

        /// <summary>
        /// Gets the info table source.
        /// </summary>
        /// <value>The info table source.</value>
        private ReactiveTableViewSource<TetrixViewModelBase> _infoTableSource
		{
			get
			{
				return _infoTableView?.Source as ReactiveTableViewSource<TetrixViewModelBase>; ;
			}
		}

		/// <summary>
		/// Gets or sets the view model.
		/// </summary>
		/// <value>The view model.</value>
		public new MapPageViewModel ViewModel
		{
			get { return (MapPageViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		/// <summary>
		/// Gets or sets the reactive user interface . IV iew for. view model.
		/// </summary>
		/// <value>The reactive user interface . IV iew for. view model.</value>
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (MapPageViewModel)value; }
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.StyleNavigationBar();
			this.NavigationController.SetNavigationBarHidden(false, true);

			_mapView = new MKMapView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				OverlayRenderer = (mapView, overlay) =>
				{
					if (overlay is MKPolyline)
					{
						var route = (MKPolyline)overlay;
						var renderer = new MKPolylineRenderer(route) { StrokeColor = UIColor.Blue, Alpha = 0.5f };

						return renderer;
					}

					return null;
				}
			};

			/* -- directions panel view -- */

			var directionView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};

			var directionTopView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = UIColor.White
			};

			var directionBottomView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = UIColor.White
			};

			directionView.Add(directionTopView);
			directionView.Add(directionBottomView);

			var directionViews = new DictionaryViews()
			{
				{"directionTopView", directionTopView},
				{"directionBottomView", directionBottomView},
			};

			directionView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|[directionTopView(40)][directionBottomView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionViews)
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[directionTopView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[directionBottomView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionViews))
				.ToArray());

			/* -- directions top view -- */

			var directionTopViews = new DictionaryViews();

			_buttons = new List<UIButton>();

			for (int i = 0; i < _buttonTitles.Length; i++)
			{
				var button = new UIButton(UIButtonType.Custom)
				{
					TranslatesAutoresizingMaskIntoConstraints = false,
				};

				// all images except car and direction buttons are active
				button.SetImage(UIImage.FromFile(i == 0 || i == 3 ? _buttonActiveImagePaths[i] : 
				                                _buttonNonActiveImagePaths[i]), UIControlState.Normal);

				button.TintColor = UIColor.Clear.FromHex("#c14d60");

				directionTopViews.Add(_buttonTitles[i].ToLower() + "Button", button);
				directionTopView.Add(button);
				_buttons.Add(button);
			}

			// share and feedback button will highlight when touched rather than change to active
			_buttons[4].SetImage(UIImage.FromFile(_buttonActiveImagePaths[4]), UIControlState.Highlighted);
			_buttons[5].SetImage(UIImage.FromFile(_buttonActiveImagePaths[5]), UIControlState.Highlighted);

			// all gps buttons
			for (int i = 1; i < 4; i++)
			{
				_buttons[i].Tag = i;
				// each button when touched will reset other gps buttons to non active
				// and set self to active
				_buttons[i].TouchUpInside += (sender, e) =>
				{
					var button = (sender as UIButton);
					var index = button.Tag;

					// all gps buttons reset
					for (int j = 1; j < 4; j++)
					{
						if (j != index)
						{
							_buttons[j].SetImage(UIImage.FromFile(_buttonNonActiveImagePaths[j]), UIControlState.Normal);
						}
					}

					button.SetImage(UIImage.FromFile(_buttonActiveImagePaths[index]), UIControlState.Normal);
					gpsDirectionSelection = index;
				};
			}

			// direction button
			_buttons[0].TouchUpInside += (sender, e) =>
			{
				_directionsShowing = !_directionsShowing;
				directionBottomView.Hidden = !_directionsShowing;
				_buttons[0].SetImage(UIImage.FromFile(_directionsShowing ? _buttonActiveImagePaths[0] : 
										_buttonNonActiveImagePaths[0]), UIControlState.Normal);
			};

			var directionTopVerticalSeperator = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = UIColor.Clear.FromHex("#dddddd")
			};

			var directionTopHorizontalSeperator = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = UIColor.Clear.FromHex("#dddddd")
			};

			directionTopViews.Add("directionTopVerticalSeperator", directionTopVerticalSeperator);
			directionTopViews.Add("directionTopHorizontalSeperator", directionTopHorizontalSeperator);

			directionTopView.Add(directionTopVerticalSeperator);
			directionTopView.Add(directionTopHorizontalSeperator);

			directionTopView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|[directionButton]|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionTopViews)
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-7-[carButton]-10-|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionTopViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-7-[busButton]-10-|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionTopViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-7-[walkButton]-10-|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionTopViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-7-[favouriteButton]-10-|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionTopViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-7-[shareButton]-10-|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionTopViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-10-[directionTopVerticalSeperator]-10-|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionTopViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:[directionTopHorizontalSeperator(1)]|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionTopViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[directionButton(30)]-15-[carButton(23)]-15-[busButton(carButton)]-15-[walkButton(carButton)]-15-[directionTopVerticalSeperator(1)]", NSLayoutFormatOptions.AlignAllTop, null, directionTopViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:[favouriteButton(25)]-15-[shareButton(22)]-15-|", NSLayoutFormatOptions.AlignAllTop, null, directionTopViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[directionTopHorizontalSeperator]|", NSLayoutFormatOptions.AlignAllTop, null, directionTopViews))
				.ToArray());

			/* -- directions top view -- */

			/* -- directions bottom view -- */

			_currentAddressTextField = new UnderlinedUITextField()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Text = "Address",
				Font = UIFont.FromName("Arial", 12f),
			};

			_destinationLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Font = UIFont.FromName("Arial", 12f),
				Text = "Address",
			};

			var directionImageView = new UIImageView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				ContentMode = UIViewContentMode.ScaleAspectFit,
				Image = UIImage.FromFile("direction-sign.png")
			};

			directionBottomView.Add(_currentAddressTextField);
			directionBottomView.Add(_destinationLabel);
			directionBottomView.Add(directionImageView);

			var directionBottomViews = new DictionaryViews()
			{
				{"currentAddressTextField", _currentAddressTextField},
				{"destinationLabel", _destinationLabel},
				{"directionImageView", directionImageView},
			};

			directionBottomView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|-5-[currentAddressTextField]-5-[destinationLabel(currentAddressTextField)]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionBottomViews)
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-5-[directionImageView]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, directionBottomViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-5-[directionImageView(10)]-5-[currentAddressTextField]-5-|", NSLayoutFormatOptions.AlignAllTop, null, directionBottomViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-20-[destinationLabel]-5-|", NSLayoutFormatOptions.AlignAllTop, null, directionBottomViews))
				.ToArray());
			
			/* -- directions bottom view -- */

			/* -- directions panel view -- */

			var infoView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};

			_progressView = new UIActivityIndicatorView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Color = UIColor.White,
			};

			Add(_mapView);
			Add(_progressView);
			Add(directionView);
			Add(infoView);

			var views = new DictionaryViews()
			{
				{"mapView", _mapView},
				{"progressView", _progressView},
				{"infoView", infoView},
				{"directionView", directionView},
			};

			var coords = new CLLocationCoordinate2D(48.857, 2.351);
			var span = new MKCoordinateSpan(MilesToLatitudeDegrees(20), MilesToLongitudeDegrees(20, coords.Latitude));
			_mapView.Region = new MKCoordinateRegion(coords, span);

			var segmentControl = new UISegmentedControl()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};
			segmentControl.Layer.CornerRadius = 0;
			segmentControl.Layer.BorderWidth = 1.5f;
			segmentControl.Layer.BorderColor = UIColor.Clear.CGColor;
			segmentControl.BackgroundColor = UIColor.Clear.FromHex("#f9f9f9");
			segmentControl.TintColor = UIColor.Clear;

			var selectedTextAttributes = new UITextAttributes();
			selectedTextAttributes.TextColor = UIColor.Clear.FromHex("#c14d60");
			var unSelectedTextAttributes = new UITextAttributes();
			unSelectedTextAttributes.TextColor = UIColor.Clear.FromHex("#7a7a7a");

			//set the new text color attributes on the selected segment's title
			segmentControl.SetTitleTextAttributes(selectedTextAttributes, UIControlState.Selected);
			segmentControl.SetTitleTextAttributes(unSelectedTextAttributes, UIControlState.Normal);

			segmentControl.InsertSegment("Contact", 0, false);
			segmentControl.InsertSegment("Feedback", 1, false);
			segmentControl.SelectedSegment = 0;
			segmentControl.ValueChanged += (sender, e) =>
			{
				var selectedSegmentId = (sender as UISegmentedControl).SelectedSegment;
				switch (selectedSegmentId)
				{
					case 0:
                        _infoTableView.Hidden = false;
						_feedbackView.Hidden = true;
					break;
					case 1:
						_infoTableView.Hidden = true;
						_feedbackView.Hidden = false;
					break;
				}
			} ;

			/* -- feedback view -- */

			_serialDisposable = new SerialDisposable();

			_feedbackView = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Hidden = true,
			};

			var ratingLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Font = UIFont.FromName("Helvetica", 24f),
				TextAlignment = UITextAlignment.Right,
				Text = "4.5",
				TextColor = UIColor.Clear.FromHex("#c14d60")
			};

			/* -- ratings panel view -- */

			var ratingsPanel = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
			};

			var numberOfReviewsLabel = new UILabel()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Font = UIFont.FromName("Helvetica", 14f),
                TextColor = UIColor.Clear.FromHex("#666666"),
				TextAlignment = UITextAlignment.Left,
				Text = "25 Reviews",
			};

			ratingsPanel.Add(numberOfReviewsLabel);

			var ratingsPanelViews = new DictionaryViews()
			{
				{"numberOfReviewsLabel", numberOfReviewsLabel},
			};

			ratingsPanel.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|-20-[numberOfReviewsLabel]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, ratingsPanelViews)
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-5-[numberOfReviewsLabel]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, ratingsPanelViews))
				.ToArray());

			/* -- ratings panel view -- */

			var verticalSeperator = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = UIColor.Clear.FromHex("#dddddd")
			};

			var horizontalSeperator = new UIView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = UIColor.Clear.FromHex("#dddddd")
			};

			_feedbackTableView = new UITableView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false
			};

            _infoTableView = new UITableView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false
			};
            _infoTableView.RegisterClassForCellReuse(typeof(HeaderTableViewCell), HeaderTableViewCell.Key);
			_infoTableView.RegisterClassForCellReuse(typeof(ContactTableViewCell), ContactTableViewCell.Key);

			_feedbackView.Add(ratingLabel);
			_feedbackView.Add(ratingsPanel);
			_feedbackView.Add(verticalSeperator);
			_feedbackView.Add(horizontalSeperator);
			_feedbackView.Add(_feedbackTableView);

			var feedbackViews = new DictionaryViews()
			{
				{"ratingLabel", ratingLabel},
				{"ratingsPanel", ratingsPanel},
				{"verticalSeperator", verticalSeperator},
				{"horizontalSeperator", horizontalSeperator},
				{"feedbackTableView", _feedbackTableView},
			};

			/* -- feedback view -- */

			infoView.Add(segmentControl);
			infoView.Add(_infoTableView);
			infoView.Add(_feedbackView);

			var infoViews = new DictionaryViews()
			{
				{"segmentControl", segmentControl},
				{"infoTableView", _infoTableView},
				{"feedbackView", _feedbackView},
			};

			View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[mapView][infoView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
				// map view is a bit smaller than info view
				.Concat(new NSLayoutConstraint[] { NSLayoutConstraint.Create(_mapView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, infoView, NSLayoutAttribute.Height, 0.9f, 0) })
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-5-[directionView]", NSLayoutFormatOptions.DirectionLeftToRight, null, views))
				// direction view is a third of the map view
				.Concat(new NSLayoutConstraint[] { NSLayoutConstraint.Create(directionView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, _mapView, NSLayoutAttribute.Height, 0.35f, 0) })
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[mapView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-5-[directionView]-5-|", NSLayoutFormatOptions.AlignAllTop, null, views))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[infoView]|", NSLayoutFormatOptions.AlignAllTop, null, views))
                .ToArray());
			
			infoView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|[segmentControl(35)][infoTableView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, infoViews)	
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|[segmentControl(35)][feedbackView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, infoViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[segmentControl]|", NSLayoutFormatOptions.AlignAllTop, null, infoViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[infoTableView]|", NSLayoutFormatOptions.AlignAllTop, null, infoViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[feedbackView]|", NSLayoutFormatOptions.AlignAllTop, null, infoViews))
				.ToArray());

			_feedbackView.AddConstraints(
				NSLayoutConstraint.FromVisualFormat("V:|-5-[ratingLabel(40)]-5-[horizontalSeperator(1)]-5-[feedbackTableView]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, feedbackViews)
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-5-[verticalSeperator(40)]-5-[horizontalSeperator(1)]-5-[feedbackTableView]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, feedbackViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("V:|-5-[ratingsPanel(40)]-5-[horizontalSeperator(1)]-5-[feedbackTableView]-5-|", NSLayoutFormatOptions.DirectionLeftToRight, null, feedbackViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-10-[ratingLabel]-5-[verticalSeperator(1)]-5-[ratingsPanel(ratingLabel)]-10-|", NSLayoutFormatOptions.AlignAllTop, null, feedbackViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-10-[horizontalSeperator]-10-|", NSLayoutFormatOptions.AlignAllTop, null, feedbackViews))
				.Concat(NSLayoutConstraint.FromVisualFormat("H:|[feedbackTableView]|", NSLayoutFormatOptions.AlignAllTop, null, feedbackViews))
				.ToArray());

			this.Bind(ViewModel, x => x.CompanyName, x => x.Title);
			this.Bind(ViewModel, x => x.DestinationAddress, x => x._destinationLabel.Text);
			this.Bind(ViewModel, x => x.CurrentAddress, x => x._currentAddressTextField.Text);

			this.Bind(ViewModel, x => x.Phone, x => x._phoneLabel.Text);
			this.Bind(ViewModel, x => x.Email, x => x._emailLabel.Text);
			this.Bind(ViewModel, x => x.Website, x => x._websiteLabel.Text);

			this.BindCommand(ViewModel, x => x.DrivingCommand, x => x._buttons[1]);
			this.BindCommand(ViewModel, x => x.TransitCommand, x => x._buttons[2]);
			this.BindCommand(ViewModel, x => x.WalkingCommand, x => x._buttons[3]);

			this.WhenActivated(d =>
			{
				
			});
		}

        /// <summary>
        /// Posts the load subscriptions.
        /// </summary>
        /// <returns>The load subscriptions.</returns>
        public override IEnumerable<IDisposable> LoadSubscriptions()
        {
            yield return ViewModel.WhenAnyValue(vm => vm.Feedbacks).BindTo<FeedbackListItemViewModel,
                FeedbackListItemTableViewCell>(_feedbackTableView, 80, cell => cell.Initialize());

            yield return ViewModel.WhenAnyValue(vm => vm.Infos)
                .Select((IReactiveList<TetrixViewModelBase> list) =>
                {
                    return list.GroupBy(model => (float)model.Height)
                        .Select(typeGroup =>
		                {
                            var collection = new ReactiveList<TetrixViewModelBase>();
                            collection.AddRange(typeGroup);

		                    return new TableSectionInformation<TetrixViewModelBase, BaseTableViewCell>(collection, (model) =>
	                           {
	                               var cellIndentifier = "";

	                               TypeSwitch.On(model)
	                                   .Case((HeaderListItemViewModel m) => cellIndentifier = HeaderTableViewCell.Key)
	                                   .Case((ContactListItemViewModel m) => cellIndentifier = ContactTableViewCell.Key);

	                               return new NSString(cellIndentifier);
	                           }, typeGroup.Key, cell => cell.Initialize());
		                }).ToList();
                }).BindTo (_infoTableView);
				
			yield return this.WhenAnyValue(v => v._tableSource.ElementSelected)
				.Subscribe(x =>
				{
					// when ever this value is updated we want to make sure we only ever have
					// the latest subscription
					_serialDisposable.Disposable = x.Subscribe(vm => ViewModel.SelectFeedbackAsync());
				});

			yield return this.WhenAnyValue(v => v._infoTableSource.ElementSelected)
				.Subscribe(x =>
				{
					// when ever this value is updated we want to make sure we only ever have
					// the latest subscription
					_serialDisposable.Disposable = x.Subscribe(vm => ViewModel.SelectContactAsync());
				});

			yield return Observable.FromEventPattern<PathUpdateEventArgs>(ViewModel, "PathUpdate")
					.Window(() => Observable.Interval(TimeSpan.FromSeconds(2)))
					.SelectMany(x => x.Take(1))
				    .SubscribeOn(ViewModel.Scheduler)
					.Subscribe(e =>
					{
						var pathArgs = e.EventArgs;

						var latDif = pathArgs.StartCoordinate.Latitude - pathArgs.EndCoordinate.Latitude;
						var lonDif = pathArgs.StartCoordinate.Longitude - pathArgs.EndCoordinate.Longitude;
						var midLat = latDif / 2 + pathArgs.EndCoordinate.Latitude;
						var midLon = lonDif / 2 + pathArgs.EndCoordinate.Longitude;

						double dist = Math.Sqrt(latDif * latDif + lonDif * lonDif) * 90;

						var polyline = new List<CLLocationCoordinate2D>();

						foreach (var pathPoint in pathArgs.Path)
						{
							polyline.Add(new CLLocationCoordinate2D(pathPoint.Latitude,
																	pathPoint.Longitude));
						}

						// Must update the UI on the main thread
						// set map center
						var mapCenter = new CLLocationCoordinate2D(midLat, midLon);
						var mapRegion = MKCoordinateRegion.FromDistance(mapCenter, dist * 2000, dist * 2000);

						// if _mkPolyLine not null, remove then recreate and re add
						if (_mkPolyLine != null)
						{
							_mapView.RemoveOverlay(_mkPolyLine);
							_mkPolyLine?.Dispose();
						}

						_mkPolyLine = MKPolyline.FromCoordinates(polyline.ToArray());

						_mapView.CenterCoordinate = mapCenter;
						_mapView.Region = mapRegion;

						// add map overlay for path
						_mapView.AddOverlay(_mkPolyLine);
					});

			yield return ViewModel.WhenAnyValue(vm => vm.LocationLoading)
				.SubscribeOn(ViewModel.Scheduler)
				.Subscribe(loading =>
				{
					if (loading)
					{
						_progressView.StartAnimating();
					}
					else
					{
						_progressView.StopAnimating();
					}
				});

		}

		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear(bool animated)
		{
			base.Load();

			base.ViewWillAppear(animated);
		}

		/// <summary>
		/// Views the will disappear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillDisappear(bool animated)
		{
			base.DisposePage();

			base.ViewWillDisappear(animated);
		}

		/// <summary>
		/// Mileses to latitude degrees.
		/// </summary>
		/// <returns>The to latitude degrees.</returns>
		/// <param name="miles">Miles.</param>
		public double MilesToLatitudeDegrees(double miles)
		{
			double earthRadius = 3960.0; // in miles
			double radiansToDegrees = 180.0 / Math.PI;
			return (miles / earthRadius) * radiansToDegrees;
		}

		/// <summary>
		/// Mileses to longitude degrees.
		/// </summary>
		/// <returns>The to longitude degrees.</returns>
		/// <param name="miles">Miles.</param>
		/// <param name="atLatitude">At latitude.</param>
		public double MilesToLongitudeDegrees(double miles, double atLatitude)
		{
			double earthRadius = 3960.0; // in miles
			double degreesToRadians = Math.PI / 180.0;
			double radiansToDegrees = 180.0 / Math.PI;
			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (miles / radiusAtLatitude) * radiansToDegrees;
		}
	}
}
