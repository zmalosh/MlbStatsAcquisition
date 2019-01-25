using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class VenuesFeed
	{
		public static string GetFeedUrl()
		{
			return "http://statsapi.mlb.com/api/v1/venues";
		}

		public static VenuesFeed FromJson(string json) => JsonConvert.DeserializeObject<VenuesFeed>(json, Converter.Settings);

		[JsonProperty("venues")]
		public List<Venue> Venues { get; set; }

		public class Venue
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this VenuesFeed self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
