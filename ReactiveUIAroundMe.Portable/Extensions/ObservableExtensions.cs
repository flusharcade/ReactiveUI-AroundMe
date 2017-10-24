// --------------------------------------------------------------------------------------------------
//  <copyright file="ObservableExtensions.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2014 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Extensions
{
	using System;
	using System.Reactive.Linq;
	using System.Reactive.Concurrency;
	using System.Reactive.Subjects;

	using ReactiveUIAroundMe.Portable.Threading;

	/// <summary>
	/// Observable extensions.
	/// </summary>
	public static class ObservableExtensions
    {
        #region Public Methods and Operators

		/// <summary>
		/// Exclusives the latest.
		/// </summary>
		/// <returns>The latest.</returns>
		/// <param name="source">Source.</param>
		/// <param name="section">Section.</param>
		/// <param name="scheduler">Scheduler.</param>
		/// <param name="selectMany">Select many.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <typeparam name="TResult">The 2nd type parameter.</typeparam>
		public static IObservable<TResult> ExclusiveLatest<T, TResult>(this IObservable<T> source, ExclusiveSection section, 
		                                                               IScheduler scheduler, Func<T, IObservable<TResult>> selectMany)
		{
			return Observable.Create<TResult>(observer => {
				Action<IObserver<TResult>, T> action = null;

				var latest = default(T);
				action = (o, value) => {
					if (!section.TryEnter ()) {
						return;
					}

					var s = new Subject<TResult>();
					s.Subscribe(o.OnNext);
					s.Subscribe(
						_ => {}, 
						() => {
							if (!section.ExitClean ()) {
								var x = latest;
								scheduler.Schedule(() => action(o, x));
							}
						});

					selectMany(value).Subscribe(s);
				};

				return source.Finally(() => 
				{
					System.Diagnostics.Debug.WriteLine("Done listening to inputs");
				}).Subscribe(v => {
					latest = v;
					action(observer, v);
				}, ex => {
					System.Diagnostics.Debug.WriteLine(ex.ToString());	
				});
			});
		}

        #endregion
    }
}