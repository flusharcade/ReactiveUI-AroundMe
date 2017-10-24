// --------------------------------------------------------------------------------------------------
//  <copyright file="AppDelegate.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS
{
	using System;

	using Foundation;
    using UIKit;
    using ReactiveUI;

    using ReactiveUIAroundMe.Portable.ViewModels;

    //using HockeyApp.iOS;

    using Microsoft.Azure.Mobile;
    using Microsoft.Azure.Mobile.Analytics;
    using Microsoft.Azure.Mobile.Crashes;
	using Microsoft.Azure.Mobile.Push;

    // google firebase
    using UserNotifications;
	using Firebase.CloudMessaging;
    using Firebase.InstanceID;
    using Firebase.Analytics;

    using ReactiveUIAroundMeNative.iOS.Event;
    using Firebase.Database;

    /// <summary>
    /// App delegate.
    /// </summary>
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        public event EventHandler<UserInfoEventArgs> NotificationReceived;

        #region Private Properties

        /// <summary>
        /// The window.
        /// </summary>
        private UIWindow _window;

        /// <summary>
        /// The root view model.
        /// </summary>
        private ViewModelBase _rootViewModel;

        /// <summary>
        /// The suspend helper.
        /// </summary>
        private AutoSuspendHelper _suspendHelper;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the router.
        /// </summary>
        /// <value>The router.</value>
        public RoutingState Router { get; private set; }

        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        /// <value>The root node.</value>
		public DatabaseReference RootNode { get; set; }

		#endregion

		public AppDelegate()
        {
            RxApp.SuspensionHost.CreateNewAppState = () => new AppBootstrapper();
        }

        #region Public Methods

        /// <summary>
        /// Finisheds the launching.
        /// </summary>
        /// <returns><c>true</c>, if launching was finisheded, <c>false</c> otherwise.</returns>
        /// <param name="application">Application.</param>
        /// <param name="launchOptions">Launch options.</param>
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
			// Monitor token generation
			InstanceId.Notifications.ObserveTokenRefresh(TokenRefreshNotification);

			// Register your app for remote notifications.
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				// iOS 10 or later
				var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
				UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
				{
					Console.WriteLine(granted);
				});

				// For iOS 10 display notification (sent via APNS)
				UNUserNotificationCenter.Current.Delegate = this;

				// For iOS 10 data message (sent via FCM)
				Messaging.SharedInstance.RemoteMessageDelegate = this;
			}
			else
			{
				// iOS 9 or before
				var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
				var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			}

			UIApplication.SharedApplication.RegisterForRemoteNotifications();

			App.Configure();

            // database initialisation
			Database.DefaultInstance.PersistenceEnabled = true;
			RootNode = Database.DefaultInstance.GetRootReference();
			
            //#if ENABLE_TEST_CLOUD
            //Xamarin.Calabash.Start();
            //#endif

            //MobileCenter.Start("34d7d8ca-c1f9-4099-b41c-da723fb37bf6",
                   //typeof(Analytics), typeof(Crashes));

            // hockey app
            //var manager = BITHockeyManager.SharedHockeyManager;
            //manager.Configure("com.flusharcade.ReactiveUIAroundMe");
            //manager.StartManager();
            //manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds

            RxApp.SuspensionHost.SetupDefaultSuspendResume();

            _suspendHelper = new AutoSuspendHelper(this);
            _suspendHelper.FinishedLaunching(application, launchOptions);

            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

            var bootstrapper = RxApp.SuspensionHost.GetAppState<AppBootstrapper>();

            _window = new UIWindow(UIScreen.MainScreen.Bounds);
            //_window.RootViewController = new SplitViewController(bootstrapper.CreateFlyoutMenu(),
            //                                                     bootstrapper.CreateRouteHost());

            _window.RootViewController = bootstrapper.CreateRouteHost();
            _window.MakeKeyAndVisible();

            return true;
        }

        /// <summary>
        /// Dids the enter background.
        /// </summary>
        /// <param name="application">Application.</param>
		public override void DidEnterBackground(UIApplication application)
		{
			Messaging.SharedInstance.Disconnect();
            _suspendHelper.DidEnterBackground(application);
		}


		public override void WillEnterForeground(UIApplication application)
		{
			//ConnectToFCM (Window.RootViewController);
		}

        /// <summary>
        /// Ons the activated.
        /// </summary>
        /// <param name="application">Application.</param>
		public override void OnActivated(UIApplication application)
		{
			//ConnectFCM();

			_suspendHelper.OnActivated(application);

			//base.OnActivated(application);
		}

        /// <summary>
        /// Registereds for remote notifications.
        /// </summary>
        /// <param name="application">Application.</param>
        /// <param name="deviceToken">Device token.</param>
		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
#if DEBUG
			InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Sandbox);
#endif

#if RELEASE
            InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Prod);
#endif
		}

		// To receive notifications in foregroung on iOS 9 and below.
		// To receive notifications in background in any iOS version
		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			// If you are receiving a notification message while your app is in the background,
			// this callback will not be fired 'till the user taps on the notification launching the application.

			// If you disable method swizzling, you'll need to call this method. 
			// This lets FCM track message delivery and analytics, which is performed
			// automatically with method swizzling enabled.
			//Messaging.GetInstance ().AppDidReceiveMessage (userInfo);

			if (NotificationReceived == null)
				return;

			var e = new UserInfoEventArgs { UserInfo = userInfo };
			NotificationReceived(this, e);
		}

		// To receive notifications in foreground on iOS 10 devices.
		[Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
		public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			if (NotificationReceived == null)
				return;

			var e = new UserInfoEventArgs { UserInfo = notification.Request.Content.UserInfo };
			NotificationReceived(this, e);
		}

		// Receive data message on iOS 10 devices.
		public void ApplicationReceivedRemoteMessage(RemoteMessage remoteMessage)
		{
			Console.WriteLine(remoteMessage.AppData);
		}

		#endregion

		//////////////////
		////////////////// WORKAROUND
		//////////////////

		#region Workaround for handling notifications in background for iOS 10

        /// <summary>
        /// Dids the receive notification response.
        /// </summary>
        /// <param name="center">Center.</param>
        /// <param name="response">Response.</param>
        /// <param name="completionHandler">Completion handler.</param>
		[Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
		public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
		{
			if (NotificationReceived == null)
				return;

			var e = new UserInfoEventArgs { UserInfo = response.Notification.Request.Content.UserInfo };
			NotificationReceived(this, e);
		}

		#endregion

		//////////////////
		////////////////// END OF WORKAROUND
		//////////////////
		/// 
		
        /// <summary>
        /// Tokens the refresh notification.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void TokenRefreshNotification(object sender, NSNotificationEventArgs e)
		{
			// This method will be fired everytime a new token is generated, including the first
			// time. So if you need to retrieve the token as soon as it is available this is where that
			// should be done.
			//var refreshedToken = InstanceId.SharedInstance.Token;

			ConnectToFCM(_window.RootViewController);

			// TODO: If necessary send token to application server.
		}

		#region Private Methods

        /// <summary>
        /// Connects to fcm.
        /// </summary>
        /// <param name="fromViewController">From view controller.</param>
		public static void ConnectToFCM(UIViewController fromViewController)
		{
			Messaging.SharedInstance.Connect(error =>
			{
				if (error != null)
				{
					ShowMessage("Unable to connect to FCM", error.LocalizedDescription, fromViewController);
				}
				else
				{
					ShowMessage("Success!", "Connected to FCM", fromViewController);
					Console.WriteLine($"Token: {InstanceId.SharedInstance.Token}");
				}
			});
		}

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        /// <param name="fromViewController">From view controller.</param>
        /// <param name="actionForOk">Action for ok.</param>
		public static void ShowMessage(string title, string message, UIViewController fromViewController, Action actionForOk = null)
		{
			/*if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
				alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (obj) =>
				{
					if (actionForOk != null)
					{
						actionForOk();
					}
				}));
				fromViewController.PresentViewController(alert, true, null);
			}
			else
			{
				new UIAlertView(title, message, null, "Ok", null).Show();
			}*/
		}

        #endregion
    }
}