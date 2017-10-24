using Android.App;
using Android.Widget;
using Android.OS;

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

using ReactiveUI;

using ReactiveUIAroundMe.Portable.ViewModels;

namespace ReactiveUIAroundMe.Droid
{
    [Activity(Label = "Around Me", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
		/// <summary>
		/// The root view model.
		/// </summary>
		private ViewModelBase _rootViewModel;

        /// <summary>
		/// The suspend helper.
		/// </summary>
		private AutoSuspendHelper _suspendHelper;

		#region Public Properties

		/// <summary>
		/// Gets the router.
		/// </summary>
		/// <value>The router.</value>
		public RoutingState Router { get; private set; }

		#endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			MobileCenter.Start("34d7d8ca-c1f9-4099-b41c-da723fb37bf6",
                typeof(Analytics), typeof(Crashes));

			RxApp.SuspensionHost.CreateNewAppState = () => new AppBootstrapper();

			_suspendHelper = new AutoSuspendHelper(Application);

			RxApp.SuspensionHost.SetupDefaultSuspendResume();

			var bootstrapper = RxApp.SuspensionHost.GetAppState<AppBootstrapper>();

            //bootstrapper.CreateFlyoutMenu();
        }
    }
}

