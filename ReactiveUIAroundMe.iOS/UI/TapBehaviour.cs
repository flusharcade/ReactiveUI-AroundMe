// --------------------------------------------------------------------------------------------------
//  <copyright file="TapBehaviour.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.iOS
{
	using System;
	using System.Windows.Input;

	using UIKit;

	/// <summary>
	/// Tap behaviour.
	/// </summary>
	public class TapBehaviour<T>
	{
		//public ReactiveCommand<T> Command { get; set; }

		//public TapBehaviour(UIView view)
		//{
		//	var tap = new UITapGestureRecognizer(() =>
		//	{
		//		var command = Command;
		//		if (command != null)
		//			command.Execute(null);
		//	});
		//	view.AddGestureRecognizer(tap);
		//}
	}

	public class TapBehaviour
	{
		public ICommand Command { get; set; }

		public TapBehaviour(UIView view)
		{
			var tap = new UITapGestureRecognizer(() =>
			{
				var command = Command;
				if (command != null)
					command.Execute(null);
			});
			view.AddGestureRecognizer(tap);
		}
	}

	/// <summary>
	/// Behaviour extensions.
	/// </summary>
	public static class BehaviourExtensions
	{
		//public static TapBehaviour<T> Tap<T>(this UIView view)
		//{
		//	return new TapBehaviour<T>(view);
		//}

		public static TapBehaviour Tap(this UIView view)
		{
			return new TapBehaviour(view);
		}
	}
}
