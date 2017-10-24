//// --------------------------------------------------------------------------------------------------
////  <copyright file="UIColorExtensions.cs" company="Flush Arcade.">
////    Copyright (c) 2014 Flush Arcade. All rights reserved.
////  </copyright>
//// --------------------------------------------------------------------------------------------------

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Linq;
//using System.Reactive;
//using ReactiveUI;
//using UIKit;

//namespace ReactiveUIAroundMe.iOS.Extensions
//{
//	/// <summary>
//	/// UIC olor extensions.
//	/// </summary>
//	public static class ReactiveListExtensions
//	{
//		#region Methods

//        /// <summary>
//        /// Groups the by.
//        /// </summary>
//        /// <returns>The by.</returns>
//        /// <param name="source">Source.</param>
//        /// <param name="keySelector">Key selector.</param>
//        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
//        /// <typeparam name="TElement">The 2nd type parameter.</typeparam>
//		static IReactiveList<IReactiveGrouping<TKey, TElement>> ReactiveGroupBy<TKey, TElement>(this IReactiveList<TElement> source, 
//                                                                                Func<TElement, TKey> keySelector)
//		{
//			//Grouping elements in the dictionary according to the criteria
//			var dict = new Dictionary<TKey, IReactiveList<TElement>>();

//			//Filling the dictionary. It will contain: [Key -> List<Values>]
//			foreach (var x in source)
//			{
//				var key = keySelector(x);
//				if (dict.Keys.Contains(key))
//				{
//					dict[key].Add(x);
//				}
//				else
//				{
//					dict.Add(key, new ReactiveList<TElement> { x });
//				}
//			}

//			//For each group...
//			foreach (var x in dict)
//			{
//				yield return new ReactiveGrouping<TKey, TElement>(x.Key, x.Value);
//			}
//		}

//        /// <summary>
//        /// Grouping.
//        /// </summary>
//		class ReactiveGrouping<TKey, TElement> : IReactiveGrouping<TKey, TElement>
//		{
//			private TKey _key;
//			private IReactiveList<TElement> _elements;

//            public event NotifyCollectionChangedEventHandler CollectionChanged;
//            public event NotifyCollectionChangedEventHandler CollectionChanging;
//            public event PropertyChangedEventHandler PropertyChanged;
//            public event ReactiveUI.PropertyChangingEventHandler PropertyChanging;

//            public ReactiveGrouping(TKey key, IReactiveList<TElement> elements)
//            {
//                _key = key;
//                _elements = elements;
//            }

//            public IEnumerator<TElement> GetEnumerator()
//            {
//                return _elements.GetEnumerator();
//            }

//            IEnumerator IEnumerable.GetEnumerator()
//            {
//                return GetEnumerator();
//            }

//            public void AddRange(IEnumerable<object> collection)
//            {
//                throw new NotImplementedException();
//            }

//            public void InsertRange(int index, IEnumerable<object> collection)
//            {
//                throw new NotImplementedException();
//            }

//            public void RemoveAll(IEnumerable<object> items)
//            {
//                throw new NotImplementedException();
//            }

//            public void RemoveRange(int index, int count)
//            {
//                throw new NotImplementedException();
//            }

//            public void Sort(IComparer<object> comparer = null)
//            {
//                throw new NotImplementedException();
//            }

//            public void Sort(Comparison<object> comparison)
//            {
//                throw new NotImplementedException();
//            }

//            public void Sort(int index, int count, IComparer<object> comparer)
//            {
//                throw new NotImplementedException();
//            }

//            public void Reset()
//            {
//                throw new NotImplementedException();
//            }

//            public IDisposable SuppressChangeNotifications()
//            {
//                throw new NotImplementedException();
//            }

//            public void RaisePropertyChanging(ReactiveUI.PropertyChangingEventArgs args)
//            {
//                throw new NotImplementedException();
//            }

//            public void RaisePropertyChanged(PropertyChangedEventArgs args)
//            {
//                throw new NotImplementedException();
//            }

//            public int IndexOf(object item)
//            {
//                throw new NotImplementedException();
//            }

//            public void Insert(int index, object item)
//            {
//                throw new NotImplementedException();
//            }

//            public void RemoveAt(int index)
//            {
//                throw new NotImplementedException();
//            }

//            public void Add(object item)
//            {
//                throw new NotImplementedException();
//            }

//            public void Clear()
//            {
//                throw new NotImplementedException();
//            }

//            public bool Contains(object item)
//            {
//                throw new NotImplementedException();
//            }

//            public void CopyTo(object[] array, int arrayIndex)
//            {
//                throw new NotImplementedException();
//            }

//            public bool Remove(object item)
//            {
//                throw new NotImplementedException();
//            }

//            IEnumerator<object> IEnumerable<object>.GetEnumerator()
//            {
//                throw new NotImplementedException();
//            }

//            public TKey Key
//            {
//                get { return _key; }
//            }

//            public bool IsEmpty => throw new NotImplementedException();

//            public IObservable<object> ItemsAdded => throw new NotImplementedException();

//            public IObservable<object> BeforeItemsAdded => throw new NotImplementedException();

//            public IObservable<object> ItemsRemoved => throw new NotImplementedException();

//            public IObservable<object> BeforeItemsRemoved => throw new NotImplementedException();

//            public IObservable<IMoveInfo<object>> BeforeItemsMoved => throw new NotImplementedException();

//            public IObservable<IMoveInfo<object>> ItemsMoved => throw new NotImplementedException();

//            public IObservable<NotifyCollectionChangedEventArgs> Changing => throw new NotImplementedException();

//            public IObservable<NotifyCollectionChangedEventArgs> Changed => throw new NotImplementedException();

//            public IObservable<int> CountChanging => throw new NotImplementedException();

//            public IObservable<int> CountChanged => throw new NotImplementedException();

//            public IObservable<bool> IsEmptyChanged => throw new NotImplementedException();

//            public IObservable<Unit> ShouldReset => throw new NotImplementedException();

//            public IObservable<IReactivePropertyChangedEventArgs<object>> ItemChanging => throw new NotImplementedException();

//            public IObservable<IReactivePropertyChangedEventArgs<object>> ItemChanged => throw new NotImplementedException();

//            public bool ChangeTrackingEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

//            public int Count => throw new NotImplementedException();

//            public bool IsReadOnly => throw new NotImplementedException();

//            public object this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//        }

//		public interface IReactiveGrouping<out TKey, out TElement> : IReactiveList<object>
//		{
//			//
//			// Properties
//			//
//			TKey Key
//			{
//				get;
//			}
//		}

//		#endregion
//	}
//}