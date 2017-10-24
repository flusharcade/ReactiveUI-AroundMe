// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TouchCollectionView.cs" company="Champion Data Pty Ltd.">
//   Copyright (c) 2015 Champion Data Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS
{
	using System;
	using System.Timers;
	using CoreGraphics;
	using Foundation;
	using ReactiveUIAroundMe.iOS.UI;
	using UIKit;

	/// <summary>
	/// Touch collection view.
	/// </summary>
	public class TouchCollectionView : UICollectionView
	{
		#region Constants

		/// <summary>
		/// The default duration
		/// </summary>
		public const int DefaultDuration = 500;

		/// <summary>
		/// The default movement threshold
		/// </summary>
		public const double DefaultMovementThreshold = 10.0;

		#endregion

		#region Fields

		/// <summary>
		/// The time to hold-to-drag in milliseconds before qualifying.
		/// </summary>
		public static int HoldToBeginThresholdMilliseconds = DefaultDuration;

		/// <summary>
		/// The movement threshold.
		/// </summary>
		public static double MovementThreshold = DefaultMovementThreshold;

		/// <summary>
		/// The action.
		/// </summary>
		private readonly Action<DragDropGestureRecognizer> _action;

		/// <summary>
		/// The timer.
		/// </summary>
		private Timer _timer;

		#endregion

		/// <summary>
		/// Occurs when touch.
		/// </summary>
		public event EventHandler<CGPoint> Touched;

		/// <summary>
		/// Gets a value indicating whether a long press sub-gesture has been completed.
		/// </summary>
		/// <value><c>true</c> if a long press sub-gesture has been completed; otherwise, <c>false</c>.</value>
		public bool DidLongPress { get; private set; }

		/// <summary>
		/// Gets a value indicating whether a drag operation has occured since the long press sub-gesture was completed.
		/// </summary>
		/// <value><c>true</c> if a drag operation has occured; otherwise, <c>false</c>.</value>
		public bool DidDrag { get; private set; }

		/// <summary>
		/// Gets the point at which the gesture began.
		/// </summary>
		/// <value>The begin point</value>
		public CGPoint DownAt { get; private set; }

		/// <summary>
		/// Gets the point at which the last known drag operation occured.
		/// </summary>
		/// <value>The drag point.</value>
		public CGPoint DragAt { get; private set; }

		/// <summary>
		/// Gets the point at which the view was when the gesture began.
		/// </summary>
		/// <value>The view was at.</value>
		public CGPoint ViewWasAt { get; private set; }

		/// <summary>
		/// Gets the change in position since the gesture began.
		/// </summary>
		/// <value>The delta.</value>
		public CGPoint Delta
		{
			get { return new CGPoint(DragAt.X - DownAt.X, DragAt.Y - DownAt.Y); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="DragDropGestureRecognizer"/> is active.
		/// </summary>
		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
		public bool Active { get { return DidDrag; } }

		/// <summary>
		/// The state.
		/// </summary>
		private UIGestureRecognizerState _state;

		/// <summary>
		/// The current state of this UIGestureRecognizer. Read-only.
		/// </summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public UIGestureRecognizerState State
		{
			get { return _state; }
			set
			{
				_state = value;
				//if (_action != null)
				//	_action(this);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.TouchCollectionView"/> class.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="layout">Layout.</param>
		public UIView WindowView;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.TouchCollectionView"/> class.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="layout">Layout.</param>
		public TouchCollectionView(CGRect rect, UICollectionViewLayout layout) : base(rect, layout)
		{
		}

		/// <summary>
		/// Toucheses the began.
		/// </summary>
		/// <param name="touches">Touches.</param>
		/// <param name="evt">Evt.</param>
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			// Get the current touch
			var touch = touches.AnyObject as UITouch;
			if (touch != null)
			{
				Touched?.Invoke(this, new CGPoint(touch.LocationInView(WindowView).X, touch.LocationInView(WindowView).Y));
			}
		}

		/// <summary>
		/// Inboked when a touch gesture moves.
		/// </summary>
		/// <param name="touches">To be added.</param>
		/// <param name="evt">To be added.</param>
		/// <remarks>To be added.</remarks>
		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			base.TouchesMoved(touches, evt);

			if (State == UIGestureRecognizerState.Failed)
				return;

			// After long press:
			if (DidLongPress)
			{
				var dragat = GetTouchPoint();
				if (dragat == DragAt)
					return; // Not noteworthy.

				DragAt = dragat;

				if (!StayedPut(DownAt, DragAt))
				{
					DidDrag = true;
					//OnDragging(this, new DragDropEventArgs(State, DragAt, Delta, ViewWasAt));
					State = UIGestureRecognizerState.Changed;
				}
			}

			// Before long press:
			else
			{
				if (StayedPut(GetTouchPoint(), DownAt))
					return;

				if (_timer != null)
					_timer.Dispose();
				_timer = null;
				State = UIGestureRecognizerState.Failed;
			}
		}

		/// <summary>
		/// Gets the touch point.
		/// </summary>
		/// <returns>CGPoint.</returns>
		private CGPoint GetTouchPoint()
		{
			return new CGPoint(0, 0);
			//return LocationInView(Superview);
		}

		/// <summary>
		/// Checks if movement has exceeded the movement threshold.
		/// </summary>
		/// <param name="current">The current point.</param>
		/// <param name="previous">The previous point.</param>
		/// <returns><c>true</c> if movement has exceeded the movement threshold, <c>false</c> otherwise.</returns>
		private bool StayedPut(CGPoint current, CGPoint previous)
		{
			return Distance(current, previous) < MovementThreshold;
		}

		/// <summary>
		/// Measures the distance between two points.
		/// </summary>
		/// <param name="point1">The point1.</param>
		/// <param name="point2">The point2.</param>
		/// <returns>System.Single.</returns>
		private float Distance(CGPoint point1, CGPoint point2)
		{
			var dx = point1.X - point2.X;
			var dy = point1.Y - point2.Y;

			return (float)Math.Sqrt(dx * dx + dy * dy);
		}
	}
}