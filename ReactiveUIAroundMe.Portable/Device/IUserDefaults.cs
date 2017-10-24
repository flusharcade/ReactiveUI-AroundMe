// --------------------------------------------------------------------------------------------------
//  <copyright file="IUserDefaults.cs" company="Champion Data">
//    Copyright (c) 2015 Champion Data All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.Common
{
    /// <summary>
    /// The user defaults interface.
    /// </summary>
    public interface IUserDefaults
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:ReactiveUIAroundMe.Portable.Common.IUserDefaults"/> is authenticated.
        /// </summary>
        /// <value><c>true</c> if authenticated; otherwise, <c>false</c>.</value>
        bool Authenticated { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        string Host { get; set; }

        /// <summary>
        /// Removes from defaults.
        /// </summary>
        /// <param name="key">Key.</param>
        void RemoveFromDefaults(string key);

        /// <summary>
        /// Gets the md5 hash.
        /// </summary>
        /// <returns>The md5 hash.</returns>
        /// <param name="input">Input.</param>
        string GetMd5Hash(string input);

        /// <summary>
        /// Gets the md5 value.
        /// </summary>
        /// <returns>The md5 value.</returns>
        /// <param name="hash">Hash.</param>
        bool VerifyMd5Hash(string input, string hash);
    }
}