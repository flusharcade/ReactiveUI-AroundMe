using ReactiveUIAroundMe.Portable.ViewModels;
using ReactiveUI;

namespace ReactiveUIAroundMe.iOS
{
	using System;
	using System.Collections.Generic;

	using CoreAnimation;
	using CoreGraphics;
	using UIKit;

	using ReactiveUIAroundMe.iOS.UI;

	/// <summary>
	/// Gradient mvx collection view cell.
	/// </summary>
	public class BaseCollectionViewCell : ReactiveCollectionViewCell
	{
		/// <summary>
		/// The drag drop super view.
		/// </summary>
		public UIView DragDropSuperView;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.iOS.BaseCollectionViewCell"/> is selected.
		/// </summary>
		/// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
		public override bool Selected
		{
			get { return base.Selected; }
			set
			{
				base.Selected = value;
				UpdateSelectedStyle(value);
			}
		}

		/// <summary>
		/// Gets the view model.
		/// </summary>
		/// <value>The view model.</value>
		//public TetrixViewModelBase ViewModel
		//{
		//	get { return base.DataContext != null ? (TetrixViewModelBase)this.DataContext : null;}
		//}

		/// <summary>
		/// The allow selected style.
		/// </summary>
		public bool AllowSelectedStyle = true;

		/// <summary>
		/// The on dropped events.
		/// </summary>
		public IDisposable DroppedEvents;

		/// <summary>
		/// The on dragging events.
		/// </summary>
		public IDisposable DraggingEvents;

		/// <summary>
		/// The gradients.
		/// </summary>
		public IList<CAGradientLayer> Gradients = new List<CAGradientLayer>();

		/// <summary>
		/// The views.
		/// </summary>
		private IList<UIView> views = new List<UIView>();

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.GradientMvxCollectionViewCell"/> class.
		/// </summary>
		/// <param name="handle">Handle.</param>
		protected BaseCollectionViewCell(IntPtr handle) : base(handle)
		{
		}

		/// <summary>
		/// Updates the selected style.
		/// </summary>
		/// <param name="selected">If set to <c>true</c> selected.</param>
		protected virtual void UpdateSelectedStyle(bool selected)
		{
			if (AllowSelectedStyle)
			{
				for (int i = 0; i < Gradients.Count; i++)
				{
					Gradients[i].Opacity = selected ? 0f : 1f;
				}
			}
		}

		/// <summary>
		/// Views the will layout subviews.
		/// </summary>
		public virtual void UpdateSubViews()
		{
			// because we are using auto layout, gradient layers do not update width and height properties
			// we must hard set the frame of each gradient to its parent view everytime time this function is called
			for (int i = 0; i < Gradients.Count; i++)
			{
				Gradients[i].Frame = new CGRect(0, 0, Frame.Width, views[i].Frame.Height);
			}
		}

		/// <summary>
		/// Creates the view gradient.
		/// </summary>
		/// <param name="view">View.</param>
		public void CreateViewGradient(UIView view, nfloat viewHeight)
		{
			var gradient = new CAGradientLayer();

			// HACK: Stupid heights needs to be set, collection view fails to determine this dynamically.
			var frame = view.Bounds;
			frame.Height = viewHeight;
			view.Bounds = frame;

			gradient.Frame = view.Bounds;
			gradient.NeedsDisplayOnBoundsChange = true;
			gradient.MasksToBounds = true;

			gradient.Colors = new CGColor[]
			{
				UIColor.FromRGB(253, 253, 253).CGColor,
				UIColor.FromRGB(227, 227, 228).CGColor
			};

			Gradients.Add(gradient);
			views.Add(view);

			view.Layer.AddSublayer(gradient);
		}

		/// <summary>
		/// Removes the gradient.
		/// </summary>
		/// <param name="view">View.</param>
		public void RemoveGradient(UIView view)
		{
			foreach (var gradient in Gradients)
			{
				gradient.RemoveFromSuperLayer();
			}
		}

		/// <summary>
		/// Disposes the events.
		/// </summary>
		public void DisposeEvents()
		{
			if (DroppedEvents != null)
			{
				DroppedEvents.Dispose();
			}

			if (DraggingEvents != null)
			{
				DraggingEvents.Dispose();
			}
		}

		/// <summary>
		/// Inits the drag drop.
		/// </summary>
		public void InitDragDrop(DragDropGestureRecognizer dragDropGestureRecognizer)
		{
			AddGestureRecognizer(dragDropGestureRecognizer);
		}
	}
}