using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class HitTrajectoryTypesProcessor
	{
		public void Run()
		{
			List<Feeds.HitTrajectoriesFeed> feed;
			using (var client = new WebClient())
			{
				var url = Feeds.HitTrajectoriesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.HitTrajectoriesFeed.FromJson(rawJson);
			}
			using (var context = new Model.MlbStatsContext())
			{
				var dbHitTrajectoryTypes = context.HitTrajectoryTypes.ToDictionary(x => x.Code);
				foreach (var feedHitTrajectoryType in feed)
				{
					if (!dbHitTrajectoryTypes.TryGetValue(feedHitTrajectoryType.Code, out Model.HitTrajectoryType dbHitTrajectoryType))
					{
						dbHitTrajectoryType = new Model.HitTrajectoryType
						{
							Code = feedHitTrajectoryType.Code,
							Description = feedHitTrajectoryType.Description
						};
						dbHitTrajectoryTypes.Add(feedHitTrajectoryType.Code, dbHitTrajectoryType);
						context.HitTrajectoryTypes.Add(dbHitTrajectoryType);
					}
					else
					{
						if (!string.Equals(dbHitTrajectoryType.Description, feedHitTrajectoryType.Description, StringComparison.InvariantCultureIgnoreCase))
						{
							dbHitTrajectoryType.Description = feedHitTrajectoryType.Description;
						}
					}
				}
				context.SaveChanges();
			}
		}
	}
}
