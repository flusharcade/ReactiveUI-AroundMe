// --------------------------------------------------------------------------------------------------
//  <copyright file="TileViewModel.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2014 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
using ReactiveUIAroundMe.Portable.Location;

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

	/// <summary>
	/// Tile view model.
	/// </summary>
	public class TileViewModel : TetrixViewModelBase
	{
		#region Bindable

		/// <summary>
		/// The tile image.
		/// </summary>
		private string _tileImage;

		/// <summary>
		/// Gets or sets the tile image.
		/// </summary>
		/// <value>The tile image.</value>
		public string TileImage
		{
			get { return _tileImage; }
			set { this.RaiseAndSetIfChanged(ref _tileImage, value); }
		}

		/// <summary>
		/// The banner image.
		/// </summary>
		private string _bannerImage;

		/// <summary>
		/// Gets or sets the tile image.
		/// </summary>
		/// <value>The tile image.</value>
		public string BannerImage
		{
			get { return _bannerImage; }
			set { this.RaiseAndSetIfChanged(ref _bannerImage, value); }
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
		public TileViewModel(ISQLiteStorage storage, IScheduler scheduler, ILogger log,
							 ApplicationStateHandler applicationStateHandler, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
							 IPathLocator pathLocator, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler,
				  webServiceController, googleMapsWebServiceController, pathLocator, hostScreen, locationManager)
		{
			Height = 200;
		}
	}
}