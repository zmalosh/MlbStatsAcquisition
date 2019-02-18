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
			var url = Feeds.StandingsTypesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.StandingsTypesFeed.FromJson(rawJson);

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
