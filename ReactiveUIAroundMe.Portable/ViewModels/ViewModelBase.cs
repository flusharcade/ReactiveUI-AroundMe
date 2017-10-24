// <copyright file="ViewModelBase.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.ViewModels
{
	using System;
	using System.Threading.Tasks;
	using System.Reactive.Concurrency;
	using System.Collections.Generic;
	using System.Reactive.Disposables;
	using System.Linq;
	using System.Reactive.Threading.Tasks;
	using System.Reactive.Linq;
	using System.Runtime.Serialization;
	using System.Collections.ObjectModel;

	using ReactiveUI;

	using ReactiveUIAroundMe.Portable.Resources;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Logging;
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.Location;

	using Splat;
	using WebServices;
	using UI;

	/// <summary>
	/// View model base.
	/// </summary>
	public class ViewModelBase : ReactiveObject, IRoutableViewModel
    {
		/// <summary>
		/// Occurs when position update.
		/// </summary>
		public event EventHandler<Location> LocationUpdate;

		/// <summary>
		/// Occurs when position update.
		/// </summary>
		public event EventHandler<string> AddressUpdate;

		/// <summary>
		/// Gets or sets the current location.
		/// </summary>
		/// <value>The current location.</value>
		[DataMember]
		public ObservableCollection<Location> Locations { get; set; }

        /// <summary>
        /// The web service controller.
        /// </summary>
        protected readonly WebServiceController WebServiceController;

		/// <summary>
		/// The google maps web service controller.
		/// </summary>
		protected readonly GoogleMapsWebServiceController GoogleMapsWebServiceController;

        /// <summary>
        /// The path locator.
        /// </summary>
        protected readonly IPathLocator PathLocator;

		/// <summary>
		/// The location manager.
		/// </summary>
		protected readonly ILocationManager LocationManager;

		/// <summary>
		/// The storage.
		/// </summary>
		protected readonly ISQLiteStorage Storage;

		/// <summary>
		/// The squad transaction generator.
		/// </summary>
		protected readonly ApplicationStateHandler ApplicationStateHandler;

		/// <summary>
		/// The log.
		/// </summary>
		protected readonly Logging.ILogger Log;

		/// <summary>
		/// The tag.
		/// </summary>
		protected readonly string Tag;

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        /// <value>The subscriptions.</value>
        public CompositeDisposable Subscriptions { get; private set; }

		/// <summary>
		/// The gradient on.
		/// </summary>
		private bool _gradientOn;

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/> gradient on.
		/// </summary>
		/// <value><c>true</c> if gradient on; otherwise, <c>false</c>.</value>
		public virtual bool GradientOn
		{
			get { return _gradientOn; }
			set {
				this.RaiseAndSetIfChanged(ref _gradientOn, value);
			}
		}

		/// <summary>
        /// The current address.
        /// </summary>
		private string _currentAddress;

		/// <summary>
        /// Gets or sets the current address.
        /// </summary>
        /// <value>The current address.</value>
		public string CurrentAddress
		{
			get { return _currentAddress; }
			set
			{
				this.RaiseAndSetIfChanged(ref _currentAddress, value);
			}
		}

		/// <summary>
		/// The current location.
		/// </summary>
		private Location _currentLocation;

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/> gradient on.
		/// </summary>
		/// <value><c>true</c> if gradient on; otherwise, <c>false</c>.</value>
		public Location CurrentLocation
		{
			get { return _currentLocation; }
			set
			{
				this.RaiseAndSetIfChanged(ref _currentLocation, value);
			}
		}

		/// <summary>
		/// The scheduler.
		/// </summary>
		public readonly IScheduler Scheduler;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/> class.
        /// </summary>
        /// <param name="signalRClient">Signal RC lient.</param>
        public ViewModelBase(ISQLiteStorage storage, IScheduler scheduler, Logging.ILogger log, 
		                     ApplicationStateHandler applicationStateHandler, WebServiceController webServiceController,
                             GoogleMapsWebServiceController googleMapsWebServiceController, IPathLocator pathLocator, IScreen hostScreen, 
		                     ILocationManager locationManager)
		{
            HostScreen = hostScreen;

			Locations = new ObservableCollection<Location>();
			CurrentLocation = new Location();

			LocationManager = locationManager;
			
            ConnectedStatusMessage = Labels.ConnectedTitle.ToUpper();

			Storage = storage;
			scheduler.Schedule((arg1) => Storage.CreateSQLiteConnection());

            WebServiceController = webServiceController;
			GoogleMapsWebServiceController = googleMapsWebServiceController;
            PathLocator = pathLocator;

			Subscriptions = new CompositeDisposable();

			Scheduler = scheduler;
			ApplicationStateHandler = applicationStateHandler;

			Log = log;
			Tag = $"{GetType()} ";
		}

		/// <summary>
		/// The connected status message.
		/// </summary>
		private string _connectedStatusMessage;

		/// <summary>
		/// Gets or sets the connected status message.
		/// </summary>
		/// <value>The connected status message.</value>
		public string ConnectedStatusMessage
		{
			get { return _connectedStatusMessage; }
			set { this.RaiseAndSetIfChanged(ref _connectedStatusMessage, value); }
		}

		/// <summary>
		/// The is connected.
		/// </summary>
		private bool _isConnected;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/> is connected.
		/// </summary>
		/// <value><c>true</c> if is connected; otherwise, <c>false</c>.</value>
		public bool IsConnected
		{
			get { return _isConnected; }
			set { this.RaiseAndSetIfChanged(ref _isConnected, value); }
		}

        /// <summary>
        /// The is loading.
        /// </summary>
		private bool _isLoading;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/> is loading.
        /// </summary>
        /// <value><c>true</c> if is loading; otherwise, <c>false</c>.</value>
		public bool IsLoading
		{
			get { return _isLoading; }
			set { this.RaiseAndSetIfChanged(ref _isLoading, value); }
		}

		private bool _locationLoading;
		public bool LocationLoading
		{
			get { return _locationLoading; }
			set { this.RaiseAndSetIfChanged(ref _locationLoading, value); }
		}

		private string _backgroundColor = "#fdfdfd";
		public virtual string BackgroundColor
		{
			get { return _backgroundColor; }
			set { this.RaiseAndSetIfChanged(ref _backgroundColor, value); }
		}

		/// <summary>
		/// Gets the color of the border.
		/// </summary>
		/// <value>The color of the border.</value>
		public virtual string BorderColor
		{
			get
			{
				return IsSelectable ? IsSelected ? ColorPalette.Blue :
					ColorPalette.DarkGray1 :
					ColorPalette.DarkGray1;
			}
		}


		/// <summary>
		/// Gets the color of the text.
		/// </summary>
		/// <value>The color of the text.</value>
		public string TextColor
		{
			get
			{
				return IsSelectable ? IsSelected ? "#FFFFFF" : ColorPalette.DarkGray1 : ColorPalette.DarkGray1;
			}
		}

		/// <summary>
		/// The is selected.
		/// </summary>
		private bool _isSelected;

		/// <summary>
		/// The is selectable.
		/// </summary>
		private bool _isSelectable = true;

		/// <summary>
		/// Gets or sets the is selected.
		/// </summary>
		/// <value>The is selected.</value>
		public virtual bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				this.RaiseAndSetIfChanged(ref _isSelected, value);

				GradientOn = !IsSelectable || IsSelected ? false : true;

				// raise event changes for colors everytime selection changes occur
				this.RaisePropertyChanged("BackgroundColor");
				this.RaisePropertyChanged("BorderColor");
				this.RaisePropertyChanged("TextColor");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this
		/// <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.PlayerViewModel"/> is selectable.
		/// </summary>
		/// <value><c>true</c> if is selectable; otherwise, <c>false</c>.</value>
		public virtual bool IsSelectable
		{
			get { return _isSelectable; }
			set
			{
				this.RaiseAndSetIfChanged(ref _isSelectable, value);

				GradientOn = !IsSelectable || IsSelected ? false : true;

                // raise event changes for colors everytime selection changes occur
                this.RaisePropertyChanged("BackgroundColor");
                this.RaisePropertyChanged("BorderColor");
                this.RaisePropertyChanged("TextColor");
            }
		}

		private string _errorMessage = string.Empty;
		public string ErrorMessage
		{
			get { return _errorMessage; }
			set { this.RaiseAndSetIfChanged(ref _errorMessage, value); }
		}

		private bool _isError;
		public bool IsError
		{
			get { return _isError; }
			set { this.RaiseAndSetIfChanged(ref _isError, value); }
		}

		/// <summary>
		/// The title.
		/// </summary>
		private string _title = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/> is connected.
		/// </summary>
		/// <value><c>true</c> if is connected; otherwise, <c>false</c>.</value>
		public string Title
		{
			get { return _title; }
			set { this.RaiseAndSetIfChanged(ref _title, value); }
		}

		/// <summary>
		/// The quarter time remaining.
		/// </summary>
		private TimeSpan _quarterTime;

		/// <summary>
		/// Gets or sets the quarter time remaining.
		/// </summary>
		/// <value>The quarter time remaining.</value>
		public TimeSpan QuarterTime
		{
			get { return _quarterTime; }
			set
			{
				this.RaiseAndSetIfChanged(ref _quarterTime, value);
			}
		}

        /// <summary>
        /// </summary>
        private string _message;

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message
		{
			get { return _message; }
			set { this.RaiseAndSetIfChanged(ref _message, value); }
		}

		/// <summary>
		/// </summary>
		private string _progressMessage;

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string ProgressMessage
		{
			get { return _progressMessage; }
			set { this.RaiseAndSetIfChanged(ref _progressMessage, value); }
		}

		/// <summary>
		/// Gets the URL path segment.
		/// </summary>
		/// <value>The URL path segment.</value>
		public virtual string UrlPathSegment
		{
			get
			{
				return string.Empty;
			}
		}

        private IScreen _hostScreen;

        public IScreen HostScreen
        {
            get
            {
                return _hostScreen;
            }
            set
            {
                _hostScreen = value;
            }           
        }

		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init()
		{
		}

		/// <summary>
		/// Setups the SQL ite.
		/// </summary>
		/// <returns>The SQL ite.</returns>
		protected async Task SetupSQLite()
		{
			await Storage.Setup();
		}

		/// <summary>
		/// Posts the load subscriptions.
		/// </summary>
		/// <returns>The load subscriptions.</returns>
		public virtual IEnumerable<IDisposable> PreLoadSubscriptions()
		{
			yield break;
		}

		/// <summary>
		/// Posts the load subscriptions.
		/// </summary>
		/// <returns>The load subscriptions.</returns>
		public virtual IEnumerable<IDisposable> PostLoadSubscriptions()
		{
			yield return LocationManager.LocationUpdates
	            .Subscribe(NotifyLocationUpdate);

			yield return LocationManager.AddressUpdates
				.Subscribe(NotifyAddressUpdate);

		}

		/// <summary>
		/// Ons the hide.
		/// </summary>
		public virtual void OnHide()
		{
			Dispose();
		}

		/// <summary>
		/// </summary>
		/// <param name="parameters">
		/// </param>
		public virtual void OnShow(IDictionary<string, object> parameters)
		{
			foreach (var item in this.PreLoadSubscriptions())
			{
				this.Subscriptions.Add(item);
			}

			ProgressMessage = "Loading...";
			IsLoading = true;
			LoadAsync(parameters).ToObservable()
			                     .ObserveOn(Scheduler)
			                     .Subscribe(
				result =>
					{
						IsLoading = false;
						ProgressMessage = string.Empty;
						foreach (var item in this.PostLoadSubscriptions())
						{
							this.Subscriptions.Add(item);
						}
					},
				ex =>
					{
						IsLoading = false;
						Message = ex.Message;
						Log.WriteLine(Message);
					});
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the
		/// <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/>. The <see cref="Dispose"/> method leaves the
		/// <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/> so the garbage collector can reclaim the
		/// memory that the <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.ViewModelBase"/> was occupying.</remarks>
		public virtual void Dispose()
		{
			Subscriptions.Dispose();
			Subscriptions = new CompositeDisposable();
		}

		/// <summary>
		/// Loads the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="parameters">Parameters.</param>
		public virtual Task LoadAsync(IDictionary<string, object> parameters)
		{
			return Task.FromResult(0);
		}

		/// <summary>
		/// Saveds the state.
		/// </summary>
		public async Task SavedState()
		{
			var storable = new ApplicationStateStorable()
			{
				//Key = ApplicationStateHandler.MatchId.ToString(),
			};
			storable.Apply(ApplicationStateHandler);

			await Storage.InsertObject(storable);
		}

		/// <summary>
		/// Saveds the state.
		/// </summary>
		/// <returns>The state.</returns>
		public async Task LoadState(int fixtureId)
		{
			var storable = await Storage.GetObject<ApplicationStateStorable>(fixtureId.ToString());
			if (storable != null)
			{
				ApplicationStateHandler.Apply(storable);
			}
		}

        /// <summary>
        /// Passes all exceptions to logger
        /// </summary>
        protected void DidException(Exception error, string location)
        {
            IsLoading = false;
            IsError = true;

            Log.WriteLineTime(Tag + "\n" +
                location + "\n" +
                "ErrorMessage: \n" +
                error.Message + "\n" +
                "Stacktrace: \n " +
                error.StackTrace);
        }

		/// <summary>
		/// Notifies the location update.
		/// </summary>
		/// <param name="locationArgs">Location arguments.</param>
		protected void NotifyLocationUpdate(LocationUpdatedEventArgs locationArgs)
		{
			var location = locationArgs.Location;

			var newLocation = new Location()
			{
				Longitude = location.Longitude,
				Latitude = location.Latitude,
				Timestamp = DateTime.Now,
			};

			CurrentLocation = newLocation;

			Locations.Add(newLocation);
			LocationUpdate?.Invoke(this, newLocation);
			// stop any more location updates
			LocationManager.Stop();

			LocationLoading = false;
		}

		/// <summary>
        /// Notifies the address update.
        /// </summary>
        /// <param name="addressEventArgs">The ${ParameterType} instance containing the event data.</param>
		protected void NotifyAddressUpdate(AddressUpdatedEventArgs addressEventArgs)
		{
			var address = addressEventArgs.Address;
			CurrentAddress = address;
			AddressUpdate?.Invoke(this, address);
		}

		/// <summary>
		/// Updates the location.
		/// </summary>
		public void UpdateLocation()
		{
			LocationLoading = true;

			LocationManager.GetLocation();
		}
	}
}