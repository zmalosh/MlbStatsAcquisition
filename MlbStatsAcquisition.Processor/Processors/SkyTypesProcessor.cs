using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class SkyTypesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			List<Feeds.SkyTypesFeed> feed;
			using (var client = new WebClient())
			{
				var url = Feeds.SkyTypesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.SkyTypesFeed.FromJson(rawJson);
			}

			var dbSkyTypes = context.SkyTypes.ToDictionary(x => x.Code);
			foreach (var feedSkyType in feed)
			{
				if (!dbSkyTypes.TryGetValue(feedSkyType.Code, out Model.SkyType dbSkyType))
				{
					dbSkyType = new Model.SkyType
					{
						Code = feedSkyType.Code,
						Description = feedSkyType.Description
					};
					dbSkyTypes.Add(feedSkyType.Code, dbSkyType);
					context.SkyTypes.Add(dbSkyType);
				}
				else
				{
					if (!string.Equals(dbSkyType.Description, feedSkyType.Description, StringComparison.InvariantCultureIgnoreCase))
					{
						dbSkyType.Description = feedSkyType.Description;
					}
				}
			}
			context.SaveChanges();
		}
	}
}
