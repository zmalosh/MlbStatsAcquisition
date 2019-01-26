using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class JobTypesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			List<Feeds.JobTypesFeed> feed;
			using (var client = new WebClient())
			{
				var url = Feeds.JobTypesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.JobTypesFeed.FromJson(rawJson);
				feed = feed.OrderBy(x => x.SortOrder.HasValue ? 1 : 2).ThenBy(y => y.SortOrder).ThenBy(z => z.Code).ToList();
			}

			var dbJobTypes = context.JobTypes.ToDictionary(x => x.Code);
			foreach (var feedJobType in feed)
			{
				if (!dbJobTypes.TryGetValue(feedJobType.Code, out Model.JobType dbJobType))
				{
					dbJobType = new Model.JobType
					{
						Code = feedJobType.Code,
						Description = feedJobType.Job,
						SortOrder = feedJobType.SortOrder
					};
					dbJobTypes.Add(feedJobType.Code, dbJobType);
					context.JobTypes.Add(dbJobType);
				}
				else
				{
					if (!string.Equals(dbJobType.Description, feedJobType.Job, StringComparison.InvariantCultureIgnoreCase))
					{
						dbJobType.Description = feedJobType.Job;
					}
					if (dbJobType.SortOrder != feedJobType.SortOrder)
					{
						dbJobType.SortOrder = feedJobType.SortOrder;
					}
				}
			}
			context.SaveChanges();
		}
	}
}
