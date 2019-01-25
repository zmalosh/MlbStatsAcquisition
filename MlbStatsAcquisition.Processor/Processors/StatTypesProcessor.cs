using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class StatTypesProcessor : IProcessor
	{
		public void Run()
		{
			List<Feeds.StatTypesFeed> feed;
			using (var client = new WebClient())
			{
				var url = Feeds.StatTypesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.StatTypesFeed.FromJson(rawJson);
			}
			using (var context = new Model.MlbStatsContext())
			{
				var dbStatTypes = context.StatTypes.ToDictionary(x => x.Lookup);
				foreach (var feedStatType in feed)
				{
					if (!dbStatTypes.TryGetValue(feedStatType.LookupParam, out Model.StatType dbStatType))
					{
						dbStatType = new Model.StatType
						{
							Lookup = feedStatType.LookupParam,
							Name = feedStatType.Name,
							Label = feedStatType.Label,
							IsCounting = feedStatType.IsCounting,
							IsCatching = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "Catching", StringComparison.InvariantCultureIgnoreCase)),
							IsFielding = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "Fielding", StringComparison.InvariantCultureIgnoreCase)),
							IsHitting = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "Hitting", StringComparison.InvariantCultureIgnoreCase)),
							IsPitching = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "Pitching", StringComparison.InvariantCultureIgnoreCase)),
						};
						dbStatTypes.Add(dbStatType.Lookup, dbStatType);
						context.StatTypes.Add(dbStatType);
					}
					else
					{
						; // TODO: PUT ADJUSTMENT CODE HERE TO VERIFY NO CHANGES HAVE HAPPENED
					}
				}
				context.SaveChanges();
			}
		}
	}
}
