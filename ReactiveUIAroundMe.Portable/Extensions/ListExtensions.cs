// --------------------------------------------------------------------------------------------------
//  <copyright file="ListExtensions.cs" company="Flush Arcade Pty Ltd.">
//    Copyright (c) 2014 Flush Arcade Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// List extensions.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Adds all.
        /// </summary>
        /// <param name="hashSet">Hash set.</param>
        /// <param name="items">Items.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void AddAll<T>(this ObservableCollection<T> hashSet, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                hashSet.Add(item);
            }
        }

        #endregion
    }
}