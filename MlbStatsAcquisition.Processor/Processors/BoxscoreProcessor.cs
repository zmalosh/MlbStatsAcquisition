using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MlbStatsAcquisition.Model;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class BoxscoreProcessor : IProcessor
	{
		private int GameId { get; set; }

		public BoxscoreProcessor(int gameId)
		{
			this.GameId = gameId;
		}

		public void Run(MlbStatsContext context)
		{
			Feeds.BoxscoreFeed feed;
			using (var client = new WebClient())
			{
				var url = Feeds.BoxscoreFeed.GetFeedUrl(this.GameId);
				var rawJson = client.DownloadString(url);
				feed = Feeds.BoxscoreFeed.FromJson(rawJson);
				Console.WriteLine($"{this.GameId} - {feed.Teams.Away.Team.Abbreviation.PadRight(3, ' ')} ({feed.Teams.Away.Team.Record?.GamesPlayed}) @{feed.Teams.Home.Team.Abbreviation.PadRight(3, ' ')} ({feed.Teams.Home.Team.Record?.GamesPlayed})");
			}
		}
	}
}
