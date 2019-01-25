using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class GameTypesProcessor : IProcessor
	{
		public void Run()
		{
			List<Feeds.GameTypesFeed> feed;
			using (var client = new WebClient())
			{
				var url = Feeds.GameTypesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.GameTypesFeed.FromJson(rawJson);
			}
			using (var context = new Model.MlbStatsContext())
			{
				var dbGameTypes = context.GameTypes.ToDictionary(x => x.GameTypeID);
				foreach (var feedGameType in feed)
				{
					if (!dbGameTypes.TryGetValue(feedGameType.Id, out Model.GameType dbGameType))
					{
						dbGameType = new Model.GameType
						{
							GameTypeID = feedGameType.Id,
							Description = feedGameType.Description
						};
						dbGameTypes.Add(dbGameType.GameTypeID, dbGameType);
						context.GameTypes.Add(dbGameType);
					}
					else
					{
						if (!string.Equals(dbGameType.Description, feedGameType.Description, StringComparison.InvariantCultureIgnoreCase))
						{
							dbGameType.Description = feedGameType.Description;
						}
					}
				}
				context.SaveChanges();
			}
		}
	}
}
