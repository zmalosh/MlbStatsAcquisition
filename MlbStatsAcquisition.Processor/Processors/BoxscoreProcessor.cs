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

					if (feed.Teams.Away?.Players != null && feed.Teams.Home?.Players != null)
					{
						bool isAwayPlayerUpdate = ProcessPlayerTeamSeasons(context, dbGame.AwayTeamID.Value, dbGame.Season, feed.Teams.Away.Players.Values);
						bool isHomePlayerUpdate = ProcessPlayerTeamSeasons(context, dbGame.HomeTeamID.Value, dbGame.Season, feed.Teams.Home.Players.Values);
						bool isPlayerUpdate = isAwayPlayerUpdate || isHomePlayerUpdate;
						if (isPlayerUpdate)
						{
							context.SaveChanges();
						}

						bool gameHasHittingBoxscores = feed.Teams.Away.Players.Any(x => x.Value.Stats.Batting != null && !x.Value.Stats.Batting.IsDefault())
													&& feed.Teams.Home.Players.Any(x => x.Value.Stats.Batting != null && !x.Value.Stats.Batting.IsDefault());

						if (gameHasHittingBoxscores)
						{
							var dbPlayerHittingBoxscores = context.PlayerHittingBoxscores.Where(x => x.GameID == this.GameId).ToDictionary(x => x.PlayerID);
							var awayHitters = feed.Teams?.Away?.Players.Where(x => x.Value.Stats?.Batting != null && !x.Value.Stats.Batting.IsDefault()).Select(y => y.Value).ToList();
							ProcessHitterBoxscores(context, dbGame.AwayTeamID.Value, dbGame.Season, dbPlayerHittingBoxscores, awayHitters);
							var homeHitters = feed.Teams?.Home?.Players.Where(x => x.Value.Stats?.Batting != null && !x.Value.Stats.Batting.IsDefault()).Select(y => y.Value).ToList();
							ProcessHitterBoxscores(context, dbGame.HomeTeamID.Value, dbGame.Season, dbPlayerHittingBoxscores, homeHitters);
						}
					}

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

		private bool ProcessPlayerTeamSeasons(MlbStatsContext context, int teamId, int season, IEnumerable<Feeds.BoxscoreFeed.GamePlayer> feedPlayers)
		{
			var playerIds = feedPlayers.Select(x => x.Person.Id).ToList();
			var dbPlayers = context.Players.Where(x => playerIds.Contains(x.PlayerID)).ToDictionary(x => x.PlayerID);
			var dbPlayerTeamSeasons = context.PlayerTeamSeasons.Where(x => playerIds.Contains(x.PlayerID) && x.TeamID == teamId && x.Season == season).ToDictionary(x => x.PlayerID);
			if (dbPlayers.Count == playerIds.Count && dbPlayerTeamSeasons.Count == playerIds.Count)
			{
				// NO DB CHANGES NEEDED
				return false;
			}
			if (dbPlayers.Count != playerIds.Count || dbPlayerTeamSeasons.Count != playerIds.Count)
			{
				// DB CHANGES NEEDED
				foreach (var feedPlayer in feedPlayers)
				{
					if (!dbPlayers.TryGetValue(feedPlayer.Person.Id, out Player dbPlayer))
					{
						dbPlayer = new Player
						{
							PlayerID = feedPlayer.Person.Id,
							FullName = feedPlayer.Person.FullName,
							PlayerLink = feedPlayer.Person.Link,
						};
						context.Players.Add(dbPlayer);
						dbPlayers.Add(dbPlayer.PlayerID, dbPlayer);
					}

					if (!dbPlayerTeamSeasons.TryGetValue(dbPlayer.PlayerID, out PlayerTeamSeason dbPlayerTeamSeason))
					{
						// TODO: PROCESS PLAYER DATA FROM PLAYER API CALL
						dbPlayerTeamSeason = new PlayerTeamSeason
						{
							Player = dbPlayer,
							TeamID = teamId,
							Season = season
						};
						context.PlayerTeamSeasons.Add(dbPlayerTeamSeason);
						dbPlayerTeamSeasons.Add(dbPlayer.PlayerID, dbPlayerTeamSeason);
					}
				}
			}
			return true;
		}

		private void ProcessHitterBoxscores(MlbStatsContext context, int teamId, int season,
			Dictionary<int, PlayerHittingBoxscore> dbGameBoxscores,
			List<Feeds.BoxscoreFeed.GamePlayer> feedPlayers)
		{
			foreach (var feedPlayer in feedPlayers)
			{
				if (feedPlayer?.Stats?.Batting != null)
				{
					bool updateStats = false;
					if (!dbGameBoxscores.TryGetValue(feedPlayer.Person.Id, out PlayerHittingBoxscore dbBoxscore))
					{
						updateStats = true;
						dbBoxscore = new PlayerHittingBoxscore
						{
							GameID = this.GameId,
							TeamID = teamId,
							PlayerID = feedPlayer.Person.Id,
							Season = season
						};
						context.PlayerHittingBoxscores.Add(dbBoxscore);
						dbGameBoxscores.Add(feedPlayer.Person.Id, dbBoxscore);
					}

					var feedBox = feedPlayer.Stats.Batting;

					// NOT NEW - STATS MUST BE DIFFERENT TO UPDATE
					// MAKE SURE NO UPDATES HAVE BEEN MADE TO STATS
					if (!updateStats)
					{
						updateStats = feedBox.AtBats != dbBoxscore.AtBats
										|| feedBox.TotalBases != dbBoxscore.TotalBases
										|| feedBox.Hits != dbBoxscore.Hits
										|| feedBox.Rbi != dbBoxscore.RunsBattedIn
										|| feedBox.Runs != dbBoxscore.Runs
										|| feedBox.Doubles != dbBoxscore.Doubles
										|| feedBox.Triples != dbBoxscore.Triples
										|| feedBox.HomeRuns != dbBoxscore.HomeRuns
										|| feedBox.BaseOnBalls != dbBoxscore.Walks
										|| feedBox.StrikeOuts != dbBoxscore.StrikeOuts
										|| feedBox.GroundOuts != dbBoxscore.GroundOuts
										|| feedBox.FlyOuts != dbBoxscore.FlyOuts
										|| feedBox.LeftOnBase != dbBoxscore.RunnersLeftOnBase
										|| feedBox.HitByPitch != dbBoxscore.HitByPitches
										|| (feedBox.GamesPlayed == 1) != dbBoxscore.GamePlayed
										|| feedBox.CaughtStealing != dbBoxscore.CaughtStealing
										|| feedBox.Pickoffs != dbBoxscore.Pickoffs
										|| feedBox.StolenBases != dbBoxscore.StolenBases
										|| feedBox.SacBunts != dbBoxscore.SacBunts
										|| feedBox.SacFlies != dbBoxscore.SacFlies
										|| feedBox.IntentionalWalks != dbBoxscore.IntentionalWalks
										|| feedBox.CatchersInterference != dbBoxscore.CatcherInterferences
										|| feedBox.GroundIntoDoublePlay != dbBoxscore.GroundIntoDoublePlay
										|| feedBox.GroundIntoTriplePlay != dbBoxscore.GroundIntoTriplePlay;
					}

					if (updateStats)
					{
						dbBoxscore.AtBats = (byte?)feedBox.AtBats;
						dbBoxscore.TotalBases = (byte?)feedBox.TotalBases;
						dbBoxscore.Hits = (byte?)feedBox.Hits;
						dbBoxscore.RunsBattedIn = (byte?)feedBox.Rbi;
						dbBoxscore.Runs = (byte?)feedBox.Runs;
						dbBoxscore.Doubles = (byte?)feedBox.Doubles;
						dbBoxscore.Triples = (byte?)feedBox.Triples;
						dbBoxscore.HomeRuns = (byte?)feedBox.HomeRuns;
						dbBoxscore.Walks = (byte?)feedBox.BaseOnBalls;
						dbBoxscore.GroundOuts = (byte?)feedBox.GroundOuts;
						dbBoxscore.StrikeOuts = (byte?)feedBox.StrikeOuts;
						dbBoxscore.FlyOuts = (byte?)feedBox.FlyOuts;
						dbBoxscore.RunnersLeftOnBase = (byte?)feedBox.LeftOnBase;
						dbBoxscore.HitByPitches = (byte?)feedBox.HitByPitch;
						dbBoxscore.GamePlayed = ((feedBox.GamesPlayed ?? 0) == 1);
						dbBoxscore.CaughtStealing = (byte?)feedBox.CaughtStealing;
						dbBoxscore.Pickoffs = (byte?)feedBox.Pickoffs;
						dbBoxscore.StolenBases = (byte?)feedBox.StolenBases;
						dbBoxscore.SacBunts = (byte?)feedBox.SacBunts;
						dbBoxscore.SacFlies = (byte?)feedBox.SacFlies;
						dbBoxscore.IntentionalWalks = (byte?)feedBox.IntentionalWalks;
						dbBoxscore.CatcherInterferences = (byte?)feedBox.CatchersInterference;
						dbBoxscore.GroundIntoDoublePlay = (byte?)feedBox.GroundIntoDoublePlay;
						dbBoxscore.GroundIntoTriplePlay = (byte?)feedBox.GroundIntoTriplePlay;
					}
				}
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
