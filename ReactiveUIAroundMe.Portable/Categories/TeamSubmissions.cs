
namespace ReactiveUIAroundMe.Portable.Enums
{
	using System.Collections.Generic;

    /// <summary>
    /// The page names.
    /// </summary>
    public static class TeamSubmissions
    {
		/// <summary>
		/// The codes.
		/// </summary>
		public readonly static Dictionary<ListSetStatus, string> Titles = new Dictionary<ListSetStatus, string>()
		{
			{ListSetStatus.Pending, "Pending"},
			{ListSetStatus.Submitted25, "Submitted 25"},
			{ListSetStatus.Pending, "Approved 25"},
			{ListSetStatus.Submitted22, "Submitted 22"},
			{ListSetStatus.Final22, "Final 22"},
			{ListSetStatus.Pending, "Pending"},
			{ListSetStatus.TuesdayPending, "Tuesday Pending"},
			{ListSetStatus.TuesdayConfirmed, "Tuesday Confirmed"},
		};
	}
}