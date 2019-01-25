using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class PositionsFeed
	{
		public static string GetFeedUrl()
		{
			return "http://statsapi.mlb.com/api/v1/positions";
		}

		public static List<PositionsFeed> FromJson(string json) => JsonConvert.DeserializeObject<List<PositionsFeed>>(json, Converter.Settings);

		[JsonProperty("shortName")]
		public string ShortName { get; set; }

		[JsonProperty("fullName")]
		public string FullName { get; set; }

		[JsonProperty("abbrev")]
		public string Abbrev { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("formalName")]
		public string FormalName { get; set; }

		[JsonProperty("pitcher")]
		public bool Pitcher { get; set; }

		[JsonProperty("fielder")]
		public bool Fielder { get; set; }

		[JsonProperty("outfield")]
		public bool Outfield { get; set; }

		[JsonProperty("displayName")]
		public string DisplayName { get; set; }

		internal static class Converter
		{
			public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
			{
				MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
				DateParseHandling = DateParseHandling.None,
				Converters = {
					new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
				},
			};
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this List<PositionsFeed> self) => JsonConvert.SerializeObject(self, PositionsFeed.Converter.Settings);
	}
}
