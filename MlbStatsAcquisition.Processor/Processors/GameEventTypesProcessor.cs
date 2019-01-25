using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class GameEventTypesProcessor
	{
		public void Run()
		{
			List<Feeds.GameEventTypesFeed> feed;
			using (var client = new WebClient())
			{
				var url = Feeds.GameEventTypesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.GameEventTypesFeed.FromJson(rawJson);
			}
			using (var context = new Model.MlbStatsContext())
			{
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
}
