using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class PlayByPlayProcessor : IProcessor
	{
		private int GameId { get; set; }

		public PlayByPlayProcessor(int gid)
		{
			this.GameId = gid;
		}

		public void Run(Model.MlbStatsContext context)
		{
			Feeds.PlayByPlayFeed feed;
			using (var client = new WebClient())
			{
				var url = Feeds.PlayByPlayFeed.GetFeedUrl(this.GameId);
				string rawJson = JsonUtility.GetRawJsonFromUrl(url); ;
				if (rawJson == null) { return; }
				feed = Feeds.PlayByPlayFeed.FromJson(rawJson);

				if (feed != null)
				{

				}
			}
		}
	}
}
