using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class PositionsProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			var url = Feeds.PositionsFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.PositionsFeed.FromJson(rawJson);

			var dbPositions = context.Positions.ToDictionary(x => x.PositionAbbr);
			foreach (var feedPosition in feed)
			{
				if (!dbPositions.TryGetValue(feedPosition.Abbrev, out Model.Position dbPosition))
				{
					dbPosition = new Model.Position
					{
						PositionAbbr = feedPosition.Abbrev,
						ShortName = feedPosition.ShortName,
						FullName = feedPosition.FullName,
						FormalName = feedPosition.FormalName,
						DisplayName = feedPosition.DisplayName,
						PositionCode = feedPosition.Code,
						IsFielder = feedPosition.Fielder,
						IsPitcher = feedPosition.Pitcher,
						IsOutfield = feedPosition.Outfield,
						PositionType = feedPosition.Type
					};
					dbPositions.Add(dbPosition.PositionAbbr, dbPosition);
					context.Positions.Add(dbPosition);
				}
				else
				{
					; // TODO: ADD NO-CHANGE VALIDATION LOGIC
				}
			}
			context.SaveChanges();
		}
	}
}
