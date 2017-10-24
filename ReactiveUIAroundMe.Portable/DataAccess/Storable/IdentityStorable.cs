// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityStorable.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.DataAccess
{
	using System.Reflection;

	using SQLite.Net.Attributes;

	using ReactiveUIAroundMe.Portable.WebServices;

	/// <summary>
	/// Identity storable.
	/// </summary>
	public class IdentityStorable : IStorable
    {
        #region Public Properties

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>The key.</value>
      	[PrimaryKey]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		public string Username { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the auth token.
		/// </summary>
		/// <value>The auth token.</value>
		public string WSO2Token { get; set; }

		/// <summary>
		/// Gets or sets the bearer token.
		/// </summary>
		/// <value>The bearer token.</value>
		public string AccessToken { get; set; }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ReactiveUIAroundMe.Portable.DataAccess.IdentityStorable"/> class.
		/// </summary>
		public IdentityStorable()
		{
			Key = StorableKeys.Identity.ToString();
			Username = string.Empty;
			Password = string.Empty;
			WSO2Token = string.Empty;
			AccessToken = string.Empty;
		}

		/// <summary>
		/// Apply the specified contract.
		/// </summary>
		/// <param name="contract">Contract.</param>
		public void Apply(AuthContract contract)
		{
			Username = contract.Email;
			AccessToken = contract.AccessKey;
		}

		/// <summary>
		/// Apply the specified parameters.
		/// </summary>
		/// <param name="parameters">Parameters.</param>
		//public void Apply(params object[] parameters)
		//{
		//	Username = (string)parameters[0];
		//	AccessKey = (string)parameters[1];
		//}

		/// <summary>
		/// Apply the specified reader.
		/// </summary>
		/// <param name="reader">Reader.</param>
		public void Apply<T>(object reader)
		{
			var isDBNullMethod = typeof(T).GetRuntimeMethod("IsDBNull", new System.Type[]
			{
				typeof(int)
			});

			var getStringMethod = typeof(T).GetRuntimeMethod("GetString", new System.Type[] 
			{ 
				typeof(int) 
			});

			if (!(bool)isDBNullMethod.Invoke(reader, new object[] { 1 }))
			{
				Username = (string)getStringMethod.Invoke(reader, new object[] { 1 });
			}

			if (!(bool)isDBNullMethod.Invoke(reader, new object[] { 2 }))
			{
				Password = (string)getStringMethod.Invoke(reader, new object[] { 2 });
			}

			if (!(bool)isDBNullMethod.Invoke(reader, new object[] { 3 }))
			{
				WSO2Token = (string)getStringMethod.Invoke(reader, new object[] { 3 });
			}

			if (!(bool)isDBNullMethod.Invoke(reader, new object[] { 4 }))
			{
				AccessToken = (string)getStringMethod.Invoke(reader, new object[] { 4 });
			}
		}

		/// <summary>
		/// Builds the create table query.
		/// </summary>
		/// <returns>The create table query.</returns>
		public string BuildCreateTableQuery()
		{
			return "CREATE TABLE IF NOT EXISTS IdentityStorable (Key ntext PRIMARY KEY, Username ntext, Password ntext, WSO2Token ntext, AccessToken ntext);";
		}
    }
}