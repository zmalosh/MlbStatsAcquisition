using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class StandingsTypesFeed
	{
		public static string GetFeedUrl()
		{
			return "http://statsapi.mlb.com/api/v1/standingsTypes";
		}

		public static List<StandingsTypesFeed> FromJson(string json) => JsonConvert.DeserializeObject<List<StandingsTypesFeed>>(json, Converter.Settings);

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }
	}

	public static partial class Serialize
	{
		public static string ToJson(this List<StandingsTypesFeed> self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
