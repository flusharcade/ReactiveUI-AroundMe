
namespace ReactiveUIAroundMe.Portable.Common
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// Club logos.
	/// </summary>
	public static class ImagePathsAndColors
	{
		/// <summary>
		/// The image paths.
		/// </summary>
		public static IDictionary<string, string> ImagePaths = new Dictionary<string, string>()
		{
			{"Melbourne", "melbourne"},
			{"Hawthorn", "hawthorn"},
			{"Collingwood", "collingwood"},
			{"Western Bulldogs", "western_bulldogs"},
			{"Richmond", "richmond"},
			{"Adelaide Crows", "adelaide_crows"},
			{"Brisbane Lions", "brisbane_lions"},
			{"Carlton", "carlton"},
			{"Gold Coast Suns", "gold_coast_suns"},
			{"Geelong Cats", "geelong_cats"},
			{"Sydney Swans", "sydney_swans"},
			{"St Kilda", "st_kilda"},
			{"GWS Giants", "gws_giants"},
			{"West Coast Eagles", "west_coast_eagles"},
			{"Fremantle", "fremantle_dockers"},
			{"Port Adelaide", "port_adelaide"},
			{"Essendon", "essendon"},
			{"North Melbourne", "north_melbourne"},
		};

		/// <summary>
		/// The image paths.
		/// </summary>
		public static IDictionary<int, string> StateImagePaths = new Dictionary<int, string>()
		{
			{1, "new_south_wales"},
			{2, "queensland"},
			{3, "victoria"},
			{4, "south_australia"},
			{5, "western_australia"},
			{6, "tasmania"},
			{7, "act"},
			{8, "northern_territory"},
		};

		public static IDictionary<int, string> StateNames = new Dictionary<int, string>()
		{
			{1, "New South Wales"},
			{2, "Queensland"},
			{3, "Victoria"},
			{4, "South Australia"},
			{5, "Western Australia"},
			{6, "Tasmania"},
			{7, "ACT"},
			{8, "Northern Territory"},
		};

		/// <summary>
		/// The colors.
		/// </summary>
		public static IDictionary<string, string> Colors = new Dictionary<string, string>()
		{
			{"Melbourne", "#ce2031"},
			{"Hawthorn", "#572600"},
			{"Collingwood", "#000000"},
			{"Western Bulldogs", "#005dab"},
			{"Richmond", "#ffd204"},
			{"Adelaide Crows", "#004b8d"},
			{"Brisbane Lions", "#a30046"},
			{"Carlton", "#011f36"},
			{"Gold Coast Suns", "#cc2127"},
			{"Geelong Cats", "#002a5c"},
			{"Sydney Swans", "#e41e31"},
			{"St Kilda", "#e41e31"},
			{"GWS Giants", "#f89828"},
			{"West Coast Eagles", "#002a5c"},
			{"Fremantle", "#290c54"},
			{"Port Adelaide", "#00a1b1"},
			{"Essendon", "#e5193a"},
			{"North Melbourne", "#023ca0"},
		};
	}
}
