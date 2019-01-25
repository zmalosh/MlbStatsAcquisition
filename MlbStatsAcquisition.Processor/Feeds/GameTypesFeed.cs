using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class GameTypesFeed
	{
		public static string GetFeedUrl()
		{
			return "http://statsapi.mlb.com/api/v1/gameTypes";
		}
		public static List<GameTypesFeed> FromJson(string json) => JsonConvert.DeserializeObject<List<GameTypesFeed>>(json, Converter.Settings);
		
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }
	}

	public static partial class Serialize
	{
		public static string ToJson(this List<GameTypesFeed> self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
