﻿using System;
using CoreGraphics;
using UIKit;

namespace ReactiveUIAroundMe.iOS.UI
{
	/// <summary>
	/// Class DragDropEventArgs.
	/// </summary>
	public class DragDropEventArgs : EventArgs
	{
		#region Initialization
		/// <summary>
		/// Initializes a new instance of the <see cref="DragDropEventArgs" /> class.
		/// </summary>
		/// <param name="state">The state.</param>
		/// <param name="point">The point.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="viewWasAt">Where the view was at.</param>
		public DragDropEventArgs(UIGestureRecognizerState state, CGPoint point, CGPoint delta, CGPoint viewWasAt, CGPoint downAt)
		{
			State = state;
			Point = point;
			Delta = delta;
			ViewWasAt = viewWasAt;
			DownAt = downAt;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <value>The state.</value>
		public UIGestureRecognizerState State { get; private set; }

		/// <summary>
		/// Gets the point.
		/// </summary>
		/// <value>The point.</value>
		public CGPoint Point { get; private set; }

		/// <summary>
		/// Gets the change in position since the gesture began.
		/// </summary>
		/// <value>The delta.</value>
		public CGPoint Delta { get; private set; }

		/// <summary>
		/// Gets where the view was at.
		/// </summary>
		/// <value>Where the view was at.</value>
		public CGPoint ViewWasAt { get; set; }

		/// <summary>
		/// Gets the point.
		/// </summary>
		/// <value>The point.</value>
		public CGPoint DownAt { get; private set; }

		#endregion
	}
}