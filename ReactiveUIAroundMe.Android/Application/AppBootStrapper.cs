// --------------------------------------------------------------------------------------------------
//  <copyright file="AppDelegate.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Droid
{
	using System.Net;
	using System.Net.Http;
	using System.Reactive.Concurrency;
	using SQLite.Net.Interop;
	using SQLite.Net.Platform.XamarinAndroid;

	using ReactiveUI;

	using ReactiveUIAroundMe.Droid.Device;
	using ReactiveUIAroundMe.Droid.DataAccess;
	using ReactiveUIAroundMe.Droid.Views;
	using ReactiveUIAroundMe.Droid.Logging;
	using ReactiveUIAroundMe.Droid.Converters;
    using ReactiveUIAroundMe.Droid.Location;
	using ReactiveUIAroundMe.Droid.Helpers;

	using ReactiveUIAroundMe.Shared;

	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.UI;
	using ReactiveUIAroundMe.Portable.ViewModels;
	using ReactiveUIAroundMe.Portable.Resources;
	using ReactiveUIAroundMe.Portable.Location;

	using Splat;
    using ReactiveUIAroundMe.Shared.DataAccess;

    public class AppBootstrapper : ReactiveObject, IScreen
	{
		// The Router holds the ViewModels for the back stack. Because it's
		// in this object, it will be serialized automatically.
		public RoutingState Router { get; protected set; }

		/// <summary>
		/// The flyout menu view model.
		/// </summary>
		private FlyoutMenuPageViewModel _flyoutMenuViewModel;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Droid.AppBootstrapper"/> class.
		/// </summary>
		public AppBootstrapper()
		{
			Router = new RoutingState();
			Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));

			HttpClientHandler clientHandler = new HttpClientHandler(); //_isWindows ? new HttpClientHandler() : new NativeMessageHandler();
			clientHandler.UseCookies = false;
			clientHandler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

			// TODO: singletons, we want to pull resolved items from container, rather than outside declarations
			var sqliteplatform = new SQLitePlatformAndroid();
			var logger = new LoggerAndroid();
			var settings = new Settings();
			var userDefaults = new UserDefaults(settings);

			// temp until settings page added
			userDefaults.Host = Config.BaseUrl;

			var device = new Device.Device();
			var sqliteSetup = new SQLiteSetup(sqliteplatform);
			var sqliteStorage = new SQLiteStorage(sqliteSetup, logger);
			var scheduler = new ReactiveUIAroundMe.Droid.Threading.HandlerScheduler();
			var webServiceController = new WebServiceController(sqliteStorage, logger, clientHandler, scheduler, userDefaults);
			var googleMapsWebServiceController = new GoogleMapsWebServiceController(sqliteStorage, logger, clientHandler, scheduler, userDefaults);
			var applicationStateHandler = new ApplicationStateHandler(sqliteStorage);
			//var orientationHandler = new OrientationHandlerAndroid();
			var pathLocator = new PathLocator();
			var locationManager = new LocationManager();

			// pages
			// todo: we could use reflection here
			//Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LoginPageViewModel>));
			Locator.CurrentMutable.Register(() => new HomeActivity(), typeof(IViewFor<HomePageViewModel>));
			//Locator.CurrentMutable.Register(() => new SuperAdminPage(), typeof(IViewFor<SuperAdminPageViewModel>));
			//Locator.CurrentMutable.Register(() => new SearchResultsPage(), typeof(IViewFor<SearchResultsPageViewModel>));
			//Locator.CurrentMutable.Register(() => new MapPage(), typeof(IViewFor<MapPageViewModel>));
			Locator.CurrentMutable.Register(() => new FlyoutMenuActivity(), typeof(IViewFor<FlyoutMenuPageViewModel>));

			// singletons
			Locator.CurrentMutable.RegisterLazySingleton(() => applicationStateHandler, typeof(ApplicationStateHandler));
			Locator.CurrentMutable.RegisterLazySingleton(() => webServiceController, typeof(WebServiceController));
			Locator.CurrentMutable.RegisterLazySingleton(() => googleMapsWebServiceController, typeof(GoogleMapsWebServiceController));

			Locator.CurrentMutable.Register<ISettings>(() => settings);
			Locator.CurrentMutable.Register<IUserDefaults>(() => userDefaults);
			Locator.CurrentMutable.Register<ISQLitePlatform>(() => sqliteplatform);
			Locator.CurrentMutable.Register<ISQLiteSetup>(() => sqliteSetup);
			Locator.CurrentMutable.Register<ISQLiteStorage>(() => sqliteStorage);
			Locator.CurrentMutable.Register<Portable.Logging.ILogger>(() => logger);
			Locator.CurrentMutable.Register<IDevice>(() => device);
			//Locator.CurrentMutable.Register<IOrientationHandler>(() => orientationHandler);
			Locator.CurrentMutable.Register<IPathLocator>(() => pathLocator);
			Locator.CurrentMutable.Register<IScheduler>(() => scheduler);
			Locator.CurrentMutable.Register<ILocationManager>(() => locationManager);
			Locator.CurrentMutable.Register<HttpClientHandler>(() => clientHandler);

			// converters
			Locator.CurrentMutable.RegisterConstant(new NotConverter(), typeof(IBindingTypeConverter));

			//Locator.CurrentMutable.RegisterViewsForViewModels(this.GetType().GetTypeInfo().Assembly);

			_flyoutMenuViewModel = new FlyoutMenuPageViewModel(scheduler, applicationStateHandler, sqliteStorage,
										webServiceController, googleMapsWebServiceController, pathLocator, logger, device, this, locationManager);
			
			//// Navigate to the opening page of the application
			Router.Navigate.Execute(new HomePageViewModel(scheduler, applicationStateHandler, sqliteStorage,
										webServiceController, googleMapsWebServiceController, pathLocator, logger, device, this, locationManager));
		}

		/// <summary>
		/// Creates the flyout menu.
		/// </summary>
		/// <returns>The flyout menu.</returns>
		public ReactiveActivity CreateFlyoutMenu()
		{
			return new FlyoutMenuActivity()
			{
				//ViewModel = _flyoutMenuViewModel
			};
		}

		/// <summary>
		/// Creates the main page.
		/// </summary>
		/// <returns>The main page.</returns>
		/*public RoutedViewHost CreateRouteHost()
		{
			// NB: This returns the opening page that the platform-specific
			// boilerplate code will look for. It will know to find us because
			// we've registered our AppBootstrapper as an IScreen.
			return new RoutedViewHost()
			{
				Router = this.Router
			};
		}*/
	}
}
