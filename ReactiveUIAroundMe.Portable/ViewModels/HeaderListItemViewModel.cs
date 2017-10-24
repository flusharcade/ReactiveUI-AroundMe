// --------------------------------------------------------------------------------------------------
//  <copyright file="HeaderListItemViewModel.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2014 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.ViewModels
{
	using System.Reactive.Concurrency;

	using ReactiveUI;

	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Logging;
	
	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable.UI;
	using ReactiveUIAroundMe.Portable.Location;

	/// <summary>
    /// Header list item view model.
    /// </summary>
	public class HeaderListItemViewModel : TetrixViewModelBase
	{
		#region Bindable

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
		public HeaderListItemViewModel(ISQLiteStorage storage, IScheduler scheduler, ILogger log,
							 ApplicationStateHandler applicationStateHandler, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
							 IPathLocator pathLocator, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, googleMapsWebServiceController, 
                   pathLocator, hostScreen, locationManager)
		{
			Height = 25;
		}
	}
}