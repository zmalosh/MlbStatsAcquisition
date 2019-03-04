using MlbStatsAcquisition.Model;
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

				if (feed != null && feed.AllPlays != null && feed.AllPlays.Count > 0)
				{
					var dbGame = context.Games.SingleOrDefault(x => x.GameID == this.GameId);
					if (dbGame == null)
					{
						throw new NullReferenceException($"GAME NOT FOUND IN DB: {this.GameId}");
					}
					int season = dbGame.Season;

					var dbPlaysDict = context.GamePlays.Where(x => x.GameID == dbGame.GameID).ToDictionary(x => x.GamePlayIndex);
					var dbPlayRunnersLookup = context.GamePlayRunners.Include("FieldingCredits").Where(x => x.GamePlay.GameID == dbGame.GameID).ToLookup(x => x.PlayIndex);

					var feedPlayers = feed.AllPlays.Where(x => x.Matchup?.Pitcher != null && x.Matchup?.Batter != null)
													.SelectMany(x => new[]{
														new {
															IsHome = x.About.HalfInning != "top",
															PlayerId = x.Matchup.Batter.Id,
															Name = x.Matchup.Batter.FullName,
															Link = x.Matchup.Batter.Link
														},
														new {
															IsHome = x.About.HalfInning == "top",
															PlayerId = x.Matchup.Pitcher.Id,
															Name = x.Matchup.Pitcher.FullName,
															Link = x.Matchup.Pitcher.Link
														}
													})
													.GroupBy(x => x.PlayerId)
													.Select(x => x.FirstOrDefault())
													.Where(x => x != null)
													.ToList();
					var feedPlayerIds = feedPlayers.Select(x => x.PlayerId).ToList();
					var dbPlayersDict = context.Players.Where(x => feedPlayerIds.Contains(x.PlayerID)).ToDictionary(x => x.PlayerID);
					foreach (var feedPlayer in feedPlayers)
					{
						if (!dbPlayersDict.ContainsKey(feedPlayer.PlayerId))
						{
							int teamId = feedPlayer.IsHome ? dbGame.HomeTeamID.Value : dbGame.AwayTeamID.Value;
							var dbPlayer = new Player
							{
								PlayerID = feedPlayer.PlayerId,
								PlayerLink = feedPlayer.Link,
								FullName = feedPlayer.Name
							};
							dbPlayersDict.Add(dbPlayer.PlayerID, dbPlayer);
							context.Players.Add(dbPlayer);
							var pts = new PlayerTeamSeason { Player = dbPlayer, TeamID = teamId, Season = season };
							context.PlayerTeamSeasons.Add(pts);
						}
					}
					context.SaveChanges();


					// TODO: IF ANY PLAY HAS CHANGED, DELETE ALL PLAYS AND REPROCESS FROM SCRATCH

					Player pitcher = null;
					foreach (var feedPlay in feed.AllPlays)
					{
						short playIndex = feedPlay.AtBatIndex;

						byte outCount = 0;
						RunnerLocation startRunners = RunnerLocation.None, endRunners = RunnerLocation.None;
						if (feedPlay.Runners != null && feedPlay.Runners.Count > 0)
						{
							outCount = (byte)feedPlay.Runners.Count(x => x.Movement.IsOut ?? false);
							var feedRunnersByRunnerId = feedPlay.Runners.GroupBy(x => x.Details.Runner.Id).ToList();
							foreach (var feedRunner in feedRunnersByRunnerId)
							{
								var start = (RunnerLocation)feedRunner.Select(x => GetRunnerLocationFromString(x.Movement?.Start, true)).Min(x => (int)x);
								var end = (RunnerLocation)feedRunner.Select(x => GetRunnerLocationFromString(x.Movement?.End, false)).Max(x => (int)x);
								if (start != RunnerLocation.Home_End)
								{
									startRunners = startRunners | start;
								}
								if (end != RunnerLocation.Home_End)
								{
									endRunners = endRunners | end;
								}
							}
						}

						bool isNew = !dbPlaysDict.TryGetValue(feedPlay.AtBatIndex, out GamePlay dbPlay);
						if (isNew)
						{
							dbPlay = new GamePlay
							{
								Game = dbGame,
								GamePlayIndex = playIndex,
								Season = season,
								IsInningTop = string.Equals(feedPlay.About.HalfInning, "top"),
								Inning = feedPlay.About.Inning,
							};
							dbPlaysDict.Add(playIndex, dbPlay);
							context.GamePlays.Add(dbPlay);
						}

						bool isUpdate = false;
						if (!isNew)
						{
							isUpdate = CheckPlayForUpdate(feedPlay, dbPlay, outCount, startRunners, endRunners);
						}

						if (isNew || isUpdate)
						{
							var feedPitcher = feedPlay.Matchup.Pitcher;
							if (pitcher == null || pitcher.PlayerID != feedPitcher.Id)
							{
								pitcher = dbPlayersDict[feedPitcher.Id];
							}

							var feedBatter = feedPlay.Matchup.Batter;
							if (!dbPlayersDict.TryGetValue(feedBatter.Id, out Player batter))
							{
								batter = dbPlayersDict[feedPlay.Matchup.Batter.Id];
							}

							dbPlay.StartTime = feedPlay.About.StartTime;
							dbPlay.EndTime = feedPlay.About.EndTime;
							dbPlay.IsReview = feedPlay.About.HasReview;
							dbPlay.PlayType = feedPlay.Result.Type;
							dbPlay.PlayEvent = feedPlay.Result.Event;
							dbPlay.PlayEventType = feedPlay.Result.EventType;
							dbPlay.GamePlayDescription = feedPlay.Result.Description;
							dbPlay.ScoreAway = feedPlay.Result.AwayScore;
							dbPlay.ScoreHome = feedPlay.Result.HomeScore;
							dbPlay.RunsScored = feedPlay.Result.Rbi;
							dbPlay.RunnerStatusStart = startRunners;
							dbPlay.RunnerStatusEnd = endRunners;
							dbPlay.BatterID = batter.PlayerID;
							dbPlay.PitcherID = pitcher.PlayerID;
							dbPlay.BatterHand = feedPlay.Matchup.BatSide.Code[0];
							dbPlay.PitcherHand = feedPlay.Matchup.PitchHand.Code[0];
							dbPlay.BatterSplit = feedPlay.Matchup.Splits?.Batter;
							dbPlay.PitcherSplit = feedPlay.Matchup.Splits?.Pitcher;
							dbPlay.Strikes = (byte)feedPlay.Count.Strikes;
							dbPlay.Balls = (byte)feedPlay.Count.Balls;
							dbPlay.OutsEnd = (byte)feedPlay.Count.Outs;
							dbPlay.OutsStart = (byte)(feedPlay.Count.Outs - outCount);
						}

						var dbPlayRunners = dbPlayRunnersLookup[playIndex];
						var feedRunners = feedPlay.Runners;
						if (feedRunners != null && feedRunners.Count > 0)
						{
							foreach (var feedRunner in feedRunners)
							{
								if (feedRunner.Movement != null && feedRunner.Details != null)
								{
									var runnerId = feedRunner.Details.Runner.Id;
									var startLocation = GetRunnerLocationFromString(feedRunner.Movement.Start, true);
									var endLocation = GetRunnerLocationFromString(feedRunner.Movement.Start, false);
									var dbPlayRunner = dbPlayRunners.SingleOrDefault(x => x.RunnerID == runnerId && x.StartRunnerLocation == startLocation);

									bool isRunnerNew = false;
									if (dbPlayRunner == null)
									{
										isRunnerNew = true;
										dbPlayRunner = new GamePlayRunner
										{
											GamePlay = dbPlay,
											RunnerID = runnerId,
											StartRunnerLocation = startLocation,
											EndRunnerLocation = endLocation
										};
										if (dbPlay.Runners == null)
										{
											dbPlay.Runners = new List<GamePlayRunner>();
										}
										dbPlay.Runners.Add(dbPlayRunner);
									}

									bool isRunnerUpdate = false;
									if (!isNew)
									{
										isRunnerUpdate = CheckPlayRunnerForUpdate(feedRunner, dbPlayRunner, startLocation, endLocation);
									}

									if (isRunnerNew || isRunnerUpdate)
									{
										dbPlayRunner.IsEarned = feedRunner.Details.Earned;
										dbPlayRunner.IsOut = feedRunner.Movement.IsOut;
										dbPlayRunner.IsScore = endLocation == RunnerLocation.Home_End;
										dbPlayRunner.IsTeamUnearned = feedRunner.Details.TeamUnearned;
										dbPlayRunner.MovementReason = feedRunner.Details.MovementReason;
										dbPlayRunner.OutLocation = (dbPlayRunner.IsOut ?? false)
																	? GetRunnerLocationFromString(feedRunner.Movement.OutBase, false)
																	: (RunnerLocation?)null;
										dbPlayRunner.OutNumber = feedRunner.Movement.OutNumber;
										dbPlayRunner.PlayEvent = feedRunner.Details.Event;
										dbPlayRunner.PlayEventType = feedRunner.Details.EventType;
										dbPlayRunner.PlayIndex = feedRunner.Details.PlayIndex;
										dbPlayRunner.PitcherResponsibleID = feedRunner.Details.ResponsiblePitcher?.Id;
										if (feedRunner.Credits != null && feedRunner.Credits.Count > 0)
										{
											if (dbPlayRunner.FieldingCredits == null)
											{
												dbPlayRunner.FieldingCredits = new List<GamePlayFieldingCredit>();
											}

											var dbCreditsToDelete = dbPlayRunner.FieldingCredits.ToList(); // EAGER LOAD
											foreach (var feedCredit in feedRunner.Credits)
											{
												var creditType = GetCreditTypeFromString(feedCredit.CreditCredit);
												var dbCredit = dbPlayRunner.FieldingCredits.SingleOrDefault(x => x.FielderID == feedCredit.Player.Id && x.CreditType == creditType);
												if (dbCredit != null)
												{
													dbCreditsToDelete.Remove(dbCredit);
													dbCredit.CreditType = creditType;
													dbCredit.FielderID = feedCredit.Player.Id;
													dbCredit.PosAbbr = feedCredit.Position?.Abbreviation;
													context.SaveChanges();
												}
												else
												{
													dbCredit = new GamePlayFieldingCredit
													{
														PlayRunner = dbPlayRunner,
														PlayRunnerID = dbPlayRunner.GamePlayRunnerID,
														CreditType = creditType,
														FielderID = feedCredit.Player.Id,
														PosAbbr = feedCredit.Position?.Abbreviation
													};
													context.GamePlayFieldingCredits.Add(dbCredit);
													context.SaveChanges();
												}
											}
											foreach (var dbCredit in dbCreditsToDelete)
											{
												context.GamePlayFieldingCredits.Remove(dbCredit);
												context.SaveChanges();
											}
										}
									}
								}
							}
						}
					}
					context.SaveChanges();
				}
			}
		}

		private bool CheckPlayForUpdate(Feeds.PlayByPlayFeed.FeedPlay feedPlay, GamePlay dbPlay,
			byte outCount, RunnerLocation runnersStart, RunnerLocation runnersEnd)
		{
			return feedPlay.About.StartTime != dbPlay.StartTime
					|| feedPlay.About.EndTime != dbPlay.EndTime
					|| feedPlay.About.HasReview != dbPlay.IsReview
					|| feedPlay.Result.Type != dbPlay.PlayType
					|| feedPlay.Result.Event != dbPlay.PlayEvent
					|| feedPlay.Result.EventType != dbPlay.PlayEventType
					|| feedPlay.Result.AwayScore != dbPlay.ScoreAway
					|| feedPlay.Result.HomeScore != dbPlay.ScoreHome
					|| feedPlay.Result.Description != dbPlay.GamePlayDescription
					|| feedPlay.Result.Rbi != dbPlay.RunsScored
					|| feedPlay.Count.Strikes != dbPlay.Strikes
					|| feedPlay.Count.Balls != dbPlay.Balls
					|| feedPlay.Count.Outs != dbPlay.OutsEnd
					|| (feedPlay.Count.Outs - outCount) != dbPlay.OutsStart
					|| feedPlay.Matchup?.Batter?.Id != dbPlay.BatterID
					|| feedPlay.Matchup?.Pitcher?.Id != dbPlay.PitcherID
					|| feedPlay.Matchup?.BatSide.Code != dbPlay.BatterHand.ToString()
					|| feedPlay.Matchup?.PitchHand.Code != dbPlay.PitcherHand.ToString()
					|| feedPlay.Matchup?.Splits?.Pitcher != dbPlay.PitcherSplit
					|| feedPlay.Matchup?.Splits?.Batter != dbPlay.BatterSplit
					|| runnersStart != dbPlay.RunnerStatusStart
					|| runnersEnd != dbPlay.RunnerStatusEnd;
		}

		private bool CheckPlayRunnerForUpdate(Feeds.PlayByPlayFeed.Runner feedRunner, GamePlayRunner dbRunner, RunnerLocation startLocation, RunnerLocation endLocation)
		{
			return dbRunner.IsEarned != feedRunner.Details.Earned
				|| dbRunner.IsOut != feedRunner.Movement.IsOut
				|| dbRunner.IsScore != (endLocation == RunnerLocation.Home_End)
				|| dbRunner.IsTeamUnearned != feedRunner.Details.TeamUnearned
				|| dbRunner.MovementReason != feedRunner.Details.MovementReason
				|| dbRunner.OutLocation != ((feedRunner.Movement.IsOut ?? false)
											? GetRunnerLocationFromString(feedRunner.Movement.OutBase, false)
											: (RunnerLocation?)null)
				|| dbRunner.OutNumber != feedRunner.Movement.OutNumber
				|| dbRunner.PlayEvent != feedRunner.Details.Event
				|| dbRunner.PlayEventType != feedRunner.Details.EventType
				|| dbRunner.PlayIndex != feedRunner.Details.PlayIndex
				|| dbRunner.PitcherResponsibleID != feedRunner.Details.ResponsiblePitcher?.Id
				|| CheckPlayRunnerFieldingCreditsForUpdate(feedRunner.Credits, dbRunner.FieldingCredits);
		}

		private bool CheckPlayRunnerFieldingCreditsForUpdate(List<Feeds.PlayByPlayFeed.Credit> feedCredits, IEnumerable<GamePlayFieldingCredit> dbCredits)
		{
			if ((feedCredits?.Count ?? 0) != (dbCredits?.Count() ?? 0)) { return true; }
			return feedCredits.Any(x => dbCredits.Any(y => GetCreditTypeFromString(x.CreditCredit) != y.CreditType
															|| x.Player.Id != y.FielderID
															|| x.Position.Abbreviation != y.PosAbbr));
		}

		private static RunnerLocation GetRunnerLocationFromString(string str, bool isStart)
		{
			switch (str?.ToUpper())
			{
				case null: return RunnerLocation.None;
				case "1B": return RunnerLocation.First;
				case "2B": return RunnerLocation.Second;
				case "3B": return RunnerLocation.Third;
				case "4B": return isStart ? RunnerLocation.None : RunnerLocation.Home_End;
				case "SCORE": return RunnerLocation.Home_End;
				default: throw new ArgumentException("THAT'S NOT A BASE");
			}
		}

		private static GamePlayFieldingCreditType GetCreditTypeFromString(string str)
		{
			switch (str.ToUpper())
			{
				case "F_ASSIST_OF": return GamePlayFieldingCreditType.Assist_OF;
				case "F_ASSIST": return GamePlayFieldingCreditType.Assist_OF;
				case "F_PUTOUT": return GamePlayFieldingCreditType.Putout;
				case "F_FIELDED_BALL": return GamePlayFieldingCreditType.Fielded;
				case "F_FIELDING_ERROR": return GamePlayFieldingCreditType.Error_Fielding;
				case "F_THROWING_ERROR": return GamePlayFieldingCreditType.Error_Throwing;
				case "F_ERROR_DROPPED_BALL": return GamePlayFieldingCreditType.Error_Throwing;
				case "F_DEFLECTION": return GamePlayFieldingCreditType.Deflection;
				case "F_TOUCH": return GamePlayFieldingCreditType.Touch;
				case "F_INTERFERENCE": return GamePlayFieldingCreditType.Interference;
				default: throw new ArgumentException("YOU DON'T GET CREDIT FOR THAT!");
			}
		}
	}
}
