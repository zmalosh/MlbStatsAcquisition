using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class PitchResultTypesProcessor : IProcessor
	{
		public void Run()
		{
			List<Feeds.PitchResultTypesFeed> feed;
			using (var client = new WebClient())
			{
				var url = Feeds.PitchResultTypesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.PitchResultTypesFeed.FromJson(rawJson);
			}
			using (var context = new Model.MlbStatsContext())
			{
				var dbPitchResultTypes = context.PitchResultTypes.ToDictionary(x => x.Code);
				foreach (var feedPitchResultType in feed)
				{
					if (!dbPitchResultTypes.TryGetValue(feedPitchResultType.Code, out Model.PitchResultType dbPitchResultType))
					{
						dbPitchResultType = new Model.PitchResultType
						{
							Code = feedPitchResultType.Code,
							Description = feedPitchResultType.Description
						};
						dbPitchResultTypes.Add(feedPitchResultType.Code, dbPitchResultType);
						context.PitchResultTypes.Add(dbPitchResultType);
					}
					else
					{
						if (!string.Equals(dbPitchResultType.Description, feedPitchResultType.Description, StringComparison.InvariantCultureIgnoreCase))
						{
							dbPitchResultType.Description = feedPitchResultType.Description;
						}
					}
				}
				context.SaveChanges();
			}
		}
	}
}
