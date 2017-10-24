using ReactiveUIAroundMe.Portable.Location;
namespace ReactiveUIAroundMe.Portable.ViewModels
{
	using System.Collections.Generic;
	using System.Reactive.Linq;
	using System.Reactive.Concurrency;
	using System.Linq;
	using System.Reactive;
	using System.Reactive.Disposables;
	using System;

	using ReactiveUI;

	
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.Logging;
	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable.UI;
	using ReactiveUIAroundMe.Portable.Extensions;
	using ReactiveUIAroundMe.Portable.Enums;

	/// <summary>
	/// Super admin page view model.
	/// </summary>
	public class SuperAdminPageViewModel : TetrixPageViewModelBase
	{
		/// <summary>
		/// The path locator.
		/// </summary>
		private readonly IPathLocator _pathLocator;

		/// <summary>
		/// The tile tiles.
		/// </summary>
		private string[] _tileTiles = new string[]
		{
			"MD Officials",
			"Teams",
			"Team Approver",
			"Match Day Manager",
			"Admin",
		};

		/// <summary>
		/// The tile images.
		/// </summary>
		private string[] _bannerImages = new string[]
		{
			"officials",
			"teams",
			"team_approver",
			"match_day_manager",
			"admin",
		};

		/// <summary>
		/// The tile images.
		/// </summary>
		private string[] _tileImages = new string[]
		{
			"profile_image.jpeg",
			"profile_image.jpeg",
			"profile_image.jpeg",
			"profile_image.jpeg",
			"profile_image.jpeg",
		};

		/// <summary>
		/// The tile navigation view models.
		/// </summary>
		private string[] _tileNavigationViewModels = new string[]
		{
			"OfficialsTabPageViewModel",
			"FixturesPageViewModel",
			"TeamApproverTabPageViewModel",
			"MatchDayManagerTabPageViewModel",
			"AdminTabPageViewModel",
		};

        /// <summary>
        /// 
        /// </summary>
        private Func<object, Unit> selection;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.SuperAdminPageViewModel"/> class.
        /// </summary>
        /// <param name="storage">Storage.</param>
        /// <param name="scheduler">Scheduler.</param>
        /// <param name="signalRClient">Signal RC lient.</param>
        /// <param name="log">Log.</param>
        /// <param name="applicationStateHandler">Application state handler.</param>
        /// <param name="webServiceController">Web service controller.</param>
        public SuperAdminPageViewModel(ISQLiteStorage storage, IScheduler scheduler, ILogger log,
							 ApplicationStateHandler applicationStateHandler, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
		                     IPathLocator pathLocator, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, googleMapsWebServiceController, pathLocator, 
			       hostScreen, locationManager)
		{
			_pathLocator = pathLocator;

			Title = "Super Admin";

            selection = (obj) =>
            {
                var parameters = (obj as TetrixViewModelBase)?.SelectParameters;

                object selectKey;
                parameters.TryGetValue("ViewModel", out selectKey);
                if (selectKey != null)
                {
                    HostScreen.Router.Navigate.Execute(new SuperAdminPageViewModel(Storage, Scheduler,
                                    Log, ApplicationStateHandler, WebServiceController, 
					                GoogleMapsWebServiceController, PathLocator, HostScreen, LocationManager));

                    var viewModelType = Type.GetType(string.Format("ReactiveUIAroundMe.Portable.ViewModels.{0}", selectKey));
                    var instance = (ViewModelBase)Activator.CreateInstance(viewModelType, 
                        new object[] { Storage, Scheduler,
                            Log, ApplicationStateHandler, WebServiceController,
                            PathLocator, HostScreen });

                    HostScreen.Router.Navigate.Execute(instance);
                };

                return Unit.Default;
            };

            InitSelectCommand((obj) => selection(obj));

			var tiles = _tileTiles.Select((title, index) =>
			{
				var tileModel = new TileViewModel(Storage, Scheduler, Log, 
                    ApplicationStateHandler, WebServiceController, GoogleMapsWebServiceController, PathLocator, HostScreen, LocationManager);
				tileModel.Title = _tileTiles[index];
				tileModel.BannerImage = _pathLocator.GetPath(_bannerImages[index], "jpg");
				tileModel.TileImage = _pathLocator.GetPath("profile_image", "jpeg");

				// hack: for mac until wet selectable cells working for collection views
				tileModel.InitSelectionCommand((obj) => selection(obj));
				tileModel.SelectParameters = new Dictionary<string, object>()
				{
					{"ViewModel", _tileNavigationViewModels[index]},
				};

				tileModel.UseXSpacing = true;
				tileModel.UseYSpacing = true;
				tileModel.Layout = LayoutType.Fifth;
				tileModel.Position = index;

				return tileModel;
			});

			Cells.AddRange(tiles);
		}
	}	
}