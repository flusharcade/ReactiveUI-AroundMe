//// --------------------------------------------------------------------------------------------------
////  <copyright file="BaseTableViewCell.cs" company="Flush Arcade.">
////    Copyright (c) 2014 Flush Arcade. All rights reserved.
////  </copyright>
//// --------------------------------------------------------------------------------------------------

//namespace ReactiveUIAroundMe.Droid.Controls
//{
//	using System;
//	using System.Collections.Generic;

//	using ReactiveUI;

//	using ReactiveUIAroundMe.Portable.ViewModels;

//	/// <summary>
//	/// Gradient table view cell.
//	/// </summary>
//	public class BaseTableViewCell : ReactiveTableViewCell
//	{
//		public override bool Selected
//		{
//			get { return base.Selected; }
//			set
//			{
//				base.Selected = value;
//				UpdateSelectedStyle(value);
//			}
//		}

//		/// <summary>
//		/// The gradients.
//		/// </summary>
//		private IList<CAGradientLayer> gradients = new List<CAGradientLayer>();

//		/// <summary>
//		/// The views.
//		/// </summary>
//		private IList<UIView> views = new List<UIView>();

//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.GradientMvxCollectionViewCell"/> class.
//		/// </summary>
//		/// <param name="handle">Handle.</param>
//		protected BaseTableViewCell(IntPtr handle) : base(handle)
//		{

//		}

//		/// <summary>
//		/// Updates the selected style.
//		/// </summary>
//		/// <param name="selected">If set to <c>true</c> selected.</param>
//		protected virtual void UpdateSelectedStyle(bool selected)
//		{
//			for (int i = 0; i < gradients.Count; i++)
//			{
//				gradients[i].Opacity = selected ? 0f : 1f;
//			} 
//		}

//		/// <summary>
//		/// Views the will layout subviews.
//		/// </summary>
//		public virtual void UpdateSubViews(CGRect bounds)
//		{
//			// because we are using auto layout, gradient layers do not update width and height properties
//			// we must hard set the frame of each gradient to its parent view everytime time this function is called
//			for (int i = 0; i < gradients.Count; i++)
//			{
//				gradients[i].Frame = new CGRect(0, 0, bounds.Width, views[i].Frame.Height);
//			}
//		}

//		/// <summary>
//		/// Creates the view gradient.
//		/// </summary>
//		/// <param name="view">View.</param>
//		public void CreateViewGradient(UIView view, nfloat viewHeight)
//		{
//			var gradient = new CAGradientLayer();

//			// HACK: Stupid heights needs to be set, collection view fails to determine this dynamically.
//			var frame = view.Bounds;
//			frame.Height = viewHeight;
//			view.Bounds = frame;

//			gradient.Frame = view.Bounds;
//			gradient.NeedsDisplayOnBoundsChange = true;
//			gradient.MasksToBounds = true;

//			view.BackgroundColor = UIColor.White;

//			gradient.Colors = new CGColor[]
//			{
//				UIColor.FromRGB(253, 253, 253).CGColor,
//				UIColor.FromRGB(227, 227, 228).CGColor
//			};

//			gradients.Add(gradient);
//			views.Add(view);

//			view.Layer.AddSublayer(gradient);
//		}

//		/// <summary>
//		/// Removes the gradient.
//		/// </summary>
//		/// <param name="view">View.</param>
//		public void RemoveGradient(UIView view)
//		{
//			foreach (var gradient in gradients)
//			{
//				gradient.RemoveFromSuperLayer();
//			}
//		}

//		/// <summary>
//		/// Initialize this instance.
//		/// </summary>
//		public virtual void Initialize()
//		{
//		}
//	}
//}
