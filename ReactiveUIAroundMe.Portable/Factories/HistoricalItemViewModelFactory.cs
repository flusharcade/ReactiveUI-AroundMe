//using System;

//using ReactiveUIAroundMe.Portable.ViewModels;

//namespace ReactiveUIAroundMe.Factories
//{
//	/// <summary>
//	/// Historical item view model factory.
//	/// </summary>
//	public class HistoricalItemViewModelFactory
//	{
//		/// <summary>
//		/// The container.
//		/// </summary>
//		private UnityContainer _container;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Factories.HistoricalItemViewModelFactory"/> class.
//		/// </summary>
//		/// <param name="container">Container.</param>
//		public HistoricalItemViewModelFactory(UnityContainer container)
//		{
//			_container = container;
//		}

//		/// <summary>
//		/// Create this instance.
//		/// </summary>
//		public HistoricalItemViewModel Create()
//		{
//			return _container.Resolve<HistoricalItemViewModel>();
//		}
//	}
//}