using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MlbStatsAcquisition.Processor.Feeds
{
	// CALLED Sports BY MLB
	public class AssociationsFeed
	{
		public static string GetFeedUrl()
		{
			return "http://statsapi.mlb.com/api/v1/sports";
		}

		public static AssociationsFeed FromJson(string json) => JsonConvert.DeserializeObject<AssociationsFeed>(json, Converter.Settings);

		[JsonProperty("sports")]
		public List<Association> Associations { get; set; }

		public class Association
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("code")]
			public string Code { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("abbreviation")]
			public string Abbreviation { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this AssociationsFeed self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
