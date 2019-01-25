using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class GameStatusTypesFeed
	{
		public static string GetFeedUrl()
		{
			return "http://statsapi.mlb.com/api/v1/gameStatus";
		}

		public static List<GameStatusTypesFeed> FromJson(string json) => JsonConvert.DeserializeObject<List<GameStatusTypesFeed>>(json, Converter.Settings);


		[JsonProperty("abstractGameState")]
		public string AbstractGameState { get; set; }

		[JsonProperty("codedGameState")]
		public string CodedGameState { get; set; }

		[JsonProperty("detailedState")]
		public string DetailedState { get; set; }

		[JsonProperty("statusCode")]
		public string StatusCode { get; set; }

		[JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]
		public string Reason { get; set; }

		[JsonProperty("abstractGameCode")]
		public string AbstractGameCode { get; set; }
	}

	public static partial class Serialize
	{
		public static string ToJson(this List<GameStatusTypesFeed> self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}

