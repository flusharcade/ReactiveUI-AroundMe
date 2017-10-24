using System;

namespace ReactiveUIAroundMe.Portable.WebServices
{
	public enum ErrorCodes : int {
		Unknown = -1,
		Success = 0,
		InvalidAuthCredentials = 1,
		InvalidJson = 3,
		InvalidAuthToken = 4,
		InvalidAppAuthToken = 5,
		InvalidRecordId = 6,
		InvalidItemXml = 7,
		ExpiredToken = 8,
		WriteXmlError = 9,
		ParseXmlError = 10,
		FailedToGetPersonInfo = 11,
		FilterIsNull = 12,
		NoItemsFound = 13
	}
}