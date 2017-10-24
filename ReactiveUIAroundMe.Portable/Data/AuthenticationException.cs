namespace ReactiveUIAroundMe.Data
{
	using System;
	using System.Net;
	using System.Net.Http;

	using ReactiveUIAroundMe.Portable.Resources;

	public sealed class AuthenticationException : BaseException
    {
		/// <summary>
		/// </summary>
		public override string ExceptionMessage {get; set;}

        #region Constructors and Destructors

        /// <summary>
		/// Initializes a new instance of the <see cref="ConnectionException"/> class.
        /// </summary>
		public AuthenticationException()
        {
			ExceptionMessage = Labels.AuthExceptionMessage;
        }

        #endregion
    }
}