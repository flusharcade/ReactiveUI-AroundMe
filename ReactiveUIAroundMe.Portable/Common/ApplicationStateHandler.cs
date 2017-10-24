// --------------------------------------------------------------------------------------------------
//  <copyright file="ApplicationStateHandler.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Common
{
	using System;
	using System.Collections.Generic;
	using System.Reactive.Linq;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Reactive.Subjects;

	using ReactiveUIAroundMe.Portable.WebServices;
	using ReactiveUIAroundMe.Portable.DataAccess;
	using ReactiveUIAroundMe.Portable.Enums;
	using ReactiveUIAroundMe.Portable.ViewModels;

	/// <summary>
	/// Application state.
	/// </summary>
	public class ApplicationStateHandler
	{
		/// <summary>
		/// The latest transaction identifier.
		/// </summary>
		private readonly ISQLiteStorage _storage;

		/// <summary>
		/// Occurs when state updated.
		/// </summary>
		public Subject<ApplicationUpdatedArgs> StateUpdated;

		/// <summary>
		/// Occurs when players loading.
		/// </summary>
		public Subject<bool> ApplicationLoading;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.Common.ApplicationStateHandler"/> class.
		/// </summary>
		public ApplicationStateHandler(ISQLiteStorage storage)
		{
			_storage = storage;

			StateUpdated = new Subject<ApplicationUpdatedArgs>();
			ApplicationLoading = new Subject<bool>();
		}

		/// <summary>
		/// Apply the specified storable.
		/// </summary>
		/// <param name="storable">Storable.</param>
		public void Apply(ApplicationStateStorable storable)
		{
			SavedState().ConfigureAwait(false);
		}

		/// <summary>
		/// Saveds the state.
		/// </summary>
		public async Task SavedState()
		{
			NotifyStatusUpdate();

			var storable = new ApplicationStateStorable()
			{
				//Key = MatchId.ToString(),
			};
			storable.Apply(this);

			await _storage.InsertObject(storable);
		}

		/// <summary>
		/// Saveds the state.
		/// </summary>
		/// <returns>The state.</returns>
		public async Task LoadState(int fixtureId)
		{
			var storable = await _storage.GetObject<ApplicationStateStorable>(fixtureId.ToString());
			if (storable != null)
			{
				Apply(storable);
			}

			NotifyStatusUpdate();
		}

		/// <summary>
		/// Notifies the status update.
		/// </summary>
		public void NotifyStatusUpdate()
		{
			// update app status to listeners
			StateUpdated.OnNext(new ApplicationUpdatedArgs()
			{
			});
		}

		/// <summary>
		/// Notifies the players loading.
		/// </summary>
		/// <param name="isLoading">If set to <c>true</c> is loading.</param>
		public void NotifyApplicationLoading(bool isLoading)
		{
			// update app status to listeners
			ApplicationLoading.OnNext(isLoading);
		}
	}
}