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

				if (feed != null)
				{
					var dbGame = context.Games.SingleOrDefault(x => x.GameID == this.GameId);
					if (dbGame == null)
					{
						throw new NullReferenceException($"GAME NOT FOUND IN DB: {this.GameId}");
					}

					if (feed.Officials != null && feed.Officials.Count > 0)
					{
						this.ProcessUmpires(context, dbGame, feed.Officials);
					}

					var dbPlayerHittingBoxscores = context.PlayerHittingBoxscores.Where(x => x.GameID == this.GameId).ToDictionary(x => x.PlayerID);

					context.SaveChanges();
				}
			}
		}

		private void ProcessUmpires(MlbStatsContext context, Game dbGame, List<Feeds.BoxscoreFeed.OfficialElement> feedUmpires)
		{
			var officialIds = feedUmpires.Select(x => x.Official?.Id).Where(x => x.HasValue).ToList();
			var dbUmpires = context.Umpires.Where(x => officialIds.Contains(x.UmpireID)).ToList();
			var dbUmpireAssignments = context.UmpireAssignments.Where(x => x.GameID == this.GameId).ToList();
			foreach (var feedOfficial in feedUmpires)
			{
				var dbUmpire = dbUmpires.SingleOrDefault(x => x.UmpireID == feedOfficial.Official.Id);
				if (dbUmpire == null)
				{
					dbUmpire = new Umpire
					{
						UmpireID = feedOfficial.Official.Id,
						UmpireName = feedOfficial.Official.FullName,
						UmpireLink = feedOfficial.Official.Link
					};
					context.Umpires.Add(dbUmpire);
					dbUmpires.Add(dbUmpire);
				}

				var dbUmpireAssignment = dbUmpireAssignments.SingleOrDefault(x => x.UmpireID == feedOfficial.Official.Id);
				if (dbUmpireAssignment == null)
				{
					dbUmpireAssignment = new UmpireAssignment
					{
						Umpire = dbUmpire,
						Game = dbGame,
						UmpireType = UmpireType.Unknown
					};
					context.UmpireAssignments.Add(dbUmpireAssignment);
					dbUmpireAssignments.Add(dbUmpireAssignment);
				}

				UmpireType umpireType = GetUmpireType(feedOfficial.OfficialType);
				dbUmpireAssignment.UmpireType = umpireType;
			}
		}

		private static UmpireType GetUmpireType(string umpType)
		{
			string upperUmpType = umpType.ToUpper();
			switch (upperUmpType)
			{
				case "HOME PLATE": return UmpireType.HomePlate;
				case "FIRST BASE": return UmpireType.FirstBase;
				case "SECOND BASE": return UmpireType.SecondBase;
				case "THIRD BASE": return UmpireType.ThirdBase;
				case "RIGHT FIELD": return UmpireType.RightField;
				case "LEFT FIELD": return UmpireType.LeftField;
				default: throw new ArgumentException($"umpType NOT EXPECTED: {umpType}");
			}
		}
	}
}
