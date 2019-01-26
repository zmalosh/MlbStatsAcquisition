using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class WindTypesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			List<Feeds.WindTypesFeed> feed;
			using (var client = new WebClient())
			{
				var url = Feeds.WindTypesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.WindTypesFeed.FromJson(rawJson);
			}

			var dbWindTypes = context.WindTypes.ToDictionary(x => x.Code);
			foreach (var feedWindType in feed)
			{
				if (!dbWindTypes.TryGetValue(feedWindType.Code, out Model.WindType dbWindType))
				{
					dbWindType = new Model.WindType
					{
						Code = feedWindType.Code,
						Description = feedWindType.Description
					};
					dbWindTypes.Add(feedWindType.Code, dbWindType);
					context.WindTypes.Add(dbWindType);
				}
				else
				{
					if (!string.Equals(dbWindType.Description, feedWindType.Description, StringComparison.InvariantCultureIgnoreCase))
					{
						dbWindType.Description = feedWindType.Description;
					}
				}
			}
		}
	}
}
