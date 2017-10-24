// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationStateStorable.cs" company="Flush Arcade Pty Ltd.">
//   Copyright (c) 2015 Flush Arcade Pty Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ReactiveUIAroundMe.Portable.DataAccess
{
	using SQLite.Net.Attributes;

	using ReactiveUIAroundMe.Portable.Common;
	using System.Reflection;

	/// <summary>
	/// File storable.
	/// </summary>
	public class ApplicationStateStorable : IStorable
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>The key.</value>
		[PrimaryKey]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the name of the team.
		/// </summary>
		/// <value>The name of the team.</value>
		public string TeamName { get; set; }

		/// <summary>
		/// Gets or sets the fixture identifier.
		/// </summary>
		/// <value>The fixture identifier.</value>
		public int MatchId { get; set; }

		/// <summary>
		/// Gets or sets the squad identifier.
		/// </summary>
		/// <value>The squad identifier.</value>
		public int SquadId { get; set; }

		#endregion

		/// <summary>
		/// Apply this instance.
		/// </summary>
		public void Apply(ApplicationStateHandler applicationStateHandler)
		{
		}

		/// <summary>
		/// Apply the specified parameters.
		/// </summary>
		/// <param name="parameters">Parameters.</param>
		//public void Apply(params object[] parameters)
		//{
		//	TeamName = (string)parameters[0];
		//	FixtureId = (int)parameters[0];
		//	SquadId = (int)parameters[0];
		//}

		public void Apply<T>(object reader)
		{
			var getStringMethod = typeof(T).GetRuntimeMethod("GetString", new System.Type[]
			{
				typeof(int)
			});

			TeamName = (string)getStringMethod.Invoke(reader, new object[] { 0 });
			MatchId = (int)getStringMethod.Invoke(reader, new object[] { 1 });
			SquadId = (int)getStringMethod.Invoke(reader, new object[] { 2 });
		}

		/// <summary>
		/// Builds the create table query.
		/// </summary>
		/// <returns>The create table query.</returns>
		public string BuildCreateTableQuery()
		{
			return "CREATE TABLE IF NOT EXISTS ApplicationStateStorable (Key ntext PRIMARY KEY, TeamName ntext, FixtureId INTEGER, SquadId INTEGER);";
		}
	}
}