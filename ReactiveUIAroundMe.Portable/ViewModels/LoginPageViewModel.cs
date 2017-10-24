// --------------------------------------------------------------------------------------------------
//  <copyright file="LoginPageViewModel.cs" company="Flush Arcade Pty Ltd.">
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

	using ReactiveUI;

	using ReactiveUIAroundMe.Portable;
	
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable.Logging;
	using ReactiveUIAroundMe.Portable.Location;
	using ReactiveUIAroundMe.Portable.UI;

	/// <summary>
	/// Login page view model.
	/// </summary>
	public class LoginPageViewModel : ViewModelBase
	{
		/// <summary>
		/// The web service controller.
		/// </summary>
		private WebServiceController _webServiceController;

		/// <summary>
		/// The device.
		/// </summary>
		private IDevice _device;

        #region Bindable

        /// <summary>
        /// The username.
        /// </summary>
        private string _username = string.Empty;

		/// <summary>
		/// The password.
		/// </summary>
		private string _password = string.Empty;

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		[DataMember]
		public string Username
		{
			get { return _username; }
			set { 
				this.RaiseAndSetIfChanged(ref _username, value); 
			}
		}

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		[DataMember]
		public string Password
		{
			get { return _password; }
			set {
                this.RaiseAndSetIfChanged(ref _password, value);
            }
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the login command.
		/// </summary>
		/// <value>The login command.</value>
		[DataMember]
		public ReactiveCommand LoginCommand { get; set; }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.LoginPageViewModel"/> class.
		/// </summary>
		/// <param name="signalRClient">Signal RC lient.</param>
		/// <param name="scheduler">Scheduler.</param>
		/// <param name="applicationStateHandler">Application state handler.</param>
		/// <param name="storage">Storage.</param>
		/// <param name="webServiceController">Web service controller.</param>
		/// <param name="log">Log.</param>
		/// <param name="device">Device.</param>
		public LoginPageViewModel(IScheduler scheduler, ApplicationStateHandler applicationStateHandler, 
		                         ISQLiteStorage storage, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
		                         IPathLocator pathLocator, ILogger log, IDevice device, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, googleMapsWebServiceController, pathLocator, hostScreen, locationManager)
		{
			Title = "Welcome";

			_webServiceController = webServiceController;
			_device = device;

			scheduler.ScheduleAsync((arg1, arg2) => SetupSQLite());
			scheduler.ScheduleAsync((arg1, arg2) => Load());

            var canLogin = this.WhenAnyValue(
                vm => vm.Username, 
                vm => vm.Password, 
                vm => vm.IsLoading, 
                (username, password, loading) => 
					!string.IsNullOrEmpty(Username) &&
					!string.IsNullOrEmpty(Password) &&
					!IsLoading);

            LoginCommand = ReactiveCommand.CreateFromObservable(LoginAsync,
                canLogin,
                Scheduler);
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		private async Task Load()
		{
			var identity = await Storage.GetObject<IdentityStorable>(StorableKeys.Identity.ToString());
			if (identity != null)
			{
				Username = identity.Username;
			}
		}

		/// <summary>
		/// Login this instance.
		/// </summary>
		private IObservable<Unit> LoginAsync()
		{
            IsLoading = true;
            IsError = false;

            return _webServiceController
                .AuthorizeDevice(Username, Password, _device.DeviceId)
                .ObserveOn(this.Scheduler)
                .Catch<AuthContract, Exception>(error =>
				{
                    DidException(error, "Login failed using call to web service AuthorizeDevice");
					return Observable.Empty<AuthContract>();
				})
                .Do(auth =>
                {
                    IsLoading = false;

                    var identity = new IdentityStorable();
                    identity.Apply(auth);

                    Storage.InsertObject(identity).ConfigureAwait(false);

                    if (auth != null)
                    {
                        // would like to add a nicer control for reusing singletons in Splat
                        HostScreen.Router.Navigate.Execute(new SuperAdminPageViewModel(Storage, Scheduler,
						Log, ApplicationStateHandler, WebServiceController, GoogleMapsWebServiceController,
						PathLocator, HostScreen, LocationManager));
                    }
                })
				.Select(x => Unit.Default);
		}
	}
}