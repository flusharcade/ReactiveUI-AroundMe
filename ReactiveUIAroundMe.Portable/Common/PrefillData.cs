namespace ReactiveUIAroundMe.Portable.Common
{
	using System;
	using ReactiveUIAroundMe.Portable.Models;

	/// <summary>
	/// Prefill data.
	/// </summary>
	public static class PrefillData
	{
		public static string[] FirstNamesData = new string[]
		{
			"Dylan",
			"Marcus",
			"Bernie",
			"Jack",
			"Josh",
			"Jack",
			"Tom",
			"Brent",
			"Nathan",
			"Harry",
			"Gawn",
			"Tyson",
			"Brett",
			"Tom",
			"Taylor",
			"Jake",
			"Andrew",
			"Nic",
			"Shaun",
			"Jeremy",
			"Eddie",
			"Robert",
			"Harry",
			"Gawn",
			"Tyson",
		};

		/// <summary>
		/// The names data.
		/// </summary>
		public static string[] LastNamesData = new string[]
		{
			"Shiel",
			"Bontempelli",
			"Vince",
			"Riewoldt",
			"Gibson",
			"Steven",
			"Hawkins",
			"Harvey",
			"Jones",
			"Brayshaw",
			"Taylor",
			"Tyson",
			"Oliver",
			"Dunn",
			"Stretch",
			"Frost",
			"Pederson",
			"Vince",
			"McDonald",
			"Bugg",
			"Williams",
			"Murphy",
			"Pederson",
			"Vince",
			"McDonald",
		};

		/// <summary>
		/// The status data.
		/// </summary>
		public static string[] StatusData = new string[]
		{
			"Hamstring",
			"Knee",
			"Soreness",
			"Ankle",
			"Concussion",
			"Suspended",
			"Calf",
			"Illness",
			"Shoulder",
			"Foot",
		};

		/// <summary>
		/// The length data.
		/// </summary>
		public static string[] LengthData = new string[]
		{
			"TBC",
			"Test",
			"Indefinite",
			"Season",
			"Weeks",
		};


		/// <summary>
		/// The length data.
		/// </summary>
		public static Match[] Matches = new Match[]
		{
			new Match()
			{
				Quarter = 1,
				Home = "ADELAIDE CROWS",
				HomeImage = "adelaidecrows.png",
				Away = "ESSENDON",
				AwayImage = "essendon.png",
				Date = new DateTime(),
				Venue = "Adelaide"
			},
			new Match()
			{
				Quarter = 4,
				Home = "MELBOURNE",
				HomeImage = "melbourne.png",
				Away = "WEST COAST EAGLES",
				AwayImage = "westcoast.png",
				Date = new DateTime(),
				Venue = "West Coast"
			},
			new Match()
			{
				Quarter = 2,
				Home = "GEELONG",
				HomeImage = "geelong.png",
				Away = "HAWTHORN",
				AwayImage = "hawthorn.png",
				Date = new DateTime(),
				Venue = "Hawthorn"
			},
			new Match()
			{
				Quarter = 1,
				Home = "COLLINGWOOD",
				HomeImage = "collingwood.png",
				Away = "FREEMANTLE",
				AwayImage = "freemantle.png",
				Date = new DateTime(),
				Venue = "Etihad"
			},
			new Match()
			{
				Quarter = 3,
				Home = "ADELAIDE",
				HomeImage = "adelaide.png",
				Away = "BRISBANE",
				AwayImage = "brisbane.png",
				Date = new DateTime(),
				Venue = "Adelaide"
			},
			new Match()
			{
				Quarter = 2,
				Home = "GIANTS",
				HomeImage = "giants.png",
				Away = "RICHMOND",
				AwayImage = "richmond.png",
				Date = new DateTime(),
				Venue = "MCG"
			},
		};
	}
}
