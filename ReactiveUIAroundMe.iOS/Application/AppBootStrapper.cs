// --------------------------------------------------------------------------------------------------
//  <copyright file="AppDelegate.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS
{
	using System.Net;
	using System.Net.Http;
	using System.Reactive.Concurrency;
	using SQLite.Net.Interop;
	using SQLite.Net.Platform.XamarinIOS;

	using ReactiveUI;

	using ReactiveUIAroundMe.iOS.Device;
	using ReactiveUIAroundMe.iOS.DataAccess;
	using ReactiveUIAroundMe.iOS.Views;
	using ReactiveUIAroundMe.iOS.Extras;
	using ReactiveUIAroundMe.iOS.Logging;
	using ReactiveUIAroundMe.iOS.Location;
	using ReactiveUIAroundMe.iOS.Converters;

	using ReactiveUIAroundMe.Shared.DataAccess;
	
	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.UI;
	using ReactiveUIAroundMe.Portable.ViewModels;
	using ReactiveUIAroundMe.Portable.Resources;
	using ReactiveUIAroundMe.Portable.Location;

	using Splat;
    using System;

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
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.AppBootstrapper"/> class.
		/// </summary>
		public AppBootstrapper()
		{
            try
            {
                Router = new RoutingState();

                Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));

                HttpClientHandler clientHandler = new HttpClientHandler(); //_isWindows ? new HttpClientHandler() : new NativeMessageHandler();
                clientHandler.UseCookies = false;
                clientHandler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                // TODO: singletons, we want to pull resolved items from container, rather than outside declarations
                var sqliteplatform = new SQLitePlatformIOS();
                var logger = new LoggeriOS();
                var settings = new Settings();
                var userDefaults = new UserDefaults(settings);

                // temp until settings page added
                userDefaults.Host = Config.BaseUrl;

                var device = new Device.Device();
                var sqliteSetup = new SQLiteSetup(sqliteplatform);
                var sqliteStorage = new SQLiteStorage(sqliteSetup, logger);
                var scheduler = new ReactiveUIAroundMe.iOS.Threading.NSRunloopScheduler();
                var webServiceController = new WebServiceController(sqliteStorage, logger, clientHandler, scheduler, userDefaults);
                var googleMapsWebServiceController = new GoogleMapsWebServiceController(sqliteStorage, logger, clientHandler, scheduler, userDefaults);
                var applicationStateHandler = new ApplicationStateHandler(sqliteStorage);
                var orientationHandler = new OrientationHandleriOS();
                var pathLocator = new PathLocator();
                var locationManager = new LocationManager();

                // pages
                // todo: we could use reflection here
                Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LoginPageViewModel>));
                Locator.CurrentMutable.Register(() => new HomePage(), typeof(IViewFor<HomePageViewModel>));
                Locator.CurrentMutable.Register(() => new SuperAdminPage(), typeof(IViewFor<SuperAdminPageViewModel>));
                Locator.CurrentMutable.Register(() => new SearchResultsPage(), typeof(IViewFor<SearchResultsPageViewModel>));
                Locator.CurrentMutable.Register(() => new MapPage(), typeof(IViewFor<MapPageViewModel>));
                Locator.CurrentMutable.Register(() => new FlyoutMenuPage(), typeof(IViewFor<FlyoutMenuPageViewModel>));

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
                Locator.CurrentMutable.Register<IOrientationHandler>(() => orientationHandler);
                Locator.CurrentMutable.Register<IPathLocator>(() => pathLocator);
                Locator.CurrentMutable.Register<IScheduler>(() => scheduler);
                Locator.CurrentMutable.Register<ILocationManager>(() => locationManager);
                Locator.CurrentMutable.Register<HttpClientHandler>(() => clientHandler);

                // converters
                Locator.CurrentMutable.RegisterConstant(new NotConverter(), typeof(IBindingTypeConverter));

                //Locator.CurrentMutable.RegisterViewsForViewModels(this.GetType().GetTypeInfo().Assembly);

                _flyoutMenuViewModel = new FlyoutMenuPageViewModel(scheduler, applicationStateHandler, sqliteStorage,
                                            webServiceController, googleMapsWebServiceController, pathLocator, logger, device, this, locationManager);

                // Navigate to the opening page of the application
                Router.Navigate.Execute(new HomePageViewModel(scheduler, applicationStateHandler, sqliteStorage,
                                            webServiceController, googleMapsWebServiceController, pathLocator, logger, device, this, locationManager));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e);
            }
		}

		/// <summary>
		/// Creates the flyout menu.
		/// </summary>
		/// <returns>The flyout menu.</returns>
		public ReactiveViewController CreateFlyoutMenu()
		{
			return new FlyoutMenuPage()
			{
				ViewModel = _flyoutMenuViewModel
			};
		}

		/// <summary>
		/// Creates the main page.
		/// </summary>
		/// <returns>The main page.</returns>
		public RoutedViewHost CreateRouteHost()
		{
			// NB: This returns the opening page that the platform-specific
			// boilerplate code will look for. It will know to find us because
			// we've registered our AppBootstrapper as an IScreen.
			return new RoutedViewHost()
			{
				Router = this.Router
			};
		}
	}
}
