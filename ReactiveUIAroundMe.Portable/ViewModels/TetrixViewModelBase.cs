// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectableBase.cs" company="Champion Data Pty Ltd.">
//   Copyright (c) 2015 Champion Data Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using ReactiveUIAroundMe.Portable.Location;

namespace ReactiveUIAroundMe.Portable.ViewModels
{
	using System;
	using System.Windows.Input;
	using System.Reactive.Concurrency;
	using System.Collections.Generic;

	using ReactiveUI;

	using ReactiveUIAroundMe.Portable.ViewModels;
	
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Logging;
	using ReactiveUIAroundMe.Portable.Common;
	using ReactiveUIAroundMe.Portable.Enums;
	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable.UI;
	using System.Reflection;

	/// <summary>
	/// </summary>
	public abstract class TetrixViewModelBase : SelectableViewModelBase
	{
		#region Private Properties

		/// <summary>
		/// The layout.
		/// </summary>
		private LayoutType _layout;

		/// <summary>
		/// The use XS pacing.
		/// </summary>
		private bool _useXSpacing;

		/// <summary>
		/// The use YS pacing.
		/// </summary>
		private bool _useYSpacing;

		/// <summary>
		/// The is super scrollable.
		/// </summary>
		private bool _heightCalculated;

		/// <summary>
		/// The height.
		/// </summary>
		private double _height;

		/// <summary>
		/// The height.
		/// </summary>
		private double _width;

		/// <summary>
		/// The x spacing.
		/// </summary>
		private double _xSpacingScale;

		/// <summary>
		/// The  x spacing scale.
		/// </summary>
		private double _xPos;

		/// <summary>
		/// The  x position.
		/// </summary>
		private double _yPos;

		/// <summary>
		/// The position.
		/// </summary>
		private int _position;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the is selected.
		/// </summary>
		/// <value>The is selected.</value>
		public LayoutType Layout
		{
			get { return _layout; }
			set
			{
				this.RaiseAndSetIfChanged(ref _layout, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.TetrixViewModelBase"/>
		/// use XS pacing.
		/// </summary>
		/// <value><c>true</c> if use XS pacing; otherwise, <c>false</c>.</value>
		public bool UseXSpacing
		{
			get { return _useXSpacing; }
			set
			{
				this.RaiseAndSetIfChanged(ref _useXSpacing, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.TetrixViewModelBase"/>
		/// use YS pacing.
		/// </summary>
		/// <value><c>true</c> if use YS pacing; otherwise, <c>false</c>.</value>
		public bool UseYSpacing
		{
			get { return _useYSpacing; }
			set
			{
				this.RaiseAndSetIfChanged(ref _useYSpacing, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this
		/// <see cref="T:ReactiveUIAroundMe.Portable.ViewModels.TetrixViewModelBase"/> is super scrollable.
		/// </summary>
		/// <value><c>true</c> if is super scrollable; otherwise, <c>false</c>.</value>
		public bool HeightCalculated
		{
			get { return _heightCalculated; }
			set
			{
				this.RaiseAndSetIfChanged(ref _heightCalculated, value);
			}
		}


		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		/// <value>The height.</value>
		public double Width
		{
			get { return _height; }
			set
			{
				this.RaiseAndSetIfChanged(ref _height, value);
			}
		}

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		/// <value>The height.</value>
		public double Height
		{
			get { return _height; }
			set
			{
				this.RaiseAndSetIfChanged(ref _height, value);
			}
		}

		/// <summary>
		/// Gets or sets the XS pacing.
		/// </summary>
		/// <value>The XS pacing.</value>
		public double XSpacingScale
		{
			get { return _xSpacingScale; }
			set
			{
				this.RaiseAndSetIfChanged(ref _xSpacingScale, value);
			}
		}

		/// <summary>
		/// Gets or sets the XP os.
		/// </summary>
		/// <value>The XP os.</value>
		public double XPos
		{
			get { return _xPos; }
			set
			{
				this.RaiseAndSetIfChanged(ref _xPos, value);
			}
		}

		public double YPos
		{
			get { return _yPos; }
			set
			{
				this.RaiseAndSetIfChanged(ref _yPos, value);
			}
		}

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public int Position
		{
			get { return _position; }
			set
			{
				this.RaiseAndSetIfChanged(ref _position, value);
			}
		}

		/// <summary>
		/// Gets the cell identifier.
		/// </summary>
		/// <value>The cell identifier.</value>
		public abstract object CellId
		{
			get;
		}

		private bool _isEnabled = true;

		/// <summary>
		/// Gets or sets the is selected.
		/// </summary>
		/// <value>The is selected.</value>
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set
			{
				this.RaiseAndSetIfChanged(ref _isEnabled, value);
			}
		}

		private bool _isVisible = true;

		/// <summary>
		/// Gets or sets the is selected.
		/// </summary>
		/// <value>The is selected.</value>
		public bool IsVisible
		{
			get { return _isVisible; }
			set
			{
				this.RaiseAndSetIfChanged(ref _isVisible, value);
			}
		}

		/// <summary>
		/// Apply this instance.
		/// </summary>
		public virtual void Apply(TetrixViewModelBase newItem)
		{
		}

		/// <summary>
		/// Apply the specified contract.
		/// </summary>
		/// <returns>The apply.</returns>
		/// <param name="contract">Contract.</param>
		public void Apply(BaseContract contract)
		{
			// use reflection to iterate through all contract properties 
			// and set to equivalent view model properties
			foreach (PropertyInfo prop in contract.GetType().GetRuntimeProperties())
			{
				var vmProperty = GetType().GetRuntimeProperty(prop.Name);
				var contractPropertyValue = prop.GetValue(contract);
				vmProperty.SetValue(this, contractPropertyValue);
			}
		}

		/// <summary>
		/// Sets the cell identifier.
		/// </summary>
		/// <param name="id">Identifier.</param>
		public virtual void SetCellId(object id)
		{
		}

		private ReactiveCommand _editCommand;
		public ReactiveCommand EditCommand
		{
			get { return _editCommand; }
			set { this.RaiseAndSetIfChanged(ref _editCommand, value); }
		}

		private ReactiveCommand _retireCommand;
		public ReactiveCommand RetireCommand
		{
			get { return _retireCommand; }
			set { this.RaiseAndSetIfChanged(ref _retireCommand, value); }
		}

		private ReactiveCommand _cancelCommand;
		public ReactiveCommand CancelCommand
		{
			get { return _cancelCommand; }
			set { this.RaiseAndSetIfChanged(ref _cancelCommand, value); }
		}

		private ReactiveCommand _saveCommand;
		public ReactiveCommand SaveCommand
		{
			get { return _saveCommand; }
			set { this.RaiseAndSetIfChanged(ref _saveCommand, value); }
		}

		/// <summary>
		/// Gets or sets the select parameters.
		/// </summary>
		/// <value>The select parameters.</value>
		public virtual IDictionary<string, object> SelectParameters { get; set; }

		/// <summary>
		/// Applies the commands.
		/// </summary>
		/// <param name="edit">Edit.</param>
		/// <param name="retire">Retire.</param>
		/// <param name="cancel">Cancel.</param>
		/// <param name="save">Save.</param>
		public void ApplyCommands(Action edit, Action retire, Action cancel, Action save)
		{
			EditCommand = ReactiveCommand.Create(edit);
			RetireCommand = ReactiveCommand.Create(retire);
			CancelCommand = ReactiveCommand.Create(cancel);
			SaveCommand = ReactiveCommand.Create(save);
		}

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.ViewModels.TetrixViewModelBase"/> class.
		/// </summary>
		protected TetrixViewModelBase(ISQLiteStorage storage, IScheduler scheduler, ILogger log,
							 ApplicationStateHandler applicationStateHandler, WebServiceController webServiceController, GoogleMapsWebServiceController googleMapsWebServiceController,
		                     IPathLocator pathLocator, IScreen hostScreen, ILocationManager locationManager)
			: base(storage, scheduler, log, applicationStateHandler, webServiceController, googleMapsWebServiceController, pathLocator, hostScreen, locationManager)
		{
		}

		#endregion

	}
}