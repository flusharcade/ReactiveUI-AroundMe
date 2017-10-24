// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorable.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.DataAccess
{
	/// <summary>
	/// The storable interface.
	/// </summary>
	public interface IStorable
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        string Key { get; set; }

		#endregion

		/// <summary>
		/// Builds the create table query.
		/// </summary>
		/// <returns>The create table query.</returns>
		string BuildCreateTableQuery();

		/// <summary>
		/// Apply the specified reader.
		/// </summary>
		/// <param name="reader">Reader.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		void Apply<T>(object reader);
	}
}