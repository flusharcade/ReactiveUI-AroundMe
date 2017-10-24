// --------------------------------------------------------------------------------------------------
//  <copyright file="HandlerScheduler.cs" company="Flush Arcade">
//    Copyright (c) 2015 Flush Arcade All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Droid.Threading
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Reactive;
	using System.Reactive.Concurrency;
	using System.Reactive.Disposables;

	using Android.App;
	using Android.OS;

	/// <summary>
	/// HandlerScheduler is a scheduler that schedules items on a running 
	/// Activity's main thread. This is the moral equivalent of 
	/// DispatcherScheduler.
	public class HandlerScheduler : IScheduler
	{
		public static IScheduler MainThreadScheduler = new HandlerScheduler(new Handler(Looper.MainLooper), Looper.MainLooper.Thread.Id);

		Handler handler;
		long looperId;

		public HandlerScheduler()
		{
			this.handler = new Handler(Looper.MainLooper);
			this.looperId = Looper.MainLooper.Thread.Id;
		}

		public HandlerScheduler(Handler handler, long? threadIdAssociatedWithHandler)
		{
			this.handler = handler;
			looperId = threadIdAssociatedWithHandler ?? -1;
		}

		public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
		{
			bool isCancelled = false;
			var innerDisp = new SerialDisposable()
			{
				Disposable = Disposable.Empty

			};

			if (looperId > 0 && looperId == Java.Lang.Thread.CurrentThread().Id)
			{
				return action(this, state);
			}

			handler.Post(() =>
			{
				if (isCancelled)
					return;

				innerDisp.Disposable = action(this, state);
			});

			return new CompositeDisposable(Disposable.Create(() => isCancelled = true), innerDisp);
		}

		public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
		{
			bool isCancelled = false;
			var innerDisp = new SerialDisposable() { Disposable = Disposable.Empty };

			handler.PostDelayed(() =>
			{
				if (isCancelled)
					return;

				innerDisp.Disposable = action(this, state);

			}, dueTime.Ticks / 10 / 1000);

			return new CompositeDisposable(Disposable.Create(() => isCancelled = true), innerDisp);
		}

		public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
		{
			if (dueTime <= Now)
			{
				return Schedule(state, action);
			}

			return Schedule(state, dueTime - Now, action);
		}

		public DateTimeOffset Now
		{
			get { return DateTimeOffset.Now; }
		}
	}
}
