// --------------------------------------------------------------------------------------------------
//  <copyright file="FeedbackListItemViewModel.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2014 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.ViewModels
{
	using System.Windows.Input;
	using System.Reactive.Concurrency;
	using System;

	using ReactiveUI;

	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Logging;
	
	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable.UI;
	using ReactiveUIAroundMe.Portable.Location;

	/// <summary>
	/// Feedback list item view model.
	/// </summary>
	public class FeedbackListItemViewModel : TetrixViewModelBase
	{
		#region Bindable

		/// <summary>
		/// The name of the company.
		/// </summary>
		private string _companyName;

		/// <summary>
		/// Gets or sets the name of the company.
		/// </summary>
		/// <value>The name of the company.</value>
		public string CompanyName
		{
			get { return _companyName; }
			set { this.RaiseAndSetIfChanged(ref _companyName, value); }
		}

		/// <summary>
		/// The company route identifier.
		/// </summary>
		private string _companyRouteId;

		/// <summary>
		/// Gets or sets the company route identifier.
		/// </summary>
		/// <value>The company route identifier.</value>
		public string CompanyRouteId
		{
			get { return _companyRouteId; }
			set { this.RaiseAndSetIfChanged(ref _companyRouteId, value); }
		}

		/// <summary>
		/// The created.
		/// </summary>
		private DateTime _created;

		/// <summary>
		/// Gets or sets the created.
		/// </summary>
		/// <value>The created.</value>
		public DateTime Created
		{
			get { return _created; }
			set { this.RaiseAndSetIfChanged(ref _created, value); }
		}

		/// <summary>
		/// The home phone.
		/// </summary>
		private string _homePhone;

		/// <summary>
		/// Gets or sets the home phone.
		/// </summary>
		/// <value>The home phone.</value>
		public string HomePhone
		{
			get { return _homePhone; }
			set { this.RaiseAndSetIfChanged(ref _homePhone, value); }
		}

		/// <summary>
		/// The work phone.
		/// </summary>
		private string _workPhone;

		/// <summary>
		/// Gets or sets the work phone.
		/// </summary>
		/// <value>The work phone.</value>
		public string WorkPhone
		{
			get { return _workPhone; }
			set { this.RaiseAndSetIfChanged(ref _workPhone, value); }
		}

		/// <summary>
		/// The mobile phone.
		/// </summary>
		private string _mobilePhone;

		/// <summary>
		/// Gets or sets the mobile phone.
		/// </summary>
		/// <value>The mobile phone.</value>
		public string MobilePhone
		{
			get { return _mobilePhone; }
			set { this.RaiseAndSetIfChanged(ref _mobilePhone, value); }
		}

		/// <summary>
		/// The email.
		/// </summary>
		private string _email;

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
		public string Email
		{
			get { return _email; }
			set { this.RaiseAndSetIfChanged(ref _email, value); }
		}

		/// <summary>
		/// The eReactiveUIAroundMe number.
		/// </summary>
		private int _eReactiveUIAroundMeNumber;

		/// <summary>
		/// Gets or sets the eReactiveUIAroundMe number.
		/// </summary>
		/// <value>The eReactiveUIAroundMe number.</value>
		public int EReactiveUIAroundMeNumber
		{
			get { return _eReactiveUIAroundMeNumber; }
			set { this.RaiseAndSetIfChanged(ref _eReactiveUIAroundMeNumber, value); }
		}

		/// <summary>
		/// The eReactiveUIAroundMe version.
		/// </summary>
		private int _eReactiveUIAroundMeVersion;

		/// <summary>
		/// Gets or sets the eReactiveUIAroundMe version.
		/// </summary>
		/// <value>The eReactiveUIAroundMe version.</value>
		public int EReactiveUIAroundMeVersion
		{
			get { return _eReactiveUIAroundMeVersion; }
			set { this.RaiseAndSetIfChanged(ref _eReactiveUIAroundMeVersion, value); }
		}

		/// <summary>
		/// The geo location retrieved.
		/// </summary>
		private bool _geoLocationRetrieved;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.FeedbackListItemViewModel"/>
		/// geo location retrieved.
		/// </summary>
		/// <value><c>true</c> if geo location retrieved; otherwise, <c>false</c>.</value>
		public bool GeoLocationRetrieved
		{
			get { return _geoLocationRetrieved; }
			set { this.RaiseAndSetIfChanged(ref _geoLocationRetrieved, value); }
		}

		/// <summary>
		/// The identifier.
		/// </summary>
		private string _id;

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id
		{
			get { return _id; }
			set { this.RaiseAndSetIfChanged(ref _id, value); }
		}

		/// <summary>
		/// The image identifier.
		/// </summary>
		private string _imageId;

		/// <summary>
		/// Gets or sets the image identifier.
		/// </summary>
		/// <value>The image identifier.</value>
		public string ImageId
		{
			get { return _imageId; }
			set { this.RaiseAndSetIfChanged(ref _imageId, value); }
		}

		/// <summary>
		/// The info available.
		/// </summary>
		private bool _infoAvailable;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.FeedbackListItemViewModel"/>
		/// info available.
		/// </summary>
		/// <value><c>true</c> if info available; otherwise, <c>false</c>.</value>
		public bool InfoAvailable
		{
			get { return _infoAvailable; }
			set { this.RaiseAndSetIfChanged(ref _infoAvailable, value); }
		}

		/// <summary>
		/// The sequence.
		/// </summary>
		private int _sequence;

		/// <summary>
		/// Gets or sets the sequence.
		/// </summary>
		/// <value>The sequence.</value>
		public int Sequence
		{
			get { return _sequence; }
			set { this.RaiseAndSetIfChanged(ref _sequence, value); }
		}

		/// <summary>
		/// The street.
		/// </summary>
		private string _street;

		/// <summary>
		/// Gets or sets the street.
		/// </summary>
		/// <value>The street.</value>
		public string Street
		{
			get { return _street; }
			set
			{
				this.RaiseAndSetIfChanged(ref _street, value);
				this.RaisePropertyChanged("Address");
			}
		}

		/// <summary>
		/// The suburb.
		/// </summary>
		private string _suburb;

		/// <summary>
		/// Gets or sets the suburb.
		/// </summary>
		/// <value>The suburb.</value>
		public string Suburb
		{
			get { return _suburb; }
			set { 
				this.RaiseAndSetIfChanged(ref _suburb, value);
				this.RaisePropertyChanged("Address");
			}
		}

		/// <summary>
		/// The state.
		/// </summary>
		private string _state;

		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		/// <value>The state.</value>
		public string State
		{
			get { return _state; }
			set
			{
				this.RaiseAndSetIfChanged(ref _state, value);
				this.RaisePropertyChanged("Address");
			}
		}

		/// <summary>
		/// The city.
		/// </summary>
		private string _city;

		/// <summary>
		/// Gets or sets the city.
		/// </summary>
		/// <value>The city.</value>
		public string City
		{
			get { return _city; }
			set
			{
				this.RaiseAndSetIfChanged(ref _city, value);
				this.RaisePropertyChanged("Address");
			}
		}

		/// <summary>
		/// The postcode.
		/// </summary>
		private string _postcode;

		/// <summary>
		/// Gets or sets the postcode.
		/// </summary>
		/// <value>The postcode.</value>
		public string Postcode
		{
			get { return _postcode; }
			set
			{
				this.RaiseAndSetIfChanged(ref _postcode, value);
				this.RaisePropertyChanged("Address");
			}
		}

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		public string Address
		{
			get 
			{
				return string.Format("{0}, {1}, {2}, {3}",
				                     Street, City, State, Postcode); 
			}
		}

		/// <summary>
		/// The distance.
		/// </summary>
		private decimal _distance;

		/// <summary>
		/// Gets or sets the distance.
		/// </summary>
		/// <value>The distance.</value>
		public decimal Distance
		{
			get { return _distance; }
			set
			{
				this.RaiseAndSetIfChanged(ref _distance, value);
				this.RaisePropertyChanged("DistanceDisplay");
			}
		}

		/// <summary>
		/// Gets or sets the postcode.
		/// </summary>
		/// <value>The postcode.</value>
		public string DistanceDisplay
		{
			get {
				return string.Format("{0} kms");
			}
		}

		/// <summary>
		/// The latitude.
		/// </summary>
		private decimal _latitude;

		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		/// <value>The latitude.</value>
		public decimal Latitude
		{
			get { return _latitude; }
			set { this.RaiseAndSetIfChanged(ref _latitude, value); }
		}

		/// <summary>
		/// The longitude.
		/// </summary>
		private decimal _longitude;

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		/// <value>The longitude.</value>
		public decimal Longitude
		{
			get { return _longitude; }
			set { this.RaiseAndSetIfChanged(ref _longitude, value); }
		}

		/// <summary>
		/// The distance.
		/// </summary>
		private double _bearing;

		/// <summary>
		/// Gets or sets the distance.
		/// </summary>
		/// <value>The distance.</value>
		public double Bearing
		{
			get { return _bearing; }
			set { this.RaiseAndSetIfChanged(ref _bearing, value); }
		}

		#endregion

		#region TetrixViewModelBase Implementation

		public override object CellId
		{
			get
			{
				return Title;
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.ViewModels.TileViewModel"/> class.
		/// </summary>
		/// <param name="signalRClient">Signal RC lient.</param>
		public FeedbackListItemViewModel(ISQLiteStorage storage, IScheduler scheduler, ILogger log,
							 ApplicationStateHandler applicationStateHandler, WebServiceController webServiceController,
							 GoogleMapsWebServiceController googleMapsWebServiceController, IPathLocator pathLocator, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, 
			       googleMapsWebServiceController, pathLocator, hostScreen, locationManager)
		{
			Height = 200;
		}
	}
}