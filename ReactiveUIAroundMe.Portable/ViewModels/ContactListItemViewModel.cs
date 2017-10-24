// --------------------------------------------------------------------------------------------------
//  <copyright file="ContactListItemViewModel.cs" company="Flush Arcade Pty Ltd.">
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
    /// Contact list item view model.
    /// </summary>
	public class ContactListItemViewModel : TetrixViewModelBase
	{
		#region Bindable

		/// <summary>
		/// The name of the company.
		/// </summary>
        private string _icon;

		/// <summary>
		/// Gets or sets the name of the company.
		/// </summary>
		/// <value>The name of the company.</value>
        public string Icon
		{
			get { return _icon; }
			set { this.RaiseAndSetIfChanged(ref _icon, value); }
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
        /// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ContactListItemViewModel"/> class.
        /// </summary>
        /// <param name="storage">Storage.</param>
        /// <param name="scheduler">Scheduler.</param>
        /// <param name="signalRClient">Signal RC lient.</param>
        /// <param name="log">Log.</param>
        /// <param name="applicationStateHandler">Application state handler.</param>
        /// <param name="webServiceController">Web service controller.</param>
        /// <param name="googleMapsWebServiceController">Google maps web service controller.</param>
        /// <param name="pathLocator">Path locator.</param>
        /// <param name="hostScreen">Host screen.</param>
        /// <param name="locationManager">Location manager.</param>
		public ContactListItemViewModel(ISQLiteStorage storage, IScheduler scheduler, ILogger log,
							 ApplicationStateHandler applicationStateHandler, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
							 IPathLocator pathLocator, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, googleMapsWebServiceController, 
                   pathLocator, hostScreen, locationManager)
		{
			Height = 40;
		}
	}
}