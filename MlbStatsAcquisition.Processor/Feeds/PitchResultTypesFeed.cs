﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class PitchResultTypesFeed
	{
		public static string GetFeedUrl()
		{
			return "http://statsapi.mlb.com/api/v1/pitchCodes";
		}

		public static List<PitchResultTypesFeed> FromJson(string json) => JsonConvert.DeserializeObject<List<PitchResultTypesFeed>>(json, Converter.Settings);

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }
	}

	public static partial class Serialize
	{
		public static string ToJson(this List<PitchResultTypesFeed> self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
