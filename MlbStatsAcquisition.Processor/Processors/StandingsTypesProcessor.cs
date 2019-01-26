using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class StandingsTypesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			List<Feeds.StandingsTypesFeed> feed;
			using (var client = new WebClient())
			{
				var url = Feeds.StandingsTypesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.StandingsTypesFeed.FromJson(rawJson);
			}

			var dbStandingsTypes = context.StandingsTypes.ToDictionary(x => x.StandingsTypeName);
			foreach (var feedStandingsType in feed)
			{
				if (!dbStandingsTypes.TryGetValue(feedStandingsType.Name, out Model.StandingsType dbStandingsType))
				{
					dbStandingsType = new Model.StandingsType
					{
						StandingsTypeName = feedStandingsType.Name,
						Description = feedStandingsType.Description
					};
					dbStandingsTypes.Add(feedStandingsType.Name, dbStandingsType);
					context.StandingsTypes.Add(dbStandingsType);
				}
				else
				{
					if (!string.Equals(dbStandingsType.Description, feedStandingsType.Description, StringComparison.InvariantCultureIgnoreCase))
					{
						dbStandingsType.Description = feedStandingsType.Description;
					}
				}
			}
			context.SaveChanges();
		}
	}
}
