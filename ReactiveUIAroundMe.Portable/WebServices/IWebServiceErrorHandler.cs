using System;
using System.Threading.Tasks;

namespace ReactiveUIAroundMe.Portable.WebServices
{
	public interface IWebServiceErrorHandler
	{
		bool RefreshAndRetryOnExpiration { get; set; }
		bool HandleException(Exception exception);
	}
}