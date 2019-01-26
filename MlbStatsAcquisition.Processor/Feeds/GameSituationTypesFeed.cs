using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class GameSituationTypesFeed
	{
		public static string GetFeedUrl()
		{
			return "http://statsapi.mlb.com/api/v1/situationCodes";
		}

		public static List<GameSituationTypesFeed> FromJson(string json) => JsonConvert.DeserializeObject<List<GameSituationTypesFeed>>(json, Converter.Settings);

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("sortOrder")]
		public int? SortOrder { get; set; }

		[JsonProperty("navigationMenu")]
		public string NavigationMenu { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("team")]
		public bool Team { get; set; }

		[JsonProperty("batting")]
		public bool Batting { get; set; }

		[JsonProperty("fielding")]
		public bool Fielding { get; set; }

		[JsonProperty("pitching")]
		public bool Pitching { get; set; }
	}

	public static partial class Serialize
	{
		public static string ToJson(this List<GameSituationTypesFeed> self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
