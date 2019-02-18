using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class GameEventTypesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			var url = Feeds.GameEventTypesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.GameEventTypesFeed.FromJson(rawJson);


			var dbGameEventTypes = context.GameEventTypes.ToDictionary(x => x.Code);
			foreach (var feedGameEventType in feed)
			{
				if (!dbGameEventTypes.TryGetValue(feedGameEventType.Code, out Model.GameEventType dbGameEventType))
				{
					dbGameEventType = new Model.GameEventType
					{
						Code = feedGameEventType.Code,
						Description = feedGameEventType.Description
					};
					dbGameEventTypes.Add(feedGameEventType.Code, dbGameEventType);
					context.GameEventTypes.Add(dbGameEventType);
				}
				else
				{
					if (!string.Equals(dbGameEventType.Description, feedGameEventType.Description, StringComparison.InvariantCultureIgnoreCase))
					{
						dbGameEventType.Description = feedGameEventType.Description;
					}
				}
			}
			context.SaveChanges();
		}
	}
}
