// --------------------------------------------------------------------------------------------------
//  <copyright file="SearchResultsPageViewModel.cs" company="Flush Arcade Pty Ltd.">
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

	using Newtonsoft.Json;

	using ReactiveUI;

	using ReactiveUIAroundMe.Portable;
	
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable.Logging;
	using ReactiveUIAroundMe.Portable.UI;
	using ReactiveUIAroundMe.Portable.Location;
	using ReactiveUIAroundMe.Portable.Models;

	/// <summary>
	/// Search results page view model.
	/// </summary>
	public class SearchResultsPageViewModel : ViewModelBase
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
		public SearchResultsPageViewModel(IScheduler scheduler, ApplicationStateHandler applicationStateHandler, 
		                         ISQLiteStorage storage, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController, 
		                         IPathLocator pathLocator, ILogger log, IDevice device, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, 
			        googleMapsWebServiceController, pathLocator, hostScreen, locationManager)
		{
			Title = "Results";

			Results = new ReactiveList<EReactiveUIAroundMeListItemViewModel>();

			_webServiceController = webServiceController;
			_device = device;
		}

		/// <summary>
		/// Loads the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="parameters">Parameters.</param>
		public override Task LoadAsync(IDictionary<string, object> parameters)
		{
			if (parameters.ContainsKey("eReactiveUIAroundMes"))
			{
                var eReactiveUIAroundMes = parameters["eReactiveUIAroundMes"] as IEnumerable<EReactiveUIAroundMeListItemContract>;
				foreach (var eReactiveUIAroundMe in eReactiveUIAroundMes)
				{
					var eReactiveUIAroundMeModel = new EReactiveUIAroundMeListItemViewModel(Storage, Scheduler, Log,
																	  ApplicationStateHandler, WebServiceController,
																	  GoogleMapsWebServiceController, PathLocator, HostScreen, LocationManager);
					eReactiveUIAroundMeModel.Apply(eReactiveUIAroundMe);
					Results.Add(eReactiveUIAroundMeModel);
				}

                IsLoading = false;
			}

			if (parameters.ContainsKey("location"))
			{
				CurrentLocation = (Location)parameters["location"];
			}

			return base.LoadAsync(parameters);
		}

		/// <summary>
		/// Login this instance.
		/// </summary>
		public IObservable<Unit> SelectAsync(EReactiveUIAroundMeListItemViewModel selected)
		{
			IsLoading = true;
			IsError = false;

			// navigate to map page
			var mapPageViewModel = new MapPageViewModel(Scheduler,
				ApplicationStateHandler, Storage, WebServiceController, GoogleMapsWebServiceController, PathLocator, Log, _device,
				HostScreen, LocationManager);

			HostScreen.Router.Navigate.Execute(mapPageViewModel);

			mapPageViewModel.LoadAsync(new Dictionary<string, object>()
			{
				{"company-name", selected.CompanyName},
				{"destination-address", selected.Address},
				{"end-coordinate", new GeoCoordinate(Convert.ToDouble(selected.Latitude),
													 Convert.ToDouble(selected.Longitude))},
				{"location", CurrentLocation},
                {"phone", selected.HomePhone},
                {"email", selected.Email},
                {"website", selected.Website},
			}).ConfigureAwait(false);

			return _webServiceController
				.GetEReactiveUIAroundMeById(selected.Id)
				.ObserveOn(this.Scheduler)
				.Catch<object, Exception>(error =>
				{
					DidException(error, "Select failed using call to web service GetEReactiveUIAroundMeById");
					return Observable.Empty<AuthContract>();
				})
				.Do(auth =>
				{
					IsLoading = false;
				})
                .Select(x => Unit.Default);
		}
	}
}