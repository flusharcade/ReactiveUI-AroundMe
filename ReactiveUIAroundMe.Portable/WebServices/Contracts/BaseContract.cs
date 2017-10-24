namespace ReactiveUIAroundMe.Portable.WebServices
{
	using Newtonsoft.Json;

	/// <summary>
	/// Base contract.
	/// </summary>
	public abstract class BaseContract
	{
		public string Serialize()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}