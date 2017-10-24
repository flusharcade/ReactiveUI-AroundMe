namespace ReactiveUIAroundMe.Portable.UI
{
	/// <summary>
	/// Path locator.
	/// </summary>
	public interface IPathLocator
	{
		/// <summary>
		/// Gets the path.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <param name="type">Type.</param>
		string GetPath(string fileName, string type);
	}
}
