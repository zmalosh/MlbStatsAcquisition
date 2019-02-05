using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class RosterFeed
	{
		public string GetFeedUrl(int teamId, int? season = null)
		{
			// NULL GIVES ALL TIME ROSTER
			if (season.HasValue)
			{
				return string.Format($"http://statsapi.mlb.com/api/v1/teams/{teamId}/roster/fullSeason?season={season}");
			}
			return string.Format($"http://statsapi.mlb.com/api/v1/teams/{teamId}/roster/allTime");
		}

		public static RosterFeed FromJson(string json) => JsonConvert.DeserializeObject<RosterFeed>(json, MlbStatsAcquisition.Processor.Feeds.Converter.Settings);

		[JsonProperty("copyright")]
		public string Copyright { get; set; }

		[JsonProperty("roster")]
		public List<RosterNode> Roster { get; set; }

		[JsonProperty("link")]
		public string Link { get; set; }

		[JsonProperty("teamId")]
		public long TeamId { get; set; }

		[JsonProperty("rosterType")]
		public string RosterType { get; set; }

		public class RosterNode
		{
			[JsonProperty("person")]
			public Person Person { get; set; }

			[JsonProperty("jerseyNumber")]
			public string JerseyNumber { get; set; }

			[JsonProperty("position")]
			public Position Position { get; set; }
		}

		public class Person
		{
			[JsonProperty("id")]
			public long Id { get; set; }

			[JsonProperty("fullName")]
			public string FullName { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }
		}

		public class Position
		{
			[JsonProperty("code")]
			public string Code { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("type")]
			public string Type { get; set; }

			[JsonProperty("abbreviation")]
			public string Abbreviation { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this RosterFeed self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
