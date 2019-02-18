using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class ReviewReasonTypesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			var url = Feeds.ReviewReasonTypesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.ReviewReasonTypesFeed.FromJson(rawJson);

			var dbReviewReasonTypes = context.ReviewReasonTypes.ToDictionary(x => x.Code);
			foreach (var feedReviewReasonType in feed)
			{
				if (!dbReviewReasonTypes.TryGetValue(feedReviewReasonType.Code, out Model.ReviewReasonType dbReviewReasonType))
				{
					dbReviewReasonType = new Model.ReviewReasonType
					{
						Code = feedReviewReasonType.Code,
						Description = feedReviewReasonType.Description
					};
					dbReviewReasonTypes.Add(feedReviewReasonType.Code, dbReviewReasonType);
					context.ReviewReasonTypes.Add(dbReviewReasonType);
				}
				else
				{
					if (!string.Equals(dbReviewReasonType.Description, feedReviewReasonType.Description, StringComparison.InvariantCultureIgnoreCase))
					{
						dbReviewReasonType.Description = feedReviewReasonType.Description;
					}
				}
			}
			context.SaveChanges();
		}
	}
}
