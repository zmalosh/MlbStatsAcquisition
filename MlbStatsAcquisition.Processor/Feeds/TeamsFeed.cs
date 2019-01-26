using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class TeamsFeed
	{
		public static string GetFeedUrl(int? year = null)
		{
			if (year.HasValue)
			{
				return string.Format($"http://statsapi.mlb.com/api/v1/teams?season={year}");
			}
			return "http://statsapi.mlb.com/api/v1/teams";
		}

		public static TeamsFeed FromJson(string json) => JsonConvert.DeserializeObject<TeamsFeed>(json, Converter.Settings);

		[JsonProperty("teams")]
		public List<FeedTeam> Teams { get; set; }
		public partial class FeedTeam
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }

			[JsonProperty("venue")]
			public FeedGroup Venue { get; set; }

			[JsonProperty("teamCode")]
			public string TeamCode { get; set; }

			[JsonProperty("fileCode")]
			public string FileCode { get; set; }

			[JsonProperty("abbreviation")]
			public string Abbreviation { get; set; }

			[JsonProperty("teamName")]
			public string TeamName { get; set; }

			[JsonProperty("locationName", NullValueHandling = NullValueHandling.Ignore)]
			public string LocationName { get; set; }

			[JsonProperty("firstYearOfPlay", NullValueHandling = NullValueHandling.Ignore)]
			public int? FirstYearOfPlay { get; set; }

			[JsonProperty("league")]
			public FeedGroup League { get; set; }

			[JsonProperty("sport")]
			public FeedGroup Sport { get; set; }

			[JsonProperty("shortName")]
			public string ShortName { get; set; }

			[JsonIgnore]
			public bool IsAllStarTeam { get { return this._allStarStatus == "Y"; } }

			[JsonProperty("allStarStatus")]
			private string _allStarStatus { get; set; }

			[JsonProperty("active")]
			public bool Active { get; set; }

			[JsonProperty("division", NullValueHandling = NullValueHandling.Ignore)]
			public FeedGroup Division { get; set; }

			[JsonProperty("parentOrgName", NullValueHandling = NullValueHandling.Ignore)]
			public string ParentOrgName { get; set; }

			[JsonProperty("parentOrgId", NullValueHandling = NullValueHandling.Ignore)]
			public int? ParentOrgId { get; set; }

			[JsonProperty("springLeague", NullValueHandling = NullValueHandling.Ignore)]
			public FeedGroup SpringLeague { get; set; }
		}

		public class FeedGroup
		{
			[JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
			public int? Id { get; set; }

			[JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
			public string Name { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }

			[JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
			public string Abbreviation { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this TeamsFeed self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
