using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Feeds
{
	// CALLED baseballStats BY MLB... statTypes TO BE CALLED StatClassifications IF INCLUDED.
	public class StatTypesFeed
	{
		public static string GetFeedUrl()
		{
			return "http://statsapi.mlb.com/api/v1/baseballStats";
		}

		public static List<StatTypesFeed> FromJson(string json) => JsonConvert.DeserializeObject<List<StatTypesFeed>>(json, Converter.Settings);

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("lookupParam")]
		public string LookupParam { get; set; }

		[JsonProperty("isCounting")]
		public bool IsCounting { get; set; }

		[JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
		public string Label { get; set; }

		[JsonProperty("statGroups")]
		public List<StatGroup> StatGroups { get; set; }

		[JsonProperty("orgTypes")]
		public List<object> OrgTypes { get; set; }

		public class StatGroup
		{
			[JsonProperty("displayName")]
			public string Name { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this List<StatTypesFeed> self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
