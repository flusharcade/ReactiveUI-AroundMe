// --------------------------------------------------------------------------------------------------
//  <copyright file="HomePageViewModel.cs" company="Flush Arcade Pty Ltd.">
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
	using System.Linq;

	using ReactiveUI;

	using ReactiveUIAroundMe.Portable;
	
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable.Logging;
	using ReactiveUIAroundMe.Portable.Location;
	using ReactiveUIAroundMe.Portable.UI;

	/// <summary>
	/// Home page view model.
	/// </summary>
	public class HomePageViewModel : ViewModelBase
	{
		#region Constants

		/// <summary>
		/// The max range for kilometers
		/// </summary>
		public const double MAX_RANGE_KM = 20.1;

		/// <summary>
		/// The default range for kilometers
		/// </summary>
		public const double DEFAULT_RANGE_KM = 5.0;

		#endregion

		#region Private Properties

		/// <summary>
		/// The web service controller.
		/// </summary>
		private WebServiceController _webServiceController;

		/// <summary>
		/// The device.
		/// </summary>
		private IDevice _device;

		/// <summary>
		/// My locale.
		/// </summary>
		private string myLocale = string.Empty;

		/// <summary>
		/// The unit of measurement.
		/// </summary>
		private int unitOfMeasurement;

		/// <summary>
		/// The seconds since last action.
		/// </summary>
		private int secondsSinceLastAction;

		/// <summary>
		/// The int property.
		/// </summary>
		private string intProperty = "5";

		/// <summary>
		/// The address part1.
		/// </summary>
		private string addressPart1 = "Cronulla";

		/// <summary>
		/// The address part2.
		/// </summary>
		private string addressPart2 = "17 Wolger Street, Como West";

		/// <summary>
		/// The address part3.
		/// </summary>
		private string addressPart3 = "NSW Australia 2226";

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the login command.
		/// </summary>
		/// <value>The login command.</value>
		[DataMember]
		public ReactiveCommand SearchCommand { get; set; }

		/// <summary>
		/// Gets the URL path segment.
		/// </summary>
		/// <value>The URL path segment.</value>
		public override string UrlPathSegment
		{
			get
			{
				return "Home";
			}
		}

		#endregion

		/// <summary>
		/// Occurs when flip.
		/// </summary>
		public event EventHandler Flip;

        /// <summary>
        /// Occurs when flip reset.
        /// </summary>
        public event EventHandler FlipReset;

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
		public HomePageViewModel(IScheduler scheduler, ApplicationStateHandler applicationStateHandler, 
		                         ISQLiteStorage storage, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
		                         IPathLocator pathLocator, ILogger log, IDevice device, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, googleMapsWebServiceController, pathLocator, hostScreen, 
			       locationManager)
		{
			Title = "Welcome";

			_webServiceController = webServiceController;
			_device = device;

			scheduler.ScheduleAsync((arg1, arg2) => SetupSQLite());
			scheduler.ScheduleAsync((arg1, arg2) => Load());

			var canSearch = this.WhenAnyValue(
				vm => vm.CurrentLocation,
				(location) => location.Timestamp != default(DateTimeOffset));

			SearchCommand = ReactiveCommand.CreateFromObservable(SearchAsync, canSearch, scheduler);
		}

        /// <summary>
        /// Resets the actions.
        /// </summary>
        public void ResetActions()
        {
            secondsSinceLastAction = 0;

            FlipReset?.Invoke(this, EventArgs.Empty);
        }

		/// <summary>
		/// Loads the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="parameters">Parameters.</param>
		public override Task LoadAsync(IDictionary<string, object> parameters)
		{
			UpdateLocation();

			return base.LoadAsync(parameters);
		}

		/// <summary>
		/// Posts the load subscriptions.
		/// </summary>
		/// <returns>The load subscriptions.</returns>
		public override IEnumerable<IDisposable> PostLoadSubscriptions()
		{
			yield return LocationManager.LocationUpdates
				.Subscribe(NotifyLocationUpdate);

			yield return LocationManager.AddressUpdates
                .Subscribe(NotifyAddressUpdate);

			yield return Scheduler.SchedulePeriodic(TimeSpan.FromSeconds(1),
				() =>
				{
					secondsSinceLastAction++;

					// if user has left screen for 10 seconds without any action flip buttons
					if (secondsSinceLastAction % 45 == 0)
					{
						Flip?.Invoke(this, new EventArgs());
					}
				});
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		private async Task Load()
		{
			var identity = await Storage.GetObject<IdentityStorable>(StorableKeys.Identity.ToString());
			if (identity != null)
			{
			}
		}

		/// <summary>
		/// Login this instance.
		/// </summary>
		private IObservable<Unit> SearchAsync()
		{
			var searchResultsPageViewModel = new SearchResultsPageViewModel(Scheduler,
				ApplicationStateHandler, Storage, WebServiceController, GoogleMapsWebServiceController, PathLocator, Log, _device,
				HostScreen, LocationManager);
			
			// would like to add a nicer control for reusing singletons in Splat
			HostScreen.Router.Navigate.Execute(searchResultsPageViewModel);

			secondsSinceLastAction = 0;

            IsLoading = true;
            IsError = false;

			var location = Locations.LastOrDefault();

            searchResultsPageViewModel.IsLoading = true;

            return _webServiceController
				.GetEReactiveUIAroundMesByLocation(location)
                .ObserveOn(this.Scheduler)
                .Catch<IEnumerable<EReactiveUIAroundMeListItemContract>, Exception>(error =>
				{
                    IsError = true;
                    DidException(error, "Error: Search failed, could not retrieve search results using GetEReactiveUIAroundMesByLocation.");
					return Observable.Empty<IEnumerable<EReactiveUIAroundMeListItemContract>>();
				})
                .Do(eReactiveUIAroundMes =>
                {
                    IsLoading = false;
					
					searchResultsPageViewModel.LoadAsync(new Dictionary<string, object>()
					{
						{"location", CurrentLocation},
						{"eReactiveUIAroundMes", eReactiveUIAroundMes},
					});

                }).Select(x => Unit.Default);
		}
	}
}