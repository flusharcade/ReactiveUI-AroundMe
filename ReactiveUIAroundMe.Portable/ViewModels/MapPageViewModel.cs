// --------------------------------------------------------------------------------------------------
//  <copyright file="MapPageViewModel.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.ViewModels
{
	using System;
	using System.Reactive.Concurrency;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
	using System.Reactive;
	using System.Runtime.Serialization;
	using System.Collections.ObjectModel;
	using System.Collections.Generic;
	using System.Globalization;

	using ReactiveUI;

	using Newtonsoft.Json;

	using ReactiveUIAroundMe.Portable;
	
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable.Logging;
	using ReactiveUIAroundMe.Portable.UI;
	using ReactiveUIAroundMe.Portable.Location;
	using ReactiveUIAroundMe.Portable.Models;
	using ReactiveUIAroundMe.Portable.Enums;

	/// <summary>
	/// Map page view model.
	/// </summary>
	public class MapPageViewModel : ViewModelBase
	{
        /// <summary>
        /// The icons.
        /// </summary>
        private string[] icons = { "phone", "email", "web" };

		/// <summary>
		/// Occurs when position update.
		/// </summary>
		public event EventHandler<PathUpdateEventArgs> PathUpdate;

		/// <summary>
		/// The name of the company.
		/// </summary>
		private GoogleMapsTravelModes _currentTravelMode;

		/// <summary>
		/// The end coordinate.
		/// </summary>
		private GeoCoordinate _endCoordinate;

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
		/// The destination address.
		/// </summary>
		private string _destinationAddress;

		/// <summary>
		/// Gets or sets the destination address.
		/// </summary>
		/// <value>The destination address.</value>
		public string DestinationAddress
		{
			get { return _destinationAddress; }
			set { this.RaiseAndSetIfChanged(ref _destinationAddress, value); }
		}

		//
		private string _phone;

		//
		public string Phone
		{
			get { return _phone; }
			set { this.RaiseAndSetIfChanged(ref _phone, value); }
		}

		//
		private string _email;

		//
		public string Email
		{
			get { return _email; }
			set { this.RaiseAndSetIfChanged(ref _email, value); }
		}

		//
		private string _website;

		//
		public string Website
		{
			get { return _website; }
			set { this.RaiseAndSetIfChanged(ref _website, value); }
		}

		/// <summary>
		/// Gets or sets the search results.
		/// </summary>
		/// <value>The search results.</value>
		[DataMember]
		public IReactiveList<FeedbackListItemViewModel> Feedbacks { get; set; }

		/// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>The contacts.</value>
		[DataMember]
		public IReactiveList<TetrixViewModelBase> Infos { get; set; }

		/// <summary>
		/// Gets or sets the login command.
		/// </summary>
		/// <value>The login command.</value>
		[DataMember]
		public ReactiveCommand FeedbackSelectCommand { get; set; }

		/// <summary>
		/// Gets or sets the login command.
		/// </summary>
		/// <value>The login command.</value>
		[DataMember]
		public ReactiveCommand TransitCommand { get; set; }

		/// <summary>
		/// Gets or sets the login command.
		/// </summary>
		/// <value>The login command.</value>
		[DataMember]
		public ReactiveCommand DrivingCommand { get; set; }

		/// <summary>
		/// Gets or sets the login command.
		/// </summary>
		/// <value>The login command.</value>
		[DataMember]
		public ReactiveCommand WalkingCommand { get; set; }

		#endregion

		/// <summary>
		/// Gets the URL path segment.
		/// </summary>
		/// <value>The URL path segment.</value>
		public override string UrlPathSegment
		{
			get
			{
				return CompanyName;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.HomePageViewModel"/> class.
		/// </summary>
		/// <param name="signalRClient">Signal RC lient.</param>
		/// <param name="scheduler">Scheduler.</param>
		/// <param name="applicationStateHandler">Application state handler.</param>
		/// <param name="storage">Storage.</param>
		/// <param name="webServiceController">Web service controller.</param>
		/// <param name="log">Log.</param>
		/// <param name="device">Device.</param>
		public MapPageViewModel(IScheduler scheduler, ApplicationStateHandler applicationStateHandler, 
		                        ISQLiteStorage storage, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
		                        IPathLocator pathLocator, ILogger log, IDevice device, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, googleMapsWebServiceController, pathLocator, hostScreen, locationManager)
		{
			Title = "Results";

			Feedbacks = new ReactiveList<FeedbackListItemViewModel>();
            Infos = new ReactiveList<TetrixViewModelBase>();
			Infos.Add(new HeaderListItemViewModel(storage, scheduler, log, applicationStateHandler,
														  webServiceController, googleMapsWebServiceController,
														  pathLocator, hostScreen, locationManager)
			{
				Title = "Contacts"
			});

            foreach (var icon in icons)
            {
                Infos.Add(new ContactListItemViewModel(storage, scheduler, 
                                                          log, applicationStateHandler, 
                                                          webServiceController, 
                                                          googleMapsWebServiceController,
                                                          pathLocator, hostScreen, locationManager)
                {
                    Icon = string.Format("{0}.png", icon)
                });
            }

            var canSelectFeedback = this.WhenAnyValue(
				vm => vm.Feedbacks,
			    vm => vm.IsLoading,
				(feedbacks, isLoading) => !feedbacks.IsEmpty && !isLoading);

			FeedbackSelectCommand = ReactiveCommand.CreateFromObservable(SelectFeedbackAsync,
				canSelectFeedback, Scheduler);

			var canSelectTravelMode = this.WhenAnyValue(vm => vm.IsLoading,
				(isLoading) => !isLoading);
			
			TransitCommand = ReactiveCommand.CreateFromObservable(() => CreatePathAsync(GoogleMapsTravelModes.Transit), 
				canSelectTravelMode, Scheduler);

			DrivingCommand = ReactiveCommand.CreateFromObservable(() => CreatePathAsync(GoogleMapsTravelModes.Driving),
				canSelectTravelMode, Scheduler);

			WalkingCommand = ReactiveCommand.CreateFromObservable(() => CreatePathAsync(GoogleMapsTravelModes.Walking),
				canSelectTravelMode, Scheduler);
		}

		/// <summary>
		/// Loads the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="parameters">Parameters.</param>
		public override async Task LoadAsync(IDictionary<string, object> parameters)
		{
			if (parameters.ContainsKey("company-name"))
			{
				CompanyName = (string)parameters["company-name"];
			}

			if (parameters.ContainsKey("destination-address"))
			{
				DestinationAddress = (string)parameters["destination-address"];
			}

			if (parameters.ContainsKey("end-coordinate"))
			{
				_endCoordinate = (GeoCoordinate)parameters["end-coordinate"];
			}

			if (parameters.ContainsKey("phone"))
			{
				Phone = (string)parameters["phone"];
			}

			if (parameters.ContainsKey("email"))
			{
				Email = (string)parameters["email"];
			}

			if (parameters.ContainsKey("website"))
			{
				Website = (string)parameters["website"];
			}

			if (parameters.ContainsKey("location"))
			{
				CurrentLocation = (Location)parameters["location"];
			}

			// on load determine walking path between points
			//CreatePathAsync(GoogleMapsTravelModes.Walking);

			CurrentAddress = await LocationManager.GetLocationAddress(CurrentLocation.Latitude,
			                                                   CurrentLocation.Longitude);

			await base.LoadAsync(parameters);
		}

		/// <summary>
		/// Creates the polyline.
		/// </summary>
		/// <returns>The polyline.</returns>
		/// <param name="encodedPoints">Encoded points.</param>
		private List<GeoCoordinate> CreatePath(string encodedPoints)
		{
			if (!string.IsNullOrEmpty(encodedPoints))
			{
				var polyline = new List<GeoCoordinate>();

				char[] polylineChars = encodedPoints.ToCharArray();

				int index = 0;
				int currentLat = 0;
				int currentLng = 0;
				int next5bits;
				int sum;
				int shifter;

				while (index < polylineChars.Length)
				{
					// calculate next latitude
					sum = 0;
					shifter = 0;

					do
					{
						next5bits = (int)polylineChars[index++] - 63;
						sum |= (next5bits & 31) << shifter;
						shifter += 5;
					} 
                    while (next5bits >= 32 && index < polylineChars.Length);

					if (index >= polylineChars.Length)
						break;

					currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

					//calculate next longitude
					sum = 0;
					shifter = 0;

					do
					{
						next5bits = (int)polylineChars[index++] - 63;
						sum |= (next5bits & 31) << shifter;
						shifter += 5;
					} 
					while (next5bits >= 32 && index < polylineChars.Length);

					if (index >= polylineChars.Length && next5bits >= 32)
					{
						break;
					}

					currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

					polyline.Add(new GeoCoordinate(currentLat / 1E5, currentLng / 1E5));
				}

				return polyline;
			}

			return null;
		}

		/// <summary>
		/// Creates the path async.
		/// </summary>
		/// <returns>The path async.</returns>
		public IObservable<Unit> CreatePathAsync(GoogleMapsTravelModes travelMode)
		{
			IsLoading = true;
			IsError = false;

			_currentTravelMode = travelMode;

            var startCoordinate = new GeoCoordinate(CurrentLocation.Latitude, CurrentLocation.Longitude);

			return GoogleMapsWebServiceController
				.GetDirectionsPolyLine(startCoordinate, _endCoordinate, _currentTravelMode)
				.ObserveOn(this.Scheduler)
				.Catch<GoogleGeocodeContract, Exception>(error =>
				{
					DidException(error, "Select failed using call to web service GetEReactiveUIAroundMeById");
					return Observable.Empty<GoogleGeocodeContract>();
				})
				.Do(geocode =>
				{
					IsLoading = false;

					var routes = geocode.Routes;
					var route = routes?.Length > 0 ? routes[0] : null;

					if (route != null)
					{
						var polyline = route.Bounds;

						var path = CreatePath(route.OverviewPolyline.Points);

						// update new path
						PathUpdate?.Invoke(this, new PathUpdateEventArgs()
						{
							StartCoordinate = startCoordinate,
							EndCoordinate = _endCoordinate,
							Path = path,
						});
					}

				}).Select(x => Unit.Default);
		}

		/// <summary>
		/// Creates the path async.
		/// </summary>
		/// <returns>The path async.</returns>
		public IObservable<Unit> SelectFeedbackAsync()
		{
			IsLoading = true;
			IsError = false;

			return GoogleMapsWebServiceController
				.GetDirectionsPolyLine(null, null, _currentTravelMode)
				.ObserveOn(this.Scheduler)
				.Catch<object, Exception>(error =>
				{
					DidException(error, "Select failed using call to web service GetEReactiveUIAroundMeById");
					return Observable.Empty<AuthContract>();
				})
				.Do(geocode =>
				{
					IsLoading = false;


				}).Select(x => Unit.Default);
		}

		/// <summary>
		/// Creates the path async.
		/// </summary>
		/// <returns>The path async.</returns>
		public IObservable<Unit> SelectContactAsync()
		{
			IsLoading = true;
			IsError = false;

            return Observable.Empty<Unit>();
		}
	}
}