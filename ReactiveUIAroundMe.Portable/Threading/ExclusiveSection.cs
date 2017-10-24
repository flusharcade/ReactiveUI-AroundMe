// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExclusiveSection.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Threading
{
	using System.Threading;

	/// <summary>
	/// Exclusive section.
	/// </summary>
	public sealed class ExclusiveSection
	{
		#region Constants and Fields

		/// <summary>
		/// The entry attempts.
		/// </summary>
		private long entryAttempts;

		#endregion

		/// <summary>
		/// Gets the number of entry attempts. DO NOT RELY ON THIS. Only used for diagnostics to get approximate values
		/// </summary>
		public long DiagnosticOnlyEntryAttempts
		{
			get
			{
				return this.entryAttempts;
			}
		}

		/// <summary>
		/// Cans the enter.
		/// </summary>
		/// <returns><c>true</c>, if enter was caned, <c>false</c> otherwise.</returns>
		public bool CanEnter()
		{
			return Interlocked.CompareExchange(ref this.entryAttempts, 0, 0) == 0;
		}

		/// <summary>
		/// Cans the exit.
		/// </summary>
		/// <returns><c>true</c>, if exit was caned, <c>false</c> otherwise.</returns>
		public bool CanExit()
		{
            return Interlocked.CompareExchange(ref this.entryAttempts, 0, 0) == 1;
		}

		/// <summary>
		/// Exits the clean.
		/// </summary>
		/// <returns><c>true</c>, if clean was exited, <c>false</c> otherwise.</returns>
		public bool ExitClean()
		{
            return Interlocked.CompareExchange(ref this.entryAttempts, 0, 0) == 1;
		}

		/// <summary>
		/// Tries the enter.
		/// </summary>
		/// <returns><c>true</c>, if enter was tryed, <c>false</c> otherwise.</returns>
		public bool TryEnter()
		{
			return Interlocked.Increment(ref this.entryAttempts) == 1;
		}

		/// <summary>
		/// Tries the exit.
		/// </summary>
		/// <returns><c>true</c>, if exit was tryed, <c>false</c> otherwise.</returns>
		public bool TryExit()
		{
			long result = Interlocked.Decrement(ref this.entryAttempts);
			if (result == 0)
			{
				return true;
			}
			else if (result < 0)
			{
				// Someone beat us to the punch, reset to the counter 
				// to Zero to ensure we are able to enter again properly
				this.ExitClean();
				return false;
			}

			return false;
		}
	}
}