namespace ReactiveUIAroundMe.Data
{
	using System;
    using System.Net;
    using System.Net.Http;

	using ReactiveUIAroundMe.Portable.Resources;

    public abstract class BaseException : Exception
    {
		/// <summary>
		/// </summary>
		public abstract string ExceptionMessage {get; set;}

        #region Constructors and Destructors

        /// <summary>
		/// Initializes a new instance of the <see cref="ConnectionException"/> class.
        /// </summary>
		public BaseException()
        {
        }

        #endregion
    }
}