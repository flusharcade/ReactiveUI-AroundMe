//using System;

//using ReactiveUIAroundMe.Portable.ViewModels;

//using Microsoft.Practices.Unity;

//namespace ReactiveUIAroundMe.Factories
//{
//	/// <summary>
//	/// Player item view model factory.
//	/// </summary>
//	public class PlayerItemViewModelFactory
//	{
//		/// <summary>
//		/// The container.
//		/// </summary>
//		private UnityContainer _container;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Factories.HistoricalItemViewModelFactory"/> class.
//		/// </summary>
//		/// <param name="container">Container.</param>
//		public PlayerItemViewModelFactory(UnityContainer container)
//		{
//			_container = container;
//		}

//		/// <summary>
//		/// Create this instance.
//		/// </summary>
//		public PlayerItemViewModel Create()
//		{
//			return _container.Resolve<PlayerItemViewModel>();
//		}
//	}
//}