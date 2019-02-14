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
				string rawJson = null;
				try
				{
					rawJson = client.DownloadString(url);
				}
				catch (WebException ex)
				{
					try
					{
						rawJson = client.DownloadString(url);
					}
					catch (Exception ex2)
					{
						string dir = "FailedGameIDs";
						if (!System.IO.Directory.Exists(dir))
						{
							System.IO.Directory.CreateDirectory(dir);
						}
						var filePath = string.Format($"{dir}\\{this.GameId}.nobueno");
						System.IO.File.Create(filePath);
						return;
					}
				}
				feed = Feeds.BoxscoreFeed.FromJson(rawJson);

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
						ProcessPlayerTeamSeasons(context, dbGame.AwayTeamID.Value, dbGame.Season, feed.Teams.Away.Players.Values);
						ProcessPlayerTeamSeasons(context, dbGame.HomeTeamID.Value, dbGame.Season, feed.Teams.Home.Players.Values);


						bool gameHasHittingBoxscores = feed.Teams.Away.Players.Any(x => x.Value.Stats?.Batting != null && !x.Value.Stats.Batting.IsDefault())
													&& feed.Teams.Home.Players.Any(x => x.Value.Stats?.Batting != null && !x.Value.Stats.Batting.IsDefault());

						bool gameHasPitchingBoxscores = feed.Teams.Away.Players.Any(x => x.Value.Stats?.Pitching != null && !x.Value.Stats.Pitching.IsDefault())
													&& feed.Teams.Home.Players.Any(x => x.Value.Stats?.Pitching != null && !x.Value.Stats.Pitching.IsDefault());

						bool gameHasFieldingBoxscores = feed.Teams.Away.Players.Any(x => x.Value.Stats?.Fielding != null && !x.Value.Stats.Fielding.IsDefault())
													&& feed.Teams.Home.Players.Any(x => x.Value.Stats?.Fielding != null && !x.Value.Stats.Fielding.IsDefault());

						// ASSUME FINISHED GAME WILL HAVE HITTING AND PITCHING DATA... FIELDING NOT SO MUCH
						if (gameHasHittingBoxscores && gameHasPitchingBoxscores)
						{
							var dbPlayerBoxscores = context.PlayerGameBoxscores.Where(x => x.GameID == this.GameId).ToDictionary(x => x.PlayerID);
							var awayPlayer = feed.Teams?.Away?.Players.Where(x => x.Value != null).Select(y => y.Value).ToList();
							ProcessPlayerGameBoxscores(context, dbGame.AwayTeamID.Value, dbGame.Season, dbPlayerBoxscores, awayPlayer);
							var homePlayers = feed.Teams?.Home?.Players.Where(x => x.Value != null).Select(y => y.Value).ToList();
							ProcessPlayerGameBoxscores(context, dbGame.HomeTeamID.Value, dbGame.Season, dbPlayerBoxscores, homePlayers);

							var dbPlayerHittingBoxscores = context.PlayerHittingBoxscores.Where(x => x.GameID == this.GameId).ToDictionary(x => x.PlayerID);
							var awayHitters = feed.Teams?.Away?.Players.Where(x => x.Value.Stats?.Batting != null && !x.Value.Stats.Batting.IsDefault()).Select(y => y.Value).ToList();
							ProcessHitterBoxscores(context, dbGame.AwayTeamID.Value, dbGame.Season, dbPlayerHittingBoxscores, awayHitters);
							var homeHitters = feed.Teams?.Home?.Players.Where(x => x.Value.Stats?.Batting != null && !x.Value.Stats.Batting.IsDefault()).Select(y => y.Value).ToList();
							ProcessHitterBoxscores(context, dbGame.HomeTeamID.Value, dbGame.Season, dbPlayerHittingBoxscores, homeHitters);

							var dbPlayerPitchingBoxscores = context.PlayerPitchingBoxscores.Where(x => x.GameID == this.GameId).ToDictionary(x => x.PlayerID);
							var awayPitchers = feed.Teams?.Away?.Players.Where(x => x.Value.Stats?.Pitching != null && !x.Value.Stats.Pitching.IsDefault()).Select(y => y.Value).ToList();
							ProcessPitcherBoxscores(context, dbGame.AwayTeamID.Value, dbGame.Season, dbPlayerPitchingBoxscores, awayPitchers);
							var homePitchers = feed.Teams?.Home?.Players.Where(x => x.Value.Stats?.Pitching != null && !x.Value.Stats.Pitching.IsDefault()).Select(y => y.Value).ToList();
							ProcessPitcherBoxscores(context, dbGame.HomeTeamID.Value, dbGame.Season, dbPlayerPitchingBoxscores, homePitchers);

							if (gameHasFieldingBoxscores)
							{
								var dbPlayerFieldingBoxscores = context.PlayerFieldingBoxscores.Where(x => x.GameID == this.GameId).ToDictionary(x => x.PlayerID);
								var awayFielders = feed.Teams?.Away?.Players.Where(x => x.Value.Stats?.Fielding != null && !x.Value.Stats.Fielding.IsDefault()).Select(y => y.Value).ToList();
								ProcessFielderBoxscores(context, dbGame.AwayTeamID.Value, dbGame.Season, dbPlayerFieldingBoxscores, awayFielders);
								var homeFielders = feed.Teams?.Home?.Players.Where(x => x.Value.Stats?.Fielding != null && !x.Value.Stats.Fielding.IsDefault()).Select(y => y.Value).ToList();
								ProcessFielderBoxscores(context, dbGame.HomeTeamID.Value, dbGame.Season, dbPlayerFieldingBoxscores, homeFielders);
							}
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

		private void ProcessPlayerTeamSeasons(MlbStatsContext context, int teamId, int season, IEnumerable<Feeds.BoxscoreFeed.GamePlayer> feedPlayers)
		{
			var playerIds = feedPlayers.Select(x => x.Person.Id).ToList();
			var dbPlayers = context.Players.Where(x => playerIds.Contains(x.PlayerID)).ToDictionary(x => x.PlayerID);
			var dbPlayerTeamSeasons = context.PlayerTeamSeasons.Where(x => playerIds.Contains(x.PlayerID) && x.TeamID == teamId && x.Season == season).ToDictionary(x => x.PlayerID);
			if (dbPlayers.Count == playerIds.Count && dbPlayerTeamSeasons.Count == playerIds.Count)
			{
				// NO DB CHANGES NEEDED
				return;
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
				context.SaveChanges();
			}
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
										|| feedBox.GroundIntoTriplePlay != dbBoxscore.GroundIntoTriplePlay
										|| feedPlayer.BattingOrder != dbBoxscore.BattingOrder
										|| feedPlayer.Position?.Abbreviation != dbBoxscore.PosAbbr;
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
						dbBoxscore.GamePlayed = ((feedBox.GamesPlayed ?? 0) == 1) || feedBox.AtBats > 0 || feedPlayer.BattingOrder != null;
						dbBoxscore.CaughtStealing = (byte?)feedBox.CaughtStealing;
						dbBoxscore.Pickoffs = (byte?)feedBox.Pickoffs;
						dbBoxscore.StolenBases = (byte?)feedBox.StolenBases;
						dbBoxscore.SacBunts = (byte?)feedBox.SacBunts;
						dbBoxscore.SacFlies = (byte?)feedBox.SacFlies;
						dbBoxscore.IntentionalWalks = (byte?)feedBox.IntentionalWalks;
						dbBoxscore.CatcherInterferences = (byte?)feedBox.CatchersInterference;
						dbBoxscore.GroundIntoDoublePlay = (byte?)feedBox.GroundIntoDoublePlay;
						dbBoxscore.GroundIntoTriplePlay = (byte?)feedBox.GroundIntoTriplePlay;
						dbBoxscore.BattingOrder = feedPlayer.BattingOrder;
						dbBoxscore.PosAbbr = feedPlayer.Position?.Abbreviation;
					}
				}
			}
		}

		private void ProcessPitcherBoxscores(MlbStatsContext context, int teamId, int season,
			Dictionary<int, PlayerPitchingBoxscore> dbGameBoxscores,
			List<Feeds.BoxscoreFeed.GamePlayer> feedPlayers)
		{
			foreach (var feedPlayer in feedPlayers)
			{
				if (feedPlayer?.Stats?.Pitching != null)
				{
					bool updateStats = false;
					if (!dbGameBoxscores.TryGetValue(feedPlayer.Person.Id, out PlayerPitchingBoxscore dbBoxscore))
					{
						updateStats = true;
						dbBoxscore = new PlayerPitchingBoxscore
						{
							GameID = this.GameId,
							TeamID = teamId,
							PlayerID = feedPlayer.Person.Id,
							Season = season
						};
						context.PlayerPitchingBoxscores.Add(dbBoxscore);
						dbGameBoxscores.Add(feedPlayer.Person.Id, dbBoxscore);
					}

					var feedBox = feedPlayer.Stats.Pitching;

					// NOT NEW - STATS MUST BE DIFFERENT TO UPDATE
					// MAKE SURE NO UPDATES HAVE BEEN MADE TO STATS
					if (!updateStats)
					{
						updateStats = feedBox.AirOuts != dbBoxscore.AirOuts
										|| feedBox.AtBats != dbBoxscore.AtBats
										|| feedBox.Balls != dbBoxscore.Balls
										|| feedBox.BaseOnBalls != dbBoxscore.BaseOnBalls
										|| feedBox.BattersFaced != dbBoxscore.BattersFaced
										|| feedBox.HitBatsmen != dbBoxscore.BattersHit
										|| feedBox.Hits != dbBoxscore.Hits
										|| feedBox.HomeRuns != dbBoxscore.HomeRuns
										|| feedBox.InheritedRunners != dbBoxscore.InheritedRunners
										|| feedBox.InheritedRunnersScored != dbBoxscore.InheritedRunnersScored
										|| feedBox.InningsPitched != dbBoxscore.InningsPitched
										|| feedBox.IntentionalWalks != dbBoxscore.IntentionalWalks
										|| feedBox.PitchesThrown != dbBoxscore.PitchCount
										|| feedBox.Outs != dbBoxscore.Outs
										|| feedBox.Pickoffs != dbBoxscore.Pickoffs
										|| feedBox.Rbi != dbBoxscore.RunsBattedIn
										|| feedBox.Runs != dbBoxscore.Runs
										|| feedBox.SacBunts != dbBoxscore.SacBunts
										|| feedBox.SacFlies != dbBoxscore.SacFlies
										|| feedBox.StolenBases != dbBoxscore.StolenBases
										|| feedBox.StrikeOuts != dbBoxscore.StrikeOuts
										|| feedBox.Strikes != dbBoxscore.Strikes
										|| feedBox.Triples != dbBoxscore.Triples
										|| feedBox.WildPitches != dbBoxscore.WildPitches
										|| feedBox.GroundOuts != dbBoxscore.GroundOuts
										|| feedBox.CaughtStealing != dbBoxscore.CaughtStealing
										|| feedBox.EarnedRuns != dbBoxscore.EarnedRuns
										|| feedBox.Doubles != dbBoxscore.Doubles
										|| feedBox.CatchersInterference != dbBoxscore.CathersInterference
										|| feedBox.FlyOuts != dbBoxscore.FlyOuts
										|| (feedBox.Holds == 1) != dbBoxscore.IsHold
										|| (feedBox.Losses == 1) != dbBoxscore.IsLoss
										|| (feedBox.SaveOpportunities == 1) != dbBoxscore.IsSaveOpp
										|| (feedBox.Saves == 1) != dbBoxscore.IsSave
										|| (feedBox.Shutouts == 1) != dbBoxscore.IsShutout
										|| (feedBox.Wins == 1) != dbBoxscore.IsWin
										|| (feedBox.GamesFinished == 1) != dbBoxscore.IsFinish
										|| (feedBox.GamesStarted == 1) != dbBoxscore.IsStart
										|| (feedBox.GamesPitched == 1) != dbBoxscore.GamePitched
										|| (feedBox.BlownSaves == 1) != dbBoxscore.IsBlownSave
										|| (feedBox.CompleteGames == 1) != dbBoxscore.IsCompleteGame;
					}

					if (updateStats)
					{
						dbBoxscore.AirOuts = (byte?)feedBox.AirOuts;
						dbBoxscore.AtBats = (byte?)feedBox.AtBats;
						dbBoxscore.Balls = (byte?)feedBox.Balls;
						dbBoxscore.BaseOnBalls = (byte?)feedBox.BaseOnBalls;
						dbBoxscore.BattersFaced = (byte?)feedBox.BattersFaced;
						dbBoxscore.BattersHit = (byte?)feedBox.HitBatsmen;
						dbBoxscore.Hits = (byte?)feedBox.Hits;
						dbBoxscore.HomeRuns = (byte?)feedBox.HomeRuns;
						dbBoxscore.InheritedRunners = (byte?)feedBox.InheritedRunners;
						dbBoxscore.InheritedRunnersScored = (byte?)feedBox.InheritedRunnersScored;
						dbBoxscore.IntentionalWalks = (byte?)feedBox.IntentionalWalks;
						dbBoxscore.PitchCount = (byte?)feedBox.PitchesThrown;
						dbBoxscore.Outs = (byte?)feedBox.Outs;
						dbBoxscore.Pickoffs = (byte?)feedBox.Pickoffs;
						dbBoxscore.RunsBattedIn = (byte?)feedBox.Rbi;
						dbBoxscore.Runs = (byte?)feedBox.Runs;
						dbBoxscore.SacBunts = (byte?)feedBox.SacBunts;
						dbBoxscore.SacFlies = (byte?)feedBox.SacFlies;
						dbBoxscore.StolenBases = (byte?)feedBox.StolenBases;
						dbBoxscore.StrikeOuts = (byte?)feedBox.StrikeOuts;
						dbBoxscore.Strikes = (byte?)feedBox.Strikes;
						dbBoxscore.Triples = (byte?)feedBox.Triples;
						dbBoxscore.WildPitches = (byte?)feedBox.WildPitches;
						dbBoxscore.GroundOuts = (byte?)feedBox.GroundOuts;
						dbBoxscore.CaughtStealing = (byte?)feedBox.CaughtStealing;
						dbBoxscore.EarnedRuns = (byte?)feedBox.EarnedRuns;
						dbBoxscore.Doubles = (byte?)feedBox.Doubles;
						dbBoxscore.CathersInterference = (byte?)feedBox.CatchersInterference;
						dbBoxscore.FlyOuts = (byte?)feedBox.FlyOuts;
						dbBoxscore.InningsPitched = feedBox.InningsPitched;
						dbBoxscore.IsHold = (feedBox.Holds == 1);
						dbBoxscore.IsLoss = (feedBox.Losses == 1);
						dbBoxscore.IsSaveOpp = (feedBox.SaveOpportunities == 1);
						dbBoxscore.IsSave = (feedBox.Saves == 1);
						dbBoxscore.IsShutout = (feedBox.Shutouts == 1);
						dbBoxscore.IsWin = (feedBox.Wins == 1);
						dbBoxscore.IsFinish = (feedBox.GamesFinished == 1);
						dbBoxscore.IsStart = (feedBox.GamesStarted == 1);
						dbBoxscore.GamePitched = (feedBox.GamesPitched == 1);
						dbBoxscore.IsBlownSave = (feedBox.BlownSaves == 1);
						dbBoxscore.IsCompleteGame = (feedBox.CompleteGames == 1);
					}
				}
			}
		}

		private void ProcessFielderBoxscores(MlbStatsContext context, int teamId, int season,
			Dictionary<int, PlayerFieldingBoxscore> dbGameBoxscores,
			List<Feeds.BoxscoreFeed.GamePlayer> feedPlayers)
		{
			foreach (var feedPlayer in feedPlayers)
			{
				if (feedPlayer?.Stats?.Fielding != null)
				{
					bool updateStats = false;
					if (!dbGameBoxscores.TryGetValue(feedPlayer.Person.Id, out PlayerFieldingBoxscore dbBoxscore))
					{
						updateStats = true;
						dbBoxscore = new PlayerFieldingBoxscore
						{
							GameID = this.GameId,
							TeamID = teamId,
							PlayerID = feedPlayer.Person.Id,
							Season = season
						};
						context.PlayerFieldingBoxscores.Add(dbBoxscore);
						dbGameBoxscores.Add(feedPlayer.Person.Id, dbBoxscore);
					}

					var feedBox = feedPlayer.Stats.Fielding;

					// NOT NEW - STATS MUST BE DIFFERENT TO UPDATE
					// MAKE SURE NO UPDATES HAVE BEEN MADE TO STATS
					if (!updateStats)
					{
						updateStats = feedBox.Assists != dbBoxscore.Assists
										|| feedBox.CaughtStealing != dbBoxscore.CaughtStealing
										|| feedBox.Chances != dbBoxscore.Chances
										|| feedBox.Errors != dbBoxscore.Errors
										|| feedBox.PassedBall != dbBoxscore.PassedBall
										|| feedBox.Pickoffs != dbBoxscore.Pickoffs
										|| feedBox.PutOuts != dbBoxscore.PutOuts
										|| feedBox.StolenBases != dbBoxscore.StolenBases
										|| feedPlayer.Position?.Abbreviation != dbBoxscore.PosAbbr;
					}

					if (updateStats)
					{
						dbBoxscore.Assists = (byte?)feedBox.Assists;
						dbBoxscore.CaughtStealing = (byte?)feedBox.CaughtStealing;
						dbBoxscore.Chances = (byte?)feedBox.Chances;
						dbBoxscore.Errors = (byte?)feedBox.Errors;
						dbBoxscore.PassedBall = (byte?)feedBox.PassedBall;
						dbBoxscore.Pickoffs = (byte?)feedBox.Pickoffs;
						dbBoxscore.PutOuts = (byte?)feedBox.PutOuts;
						dbBoxscore.StolenBases = (byte?)feedBox.StolenBases;
						dbBoxscore.PosAbbr = feedPlayer.Position?.Abbreviation;
					}
				}
			}
		}

		private void ProcessPlayerGameBoxscores(MlbStatsContext context, int teamId, int season,
			Dictionary<int, PlayerGameBoxscore> dbGameBoxscores,
			List<Feeds.BoxscoreFeed.GamePlayer> feedPlayers)
		{
			foreach (var feedPlayer in feedPlayers)
			{
				if (feedPlayer != null)
				{
					bool updateStats = false;
					if (!dbGameBoxscores.TryGetValue(feedPlayer.Person.Id, out PlayerGameBoxscore dbBoxscore))
					{
						updateStats = true;
						dbBoxscore = new PlayerGameBoxscore
						{
							GameID = this.GameId,
							TeamID = teamId,
							PlayerID = feedPlayer.Person.Id,
							Season = season
						};
						context.PlayerGameBoxscores.Add(dbBoxscore);
						dbGameBoxscores.Add(feedPlayer.Person.Id, dbBoxscore);
					}

					// NOT NEW - VALUES MUST BE DIFFERENT TO UPDATE
					// MAKE SURE NO UPDATES HAVE BEEN MADE TO STATS
					string allPositionString = feedPlayer.AllPositions == null ? null : string.Join(";", feedPlayer.AllPositions.Select(x => x.Abbreviation));
					bool hasHitting = feedPlayer.Stats?.Batting != null && !feedPlayer.Stats.Batting.IsDefault();
					bool hasPitching = feedPlayer.Stats?.Pitching != null && !feedPlayer.Stats.Pitching.IsDefault();
					bool hasFielding = feedPlayer.Stats?.Fielding != null && !feedPlayer.Stats.Fielding.IsDefault();

					if (!updateStats)
					{
						updateStats = dbBoxscore.AllPositions != allPositionString
										|| dbBoxscore.HasHitting != hasHitting
										|| dbBoxscore.HasPitching != hasPitching
										|| dbBoxscore.HasFielding != hasFielding
										|| dbBoxscore.IsOnBench != feedPlayer?.GameStatus.IsOnBench
										|| dbBoxscore.IsSub != feedPlayer?.GameStatus.IsSubstitute
										|| dbBoxscore.JerseyNumber != feedPlayer.JerseyNumber
										|| dbBoxscore.PosAbbr != feedPlayer.Position?.Abbreviation
										|| dbBoxscore.Status != feedPlayer.Status?.Code;
					}

					if (updateStats)
					{
						dbBoxscore.AllPositions = allPositionString;
						dbBoxscore.HasHitting = hasHitting;
						dbBoxscore.HasPitching = hasPitching;
						dbBoxscore.HasFielding = hasFielding;
						dbBoxscore.IsOnBench = feedPlayer?.GameStatus.IsOnBench;
						dbBoxscore.IsSub = feedPlayer?.GameStatus.IsSubstitute;
						dbBoxscore.JerseyNumber = feedPlayer.JerseyNumber;
						dbBoxscore.PosAbbr = feedPlayer.Position?.Abbreviation;
						dbBoxscore.Status = feedPlayer.Status?.Code;
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
