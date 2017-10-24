// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NSRunloopScheduler.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS.Threading
{
	using System;
	using System.Reactive.Concurrency;
	using System.Reactive.Disposables;

	using Foundation;
	using CoreFoundation;

	/// <summary>
	/// Provides a scheduler which will use the Cocoa main loop to schedule
	/// work on. This is the Cocoa equivalent of DispatcherScheduler.
	/// </summary>
	public class NSRunloopScheduler : IScheduler
	{
		/// <summary>
		/// Gets the now.
		/// </summary>
		/// <value>The now.</value>
		public DateTimeOffset Now 
		{
			get 
			{
				return DateTimeOffset.Now; 
			}
		}

		/// <summary>
		/// Schedule the specified state and action.
		/// </summary>
		/// <param name="state">State.</param>
		/// <param name="action">Action.</param>
		/// <typeparam name="TState">The 1st type parameter.</typeparam>
		public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
		{
			var innerDisp = new SingleAssignmentDisposable();

			DispatchQueue.MainQueue.DispatchAsync(new Action(() => 
			{
				if (!innerDisp.IsDisposed)
				{
					innerDisp.Disposable = action(this, state);
				}
			}));

			return innerDisp;
		}

		/// <summary>
		/// Schedule the specified state, dueTime and action.
		/// </summary>
		/// <param name="state">State.</param>
		/// <param name="dueTime">Due time.</param>
		/// <param name="action">Action.</param>
		/// <typeparam name="TState">The 1st type parameter.</typeparam>
		public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
		{
			if (dueTime <= Now) 
			{
				return Schedule(state, action);
			}

			return Schedule(state, dueTime - Now, action);
		}

		/// <summary>
		/// Schedule the specified state, dueTime and action.
		/// </summary>
		/// <param name="state">State.</param>
		/// <param name="dueTime">Due time.</param>
		/// <param name="action">Action.</param>
		/// <typeparam name="TState">The 1st type parameter.</typeparam>
		public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
		{
			var innerDisp = Disposable.Empty;
			bool isCancelled = false;

			var timer = NSTimer.CreateScheduledTimer(dueTime, _ => 
				{
					if (!isCancelled) 
					{
						innerDisp = action(this, state);
					}
				});

			return Disposable.Create(() => 
				{
					isCancelled = true;
					timer.Invalidate();
					innerDisp.Dispose();
				});
		}
	}
}