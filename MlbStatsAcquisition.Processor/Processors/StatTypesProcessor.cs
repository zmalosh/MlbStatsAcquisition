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
		public void Run(Model.MlbStatsContext context)
		{
			var url = Feeds.StatTypesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.StatTypesFeed.FromJson(rawJson);

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
						IsCatching = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "catching", StringComparison.InvariantCultureIgnoreCase)),
						IsFielding = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "fielding", StringComparison.InvariantCultureIgnoreCase)),
						IsHitting = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "hitting", StringComparison.InvariantCultureIgnoreCase)),
						IsPitching = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "pitching", StringComparison.InvariantCultureIgnoreCase)),
						IsRunning = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "running", StringComparison.InvariantCultureIgnoreCase)),
						IsTeam = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "team", StringComparison.InvariantCultureIgnoreCase)),
						IsGame = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "game", StringComparison.InvariantCultureIgnoreCase)),
						IsStreak = feedStatType.StatGroups.Any(x => string.Equals(x.Name, "streak", StringComparison.InvariantCultureIgnoreCase)),
					};

					if (feedStatType.StatGroups.Any(x => !dbStatType.IsCatching && !dbStatType.IsFielding && !dbStatType.IsHitting && !dbStatType.IsPitching
															&& !dbStatType.IsRunning && !dbStatType.IsTeam && !dbStatType.IsGame && !dbStatType.IsStreak))
					{
						throw new ArgumentException("STATIS A STAT FOR NOTHINGNESS?!");
					}
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
