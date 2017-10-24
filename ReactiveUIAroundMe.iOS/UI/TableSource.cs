
namespace ReactiveUIAroundMe.iOS.UI
{
	using UIKit;

	using ReactiveUI;

	using ReactiveUIAroundMe.iOS.Controls;

	/// <summary>
	/// Table source.
	/// </summary>
	public class TableSource : ReactiveTableViewSource<BaseTableViewCell>
	{
		/// <summary>
		/// The cell identifier.
		/// </summary>
		private string _cellIdentifier;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.iOS.TableSource"/> class.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		public TableSource(UITableView tableView, string cellIdentifier)
            : base(tableView)
        {
			_cellIdentifier = cellIdentifier;
		}

		/// <summary>
		/// Gets the or create cell for.
		/// </summary>
		/// <returns>The or create cell for.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		/// <param name="item">Item.</param>
		//protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		//{
		//	var cell = (BaseTableViewCell)tableView.DequeueReusableCell(_cellIdentifier, indexPath);
		//	return cell;
		//}
	}
}
