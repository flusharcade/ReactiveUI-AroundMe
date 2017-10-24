//// --------------------------------------------------------------------------------------------------
////  <copyright file="SuperAdminPage.cs" company="Flush Arcade.">
////    Copyright (c) 2014 Flush Arcade. All rights reserved.
////  </copyright>
//// --------------------------------------------------------------------------------------------------

//namespace ReactiveUIAroundMe.Droid.Views
//{
//	using System.Linq;

//	using UIKit;
//	using CoreGraphics;
//	using ReactiveUI;

//	using ReactiveUIAroundMe.iOS;
//	using ReactiveUIAroundMe.Droid.Extras;
//	using ReactiveUIAroundMe.Portable.ViewModels;
//	using ReactiveUIAroundMe.Portable.Common;

//	/// <summary>
//	/// Super admin page.
//	/// </summary>
//	public class SuperAdminPage : BaseMvxViewController, IViewFor<SuperAdminPageViewModel>
//	{
//		/// <summary>
//		/// The view model.
//		/// </summary>
//		SuperAdminPageViewModel _viewModel;

//		/// <summary>
//		/// Gets or sets the view model.
//		/// </summary>
//		/// <value>The view model.</value>
//		public new SuperAdminPageViewModel ViewModel
//		{
//			get { return _viewModel; }
//			set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
//		}

//		/// <summary>
//		/// Gets or sets the reactive user interface . IV iew for. view model.
//		/// </summary>
//		/// <value>The reactive user interface . IV iew for. view model.</value>
//		object IViewFor.ViewModel
//		{
//			get { return ViewModel; }
//			set { ViewModel = (SuperAdminPageViewModel)value; }
//		}

//		/// <summary>
//		/// The available collection view.
//		/// </summary>
//		private UICollectionView _tilesCollectionView;

//		/// <summary>
//		/// The umpire source.
//		/// </summary>
//		private TetrixCollectionSource _tilesSource;

//		/// <summary>
//		/// Views the did load.
//		/// </summary>
//		public override void ViewDidLoad()
//		{
//			base.ViewDidLoad();

//			base.StyleNavigationBar();

//			_tilesCollectionView = new UICollectionView(new CGRect(), new TetrixLayout())
//			{
//				TranslatesAutoresizingMaskIntoConstraints = false,
//				BackgroundColor = UIColor.White
//			};
//			_tilesCollectionView.RegisterClassForCell(typeof(TileCollectionViewCell), TileCollectionViewCell.Key);

//			_tilesSource = new TetrixCollectionSource(_tilesCollectionView, TileCollectionViewCell.Key,
//															(item, collectionView, indexPath) =>
//															{
//																var cell = default(BaseCollectionViewCell);

//																TypeSwitch.On(item)
//																	.Case((TileViewModel x) => cell = (BaseCollectionViewCell)collectionView.DequeueReusableCell(TileCollectionViewCell.Key, indexPath));

//																//cell.DataContext = item;

//																return cell;
//															});
//			_tilesCollectionView.Source = _tilesSource;

//			Add(_tilesCollectionView);

//			var views = new DictionaryViews()
//			{
//				{"tilesCollectionView", _tilesCollectionView}
//			};

//			View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|-20-[tilesCollectionView]-20-|", NSLayoutFormatOptions.DirectionLeftToRight, null, views)
//				.Concat(NSLayoutConstraint.FromVisualFormat("H:|-20-[tilesCollectionView]-20-|", NSLayoutFormatOptions.AlignAllTop, null, views))
//				.ToArray());
			
//			this.WhenActivated(d =>
//			{
//				// We need to bind the ViewModel property to the DataContext in order to be able to
//				// use WPF Bindings. Let's use WPF bindings for the UserName property.
//				//this.Bind(ViewModel, vm => vm.Cells, v => v._tilesSource.ItemsSource);

//				//set.Bind(_tilesSource).For(s => s.ItemsSource).To(vm => vm.Cells);
//				//set.Bind(_tilesSource).For(cv => cv.SelectionChangedCommand)
//				//   .To(vm => vm.SelectCommand);
				
//				//OnAppear(new Dictionary<string, object>());
//			});
//		}
//	}
//}