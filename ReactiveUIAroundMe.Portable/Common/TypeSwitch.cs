// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeSwitch.cs" company="Champion Data">
//   Copyright (c) 2015 Champion Data All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Common
{
	using System;

	/// <summary>
	/// Type switch.
	/// </summary>
	public static class TypeSwitch
	{
		/// <summary>
		/// Raises the  event.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		public static Switch<TSource> On<TSource>(TSource value)
		{
			return new Switch<TSource>(value);
		}

		/// <summary>
		/// Raises the  event.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		public static Switch OnVoid<TSource>(TSource value)
		{
			return new Switch(value);
		}

		/// <summary>
		/// Switch.
		/// </summary>
		public sealed class Switch<TSource>
		{
			/// <summary>
			/// The value.
			/// </summary>
			private readonly TSource value;

			/// <summary>
			/// The value.
			/// </summary>
			private readonly object obj;

			/// <summary>
			/// The handled.
			/// </summary>
			private bool handled = false;

			/// <summary>
			/// Initializes a new instance of the <see cref="TelstraHealth.Portable.Type.TypeSwitch+Switch`1"/> class.
			/// </summary>
			/// <param name="value">Value.</param>
			internal Switch(TSource value)
			{
				this.value = value;
			}

			/// <summary>
			/// Case the specified action.
			/// </summary>
			/// <param name="action">Action.</param>
			/// <typeparam name="TTarget">The 1st type parameter.</typeparam>
			public Switch<TSource> Case<TTarget>(Action<TTarget> action) where TTarget : TSource
			{
				if (!this.handled && this.value is TTarget)
				{
					action((TTarget) this.value);
					this.handled = true;
				}

				return this;
			}
				
			/// <summary>
			/// Default the specified action.
			/// </summary>
			/// <param name="action">Action.</param>
			public void Default(Action<TSource> action)
			{
				if (!this.handled) 
				{
					action (this.value);
				}
			}
		}

		/// <summary>
		/// Switch.
		/// </summary>
		public sealed class Switch
		{
			/// <summary>
			/// The handled.
			/// </summary>
			private bool handled = false;

			/// <summary>
			/// The value.
			/// </summary>
			private object value;

			/// <summary>
			/// Initializes a new instance of the <see cref="TelstraHealth.Portable.Type.TypeSwitch+Switch"/> class.
			/// </summary>
			/// <param name="value">Value.</param>
			internal Switch(object value)
			{
				this.value = value;
			}

			/// <summary>
			/// Case the specified action.
			/// </summary>
			/// <param name="action">Action.</param>
			/// <typeparam name="TTarget">The 1st type parameter.</typeparam>
			public Switch Case<TTarget>(Action<TTarget> action)
			{
				if (!this.handled && this.value is TTarget)
				{
					action((TTarget) this.value);
					this.handled = true;
				}

				return this;
			}
		}
	}
}