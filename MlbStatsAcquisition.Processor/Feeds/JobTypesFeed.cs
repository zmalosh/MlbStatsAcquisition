using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class JobTypesFeed
	{
		public static string GetFeedUrl()
		{
			return "http://statsapi.mlb.com/api/v1/jobTypes";
		}
		public static List<JobTypesFeed> FromJson(string json) => JsonConvert.DeserializeObject<List<JobTypesFeed>>(json, Converter.Settings);

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("job")]
		public string Job { get; set; }

		[JsonProperty("sortOrder", NullValueHandling = NullValueHandling.Ignore)]
		public int? SortOrder { get; set; }
	}

	public static partial class Serialize
	{
		public static string ToJson(this List<JobTypesFeed> self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
