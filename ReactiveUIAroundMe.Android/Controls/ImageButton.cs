// --------------------------------------------------------------------------------------------------
//  <copyright file="FlipButton.cs" company="Flush Arcade.">
//    Copyright (c) 2014 Flush Arcade. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Droid.Controls
{
	using System;

    using Android.Widget;
    using Android.Runtime;

    /// <summary>
    /// Image button.
    /// </summary>
    public class FlipButton : ImageButton
    {
        /// <summary>
        /// The is flipped.
        /// </summary>
        public bool IsFlipped;

        /// <summary>
        /// The index of the flip.
        /// </summary>
        public int FlipIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Droid.Controls.ImageButton"/> class.
        /// </summary>
        public FlipButton(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
            //_transparentBackground = new UIView()
            //{
            //	BackgroundColor = UIColor.White,
            //	UserInteractionEnabled = false,
            //	Alpha = 0.05f
            //};

            //InsertSubview(_transparentBackground, 0);
        }
    }
}
