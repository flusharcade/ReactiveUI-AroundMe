
namespace ReactiveUIAroundMe.iOS.Tab
{
	using System.Collections.Generic;
	using ReactiveUI;

	public interface ITabBarPresenter
	{
		bool ShowView(ReactiveViewController view, bool animated);
	}
}