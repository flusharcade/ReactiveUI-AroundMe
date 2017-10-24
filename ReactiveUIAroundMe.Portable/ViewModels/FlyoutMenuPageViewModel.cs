// --------------------------------------------------------------------------------------------------
//  <copyright file="FlyoutMenuPageViewModel.cs" company="Flush Arcade Pty Ltd.">
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

	using ReactiveUI;

	using ReactiveUIAroundMe.Portable;
	
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable.Logging;
	using ReactiveUIAroundMe.Portable.UI;
	using ReactiveUIAroundMe.Portable.Location;

	using Newtonsoft.Json;

	/// <summary>
	/// Search results page view model.
	/// </summary>
	public class FlyoutMenuPageViewModel : ViewModelBase
	{
		/// <summary>
		/// The web service controller.
		/// </summary>
		private WebServiceController _webServiceController;

		/// <summary>
		/// The device.
		/// </summary>
		private IDevice _device;

		/// <summary>
		/// The identifier.
		/// </summary>
		private string _eReactiveUIAroundMeId;

        #region Bindable

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the login command.
		/// </summary>
		/// <value>The login command.</value>
		[DataMember]
		public ReactiveCommand LoginCommand { get; set; }

		/// <summary>
		/// Gets or sets the search results.
		/// </summary>
		/// <value>The search results.</value>
		[DataMember]
		public IReactiveList<EReactiveUIAroundMeListItemViewModel> Results { get; set; }

		/// <summary>
		/// Gets or sets the login command.
		/// </summary>
		/// <value>The login command.</value>
		[DataMember]
		public ReactiveCommand ResultSelectCommand { get; set; }

		#endregion

		/// <summary>
		/// Gets the URL path segment.
		/// </summary>
		/// <value>The URL path segment.</value>
		public override string UrlPathSegment
		{
			get
			{
				return "Results";
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
		public FlyoutMenuPageViewModel(IScheduler scheduler, ApplicationStateHandler applicationStateHandler, 
		                         ISQLiteStorage storage, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
		                         IPathLocator pathLocator, ILogger log, IDevice device, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, 
			       googleMapsWebServiceController, pathLocator, hostScreen, locationManager)
		{
			Title = "Results";

			Results = new ReactiveList<EReactiveUIAroundMeListItemViewModel>();

			_webServiceController = webServiceController;
			_device = device;

			var canSelect = this.WhenAnyValue(vm => vm.Results,
				(results) => !results.IsEmpty && !IsLoading);

			ResultSelectCommand = ReactiveCommand.CreateFromObservable(SelectAsync,
				canSelect, Scheduler);
		}

		/// <summary>
		/// Loads the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="parameters">Parameters.</param>
		public override Task LoadAsync(IDictionary<string, object> parameters)
		{
			var resultsJson = "[{\"city\":\"Melbourne\",\"companyName\":null,\"companyRouteId\":null,\"created\":\"0001-01-01\",\"email\":null,\"eReactiveUIAroundMeNumber\":0,\"eReactiveUIAroundMeVersion\":0,\"geoLocationRetrieved\":true,\"homePhone\":null,\"id\":\"193fb9f2-56f6-4bc5-af0c-7f09be82542d\",\"imageId\":null,\"infoAvailable\":true,\"mobilePhone\":null,\"postcode\":\"3000\",\"sequence\":0,\"street\":\"233-239 Collins St\",\"suburb\":\"Melbourne CBD\",\"workPhone\":null,\"distance\":12663.5,\"latitude\":-37.805753,\"longitude\":144.893158,\"bearing\":185.04143687199624},{\"city\":\"Sydney\",\"companyName\":null,\"companyRouteId\":null,\"created\":\"0001-01-01\",\"email\":null,\"eReactiveUIAroundMeNumber\":0,\"eReactiveUIAroundMeVersion\":0,\"geoLocationRetrieved\":true,\"homePhone\":null,\"id\":\"4f7e697c-fb03-460d-810e-b6d728cf572e\",\"imageId\":null,\"infoAvailable\":true,\"mobilePhone\":null,\"postcode\":\"2000\",\"sequence\":0,\"street\":\"45 Sussex St\",\"suburb\":\"Sydney CBD\",\"workPhone\":null,\"distance\":11962.1,\"latitude\":-33.882129,\"longitude\":151.044555,\"bearing\":278.49513599249724},{\"city\":\"\",\"companyName\":null,\"companyRouteId\":null,\"created\":\"0001-01-01\",\"email\":null,\"eReactiveUIAroundMeNumber\":0,\"eReactiveUIAroundMeVersion\":0,\"geoLocationRetrieved\":true,\"homePhone\":null,\"id\":\"171a9010-213a-4b49-bb82-6897df27b443\",\"imageId\":null,\"infoAvailable\":true,\"mobilePhone\":null,\"postcode\":\"4109\",\"sequence\":0,\"street\":\"661 Compton Rd\",\"suburb\":\"Sunnybank Hills\",\"workPhone\":null,\"distance\":11402.7,\"latitude\":-27.611425,\"longitude\":153.055487,\"bearing\":318.9898654866401},{\"city\":\"Brisbane\",\"companyName\":null,\"companyRouteId\":null,\"created\":\"0001-01-01\",\"email\":null,\"eReactiveUIAroundMeNumber\":0,\"eReactiveUIAroundMeVersion\":0,\"geoLocationRetrieved\":true,\"homePhone\":null,\"id\":\"45bb3ff8-35fb-4dc0-95ff-6661559a6026\",\"imageId\":null,\"infoAvailable\":true,\"mobilePhone\":null,\"postcode\":\"4000\",\"sequence\":0,\"street\":\"260 Queen St\",\"suburb\":\"Brisbane CBD\",\"workPhone\":null,\"distance\":11395.4,\"latitude\":-27.467938,\"longitude\":153.027441,\"bearing\":310.71024470313245},{\"city\":\"\",\"companyName\":null,\"companyRouteId\":null,\"created\":\"0001-01-01\",\"email\":null,\"eReactiveUIAroundMeNumber\":0,\"eReactiveUIAroundMeVersion\":0,\"geoLocationRetrieved\":true,\"homePhone\":null,\"id\":\"1e0515e1-f0e9-404b-bc23-e202a6449dd7\",\"imageId\":null,\"infoAvailable\":true,\"mobilePhone\":null,\"postcode\":\"6107\",\"sequence\":0,\"street\":\"1386 Albany Hwy\",\"suburb\":\"Canningtom\",\"workPhone\":null,\"distance\":3711.0,\"latitude\":31.220093,\"longitude\":-82.375584,\"bearing\":349.21875693510219}]";
			
			var eReactiveUIAroundMes = JsonConvert.DeserializeObject<IEnumerable<EReactiveUIAroundMeListItemContract>>(resultsJson);
			foreach (var eReactiveUIAroundMe in eReactiveUIAroundMes)
			{
				var eReactiveUIAroundMeModel = new EReactiveUIAroundMeListItemViewModel(Storage, Scheduler, Log,
																  ApplicationStateHandler, WebServiceController,
																  GoogleMapsWebServiceController, PathLocator, HostScreen, LocationManager);
				eReactiveUIAroundMeModel.Apply(eReactiveUIAroundMe);
				Results.Add(eReactiveUIAroundMeModel);
			}

			return base.LoadAsync(parameters);
		}

		/// <summary>
		/// Login this instance.
		/// </summary>
		public IObservable<Unit> SelectAsync()
		{
			IsLoading = true;
			IsError = false;

			HostScreen.Router.Navigate.Execute(new MapPageViewModel(Scheduler,
				ApplicationStateHandler, Storage, WebServiceController, GoogleMapsWebServiceController, PathLocator, 
			    Log, _device, HostScreen, LocationManager));

			return _webServiceController
				.GetEReactiveUIAroundMeById("")
				.ObserveOn(this.Scheduler)
				.Catch<object, Exception>(error =>
				{
					DidException(error, "Select failed using call to web service GetEReactiveUIAroundMeById");
					return Observable.Empty<AuthContract>();
				})
				.Do(auth =>
				{
					IsLoading = false;

					
				}).Select(x => Unit.Default);
		}
	}
}