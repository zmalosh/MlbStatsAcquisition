﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class PitchResultTypesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			var url = Feeds.PitchResultTypesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.PitchResultTypesFeed.FromJson(rawJson);

			var dbPitchResultTypes = context.PitchResultTypes.ToDictionary(x => x.Code);
			foreach (var feedPitchResultType in feed)
			{
				if (!dbPitchResultTypes.TryGetValue(feedPitchResultType.Code, out Model.PitchResultType dbPitchResultType))
				{
					dbPitchResultType = new Model.PitchResultType
					{
						Code = feedPitchResultType.Code,
						Description = feedPitchResultType.Description
					};
					dbPitchResultTypes.Add(feedPitchResultType.Code, dbPitchResultType);
					context.PitchResultTypes.Add(dbPitchResultType);
				}
				else
				{
					if (!string.Equals(dbPitchResultType.Description, feedPitchResultType.Description, StringComparison.InvariantCultureIgnoreCase))
					{
						dbPitchResultType.Description = feedPitchResultType.Description;
					}
				}
			}
			context.SaveChanges();
		}
	}
}
