using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class PitchTypesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			var url = Feeds.PitchTypesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.PitchTypesFeed.FromJson(rawJson);

			var dbPitchTypes = context.PitchTypes.ToDictionary(x => x.Code);
			foreach (var feedPitchType in feed)
			{
				if (!dbPitchTypes.TryGetValue(feedPitchType.Code, out Model.PitchType dbPitchType))
				{
					dbPitchType = new Model.PitchType
					{
						Code = feedPitchType.Code,
						Description = feedPitchType.Description
					};
					dbPitchTypes.Add(feedPitchType.Code, dbPitchType);
					context.PitchTypes.Add(dbPitchType);
				}
				else
				{
					if (!string.Equals(dbPitchType.Description, feedPitchType.Description, StringComparison.InvariantCultureIgnoreCase))
					{
						dbPitchType.Description = feedPitchType.Description;
					}
				}
			}
			context.SaveChanges();
		}
	}
}
