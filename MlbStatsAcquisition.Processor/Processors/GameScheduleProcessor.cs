using MlbStatsAcquisition.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class GameScheduleProcessor : IProcessor
	{
		private int Year { get; set; }
		private List<int> AssociationIds { get; set; }

		public GameScheduleProcessor(int year, List<int> associationIds)
		{
			this.Year = year;
			this.AssociationIds = associationIds;
		}

		public void Run(Model.MlbStatsContext context)
		{
			var dbAssociations = context.Associations.ToDictionary(x => x.AssociationID);
			var dbSeasonsByAssociation = context.AssociationSeasons.ToLookup(x => x.AssociationID, y => y.Season).ToDictionary(x => x.Key, y => y.ToList());
			var dbGameStatusIds = context.GameStatusTypes.ToDictionary(x => x.GameStatusCode, y => y.GameStatusTypeID);
			var dbVenues = context.Venues.ToDictionary(x => x.VenueID);
			var dbSeasonsByVenue = context.VenueSeasons.ToLookup(x => x.VenueID, y => y.Season).ToDictionary(x => x.Key, y => y.ToList());
			var dbTeams = context.Teams.ToDictionary(x => x.TeamID);
			var dbSeasonsByTeam = context.TeamSeasons.ToLookup(x => x.TeamID, y => y.Season).ToDictionary(x => x.Key, y => y.ToList());
			var dbAllGameIds = context.Games.Select(x => x.GameID).ToList();

			var newTeams = new List<Team>();
			var newTeamSeasons = new List<TeamSeason>();
			var newVenues = new List<Venue>();
			var newVenueSeasons = new List<VenueSeason>();
			var newGames = new List<Game>();
			var newAssociations = new List<Association>();
			var newAssociationSeasons = new List<AssociationSeason>();

			foreach (var associationId in this.AssociationIds)
			{
				Console.WriteLine($"GameScheduleProcessor - {this.Year} - {associationId}");

				var dbGames = context.Games.Where(x => x.GameTime.Year == this.Year && x.AssociationID == associationId).ToDictionary(x => x.GameID);

				var url = Feeds.GameScheduleFeed.GetFeedUrl(this.Year, associationId);
				var rawJson = JsonUtility.GetRawJsonFromUrl(url);
				var feed = Feeds.GameScheduleFeed.FromJson(rawJson);

				if (feed?.Dates != null && feed.Dates.Count > 0)
				{
					if (!dbAssociations.TryGetValue(associationId, out Model.Association dbAssociation))
					{
						dbAssociation = new Model.Association
						{
							AssociationID = associationId,
							IsEnabled = false
						};
						dbAssociations.Add(associationId, dbAssociation);
						newAssociations.Add(dbAssociation);
						dbSeasonsByAssociation[associationId] = new List<int>();
					}

					var feedSeasonOptions = feed.Dates.SelectMany(x => x.Games.Select(y => y.Season));
					foreach (var feedSeasonOption in feedSeasonOptions)
					{
						if (!dbSeasonsByAssociation.ContainsKey(associationId))
						{
							dbSeasonsByAssociation[associationId] = new List<int>();
						}
						if (!dbSeasonsByAssociation[associationId].Contains(feedSeasonOption))
						{
							var dbAssociationSeason = new Model.AssociationSeason
							{
								AssociationID = associationId,
								Season = feedSeasonOption
							};
							newAssociationSeasons.Add(dbAssociationSeason);
							dbSeasonsByAssociation[associationId].Add(feedSeasonOption);
						}
					}

					foreach (var feedDate in feed.Dates)
					{
						if (feedDate?.Games != null && feedDate.Games.Count > 0)
						{
							foreach (var feedGame in feedDate.Games)
							{
								Model.Venue dbVenue = null;
								if (feedGame.Venue?.Id != null && !dbVenues.TryGetValue(feedGame.Venue.Id, out dbVenue))
								{
									dbVenue = new Model.Venue
									{
										VenueID = feedGame.Venue.Id,
										VenueName = feedGame.Venue.Name,
										VenueLink = feedGame.Venue.Link
									};
									newVenues.Add(dbVenue);
									dbVenues.Add(dbVenue.VenueID, dbVenue);
								}

								Model.VenueSeason dbVenueSeason = null;
								if (!dbSeasonsByVenue.ContainsKey(feedGame.Venue.Id))
								{
									// THIS MAY BE THE FIRST VenueSeason.
									dbSeasonsByVenue[feedGame.Venue.Id] = new List<int>();
								}
								if (!dbSeasonsByVenue[feedGame.Venue.Id].Contains(feedGame.Season))
								{
									dbVenueSeason = new Model.VenueSeason
									{
										VenueID = feedGame.Venue.Id,
										Season = feedGame.Season,
										VenueName = feedGame.Venue.Name
									};
									newVenueSeasons.Add(dbVenueSeason);
									dbSeasonsByVenue[feedGame.Venue.Id].Add(feedGame.Season);
								}

								var feedAwayTeam = feedGame.Teams?.Away?.Team;
								Model.Team dbAwayTeam = null;
								if (feedAwayTeam?.Id != null && !dbTeams.TryGetValue(feedGame.Teams.Away.Team.Id, out dbAwayTeam))
								{
									// TODO: MAKE CALL TO TEAM API TO FILL IN FULL TEAM DATA
									dbAwayTeam = new Model.Team
									{
										TeamID = feedAwayTeam.Id,
										IsActive = false,
										IsAllStar = false,
										FirstSeason = null,
										TeamFullName = feedAwayTeam.Name,
										TeamName = feedAwayTeam.Name
									};
									newTeams.Add(dbAwayTeam);
									dbTeams.Add(dbAwayTeam.TeamID, dbAwayTeam);
								}

								Model.TeamSeason dbAwayTeamSeason = null;
								if (dbAwayTeam != null)
								{
									if (!dbSeasonsByTeam.ContainsKey(feedAwayTeam.Id))
									{
										dbSeasonsByTeam[feedAwayTeam.Id] = new List<int>();
									}
									if (!dbSeasonsByTeam[feedAwayTeam.Id].Contains(feedGame.Season))
									{
										dbAwayTeamSeason = new Model.TeamSeason
										{
											TeamID = feedAwayTeam.Id,
											Season = feedGame.Season,
											FullName = feedAwayTeam.Name,
											TeamName = feedAwayTeam.Name,
											AssociationID = associationId
										};
										newTeamSeasons.Add(dbAwayTeamSeason);
										dbSeasonsByTeam[feedAwayTeam.Id].Add(feedGame.Season);
									}
								}

								var feedHomeTeam = feedGame.Teams?.Home?.Team;
								Model.Team dbHomeTeam = null;
								if (feedHomeTeam?.Id != null && !dbTeams.TryGetValue(feedGame.Teams.Home.Team.Id, out dbHomeTeam))
								{
									// TODO: MAKE CALL TO TEAM API TO FILL IN FULL TEAM DATA
									dbHomeTeam = new Model.Team
									{
										TeamID = feedHomeTeam.Id,
										IsActive = false,
										IsAllStar = false,
										FirstSeason = null,
										TeamFullName = feedHomeTeam.Name,
										TeamName = feedHomeTeam.Name
									};
									newTeams.Add(dbHomeTeam);
									dbTeams.Add(dbHomeTeam.TeamID, dbHomeTeam);
								}

								Model.TeamSeason dbHomeTeamSeason = null;
								if (dbHomeTeam != null)
								{
									if (!dbSeasonsByTeam.ContainsKey(feedHomeTeam.Id))
									{
										dbSeasonsByTeam[feedHomeTeam.Id] = new List<int>();
									}
									if (!dbSeasonsByTeam[feedHomeTeam.Id].Contains(feedGame.Season))
									{
										dbHomeTeamSeason = new Model.TeamSeason
										{
											TeamID = feedHomeTeam.Id,
											Season = feedGame.Season,
											FullName = feedHomeTeam.Name,
											TeamName = feedHomeTeam.Name,
											AssociationID = associationId
										};
										newTeamSeasons.Add(dbHomeTeamSeason);
										dbSeasonsByTeam[feedHomeTeam.Id].Add(feedGame.Season);
									}
								}

								if (!dbGames.TryGetValue(feedGame.GamePk, out Model.Game dbGame))
								{
									if (dbAllGameIds.Contains(feedGame.GamePk))
									{
										// GAME IS RETURNED FOR MULTIPLE ASSOCIATIONS
										dbGame = context.Games.SingleOrDefault(x => x.GameID == feedGame.GamePk);
										if (dbGame == null)
										{
											dbGame = newGames.SingleOrDefault(x => x.GameID == feedGame.GamePk);
										}
										if (dbGame == null)
										{
											throw new NullReferenceException("GameID BUT NO GAME.... WTF?!");
										}
										if (dbGame.AltAssociationID.HasValue && dbGame.AltAssociationID.Value != associationId)
										{
											throw new ArgumentException(string.Format($"GAME HAS MORE THAN 2 ASSOCIATIONS. WTF?! - {feedGame.GamePk}"));
										}
										dbGame.AltAssociationID = associationId;
									}
									else
									{
										dbGame = new Model.Game
										{
											GameID = feedGame.GamePk,
											Season = feedGame.Season,
											AssociationID = associationId,
											GameTypeID = feedGame.GameType,
											AwayTeamID = feedGame.Teams?.Away?.Team?.Id,
											HomeTeamID = feedGame.Teams?.Home?.Team?.Id,
											VenueID = feedGame.Venue.Id,
											GameTime = feedGame.GameDate,
											GameStatus = dbGameStatusIds[feedGame.Status.StatusCode],
											AwayScore = (byte?)feedGame.Teams?.Away?.Score,
											HomeScore = (byte?)feedGame.Teams?.Home?.Score,
											IsTie = feedGame.IsTie ?? false,
											IsDoubleHeader = feedGame.IsDoubleHeader,
											DayGameNum = (byte)feedGame.GameNumber,
											IsDayGame = feedGame.IsDayGame,
											IsTBD = feedGame.Status?.StartTimeTbd ?? false,
											IsIfNecessary = feedGame.IfNecessary,
											IfNecessaryDescription = feedGame.IfNecessaryDescription,
											ScheduledLength = (byte)feedGame.ScheduledInnings,
											SeriesLength = (byte?)feedGame.GamesInSeries,
											SeriesGameNum = (byte?)feedGame.SeriesGameNumber,
											SeriesDescription = feedGame.SeriesDescription,
											RecordSource = feedGame.RecordSource,
											AwaySeriesNum = (byte?)feedGame.Teams?.Away?.SeriesNumber,
											AwayWins = (byte?)feedGame.Teams?.Away?.LeagueRecord?.Wins,
											AwayLosses = (byte?)feedGame.Teams?.Away?.LeagueRecord?.Losses,
											IsAwaySplitSquad = feedGame.Teams?.Away?.SplitSquad,
											HomeSeriesNum = (byte?)feedGame.Teams?.Home?.SeriesNumber,
											HomeWins = (byte?)feedGame.Teams?.Home?.LeagueRecord?.Wins,
											HomeLosses = (byte?)feedGame.Teams?.Home?.LeagueRecord?.Losses,
											IsHomeSplitSquad = feedGame.Teams?.Home?.SplitSquad,
											AltAssociationID = null,
											RawSeason = feedGame.RawSeason,
											RescheduledDate = feedGame.RescheduleDate,
											RescheduledFromDate = feedGame.RescheduledFrom,
											ResumeDate = feedGame.ResumeDate,
											ResumedFrom = feedGame.ResumedFrom
										};
										newGames.Add(dbGame);
										dbGames.Add(dbGame.GameID, dbGame);
										dbAllGameIds.Add(dbGame.GameID);
									}
								}
								else
								{
									; // TODO: UPDATE GAME VALUES
								}

							}
						}
					}
					context.SaveChanges();
				}
			}
			context.Associations.AddRange(newAssociations);
			context.Teams.AddRange(newTeams);
			context.Venues.AddRange(newVenues);
			context.SaveChanges();
			context.AssociationSeasons.AddRange(newAssociationSeasons);
			context.VenueSeasons.AddRange(newVenueSeasons);
			context.SaveChanges();
			context.TeamSeasons.AddRange(newTeamSeasons);
			context.SaveChanges();
			context.Games.AddRange(newGames);
			context.SaveChanges();
		}
	}
}
