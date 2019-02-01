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
		private int Season { get; set; }
		private List<int> AssociationIds { get; set; }

		public GameScheduleProcessor(int season, List<int> associationIds)
		{
			this.Season = season;
			this.AssociationIds = associationIds;
		}

		public void Run(Model.MlbStatsContext context)
		{
			var dbAssociations = context.Associations.ToDictionary(x => x.AssociationID);
			var dbAssociationSeasons = context.AssociationSeasons.Where(x => x.Season == this.Season).ToDictionary(x => x.AssociationID);
			var dbGameStatusIds = context.GameStatusTypes.ToDictionary(x => x.GameStatusCode, y => y.GameStatusTypeID);
			var dbVenues = context.Venues.ToDictionary(x => x.VenueID);
			var dbVenueSeasons = context.VenueSeasons.Where(x => x.Season == this.Season).ToDictionary(y => y.VenueID);
			var dbTeams = context.Teams.ToDictionary(x => x.TeamID);
			var dbTeamSeasons = context.TeamSeasons.Where(x => x.Season == this.Season).ToDictionary(y => y.TeamID);
			var dbAllGameIds = context.Games.Select(x => x.GameID).ToList();

			foreach (var associationId in this.AssociationIds)
			{
				Console.WriteLine($"GameScheduleProcessor - {this.Season} - {associationId}");

				var dbGames = context.Games.Where(x => x.Season == this.Season && x.AssociationID == associationId).ToDictionary(x => x.GameID);

				Feeds.GameScheduleFeed feed;
				using (var client = new WebClient())
				{
					var url = Feeds.GameScheduleFeed.GetFeedUrl(this.Season, associationId);
					var rawJson = client.DownloadString(url);
					feed = Feeds.GameScheduleFeed.FromJson(rawJson);
				}

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
						context.Associations.Add(dbAssociation);
					}

					if (!dbAssociationSeasons.TryGetValue(dbAssociation.AssociationID, out Model.AssociationSeason dbAssociationSeason))
					{
						dbAssociationSeason = new Model.AssociationSeason
						{
							Association = dbAssociation,
							Season = this.Season
						};
						dbAssociationSeasons.Add(dbAssociation.AssociationID, dbAssociationSeason);
						context.AssociationSeasons.Add(dbAssociationSeason);
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
									dbVenues.Add(dbVenue.VenueID, dbVenue);
									context.Venues.Add(dbVenue);
								}

								Model.VenueSeason dbVenueSeason = null;
								if (dbVenue != null && !dbVenueSeasons.TryGetValue(dbVenue.VenueID, out dbVenueSeason))
								{
									dbVenueSeason = new Model.VenueSeason
									{
										Venue = dbVenue,
										Season = this.Season,
										VenueName = feedGame.Venue?.Name
									};
									dbVenueSeasons.Add(dbVenue.VenueID, dbVenueSeason);
									context.VenueSeasons.Add(dbVenueSeason);
								}

								var feedAwayTeam = feedGame.Teams?.Away?.Team;
								Model.Team dbAwayTeam = null;
								if (feedAwayTeam?.Id != null && !dbTeams.TryGetValue(feedGame.Teams.Away.Team.Id, out dbAwayTeam))
								{
									// TODO: MAKE CALL TO TEAM API TO FILL IN FULL TEAM DATA
									dbAwayTeam = new Model.Team
									{
										TeamID = feedAwayTeam.Id,
										Association = dbAssociation,
										IsActive = false,
										IsAllStar = false,
										FirstSeason = null,
										TeamFullName = feedAwayTeam.Name,
										TeamName = feedAwayTeam.Name
									};
									dbTeams.Add(dbAwayTeam.TeamID, dbAwayTeam);
									context.Teams.Add(dbAwayTeam);
								}

								Model.TeamSeason dbAwayTeamSeason = null;
								if (dbAwayTeam != null && !dbTeamSeasons.TryGetValue(dbAwayTeam.TeamID, out dbAwayTeamSeason))
								{
									dbAwayTeamSeason = new Model.TeamSeason
									{
										Team = dbAwayTeam,
										Season = this.Season,
										FullName = feedAwayTeam.Name,
										TeamName = feedAwayTeam.Name,
										AssociationSeason = dbAssociationSeason
									};
									dbTeamSeasons.Add(dbAwayTeam.TeamID, dbAwayTeamSeason);
								}

								var feedHomeTeam = feedGame.Teams?.Home?.Team;
								Model.Team dbHomeTeam = null;
								if (feedHomeTeam?.Id != null && !dbTeams.TryGetValue(feedGame.Teams.Home.Team.Id, out dbHomeTeam))
								{
									// TODO: MAKE CALL TO TEAM API TO FILL IN FULL TEAM DATA
									dbHomeTeam = new Model.Team
									{
										TeamID = feedHomeTeam.Id,
										Association = dbAssociation,
										IsActive = false,
										IsAllStar = false,
										FirstSeason = null,
										TeamFullName = feedHomeTeam.Name,
										TeamName = feedHomeTeam.Name
									};
									dbTeams.Add(dbHomeTeam.TeamID, dbHomeTeam);
									context.Teams.Add(dbHomeTeam);
								}

								Model.TeamSeason dbHomeTeamSeason = null;
								if (dbHomeTeam != null && !dbTeamSeasons.TryGetValue(dbHomeTeam.TeamID, out dbHomeTeamSeason))
								{
									dbHomeTeamSeason = new Model.TeamSeason
									{
										Team = dbHomeTeam,
										Season = this.Season,
										FullName = feedHomeTeam.Name,
										TeamName = feedHomeTeam.Name,
										AssociationSeason = dbAssociationSeason
									};
									dbTeamSeasons.Add(dbHomeTeam.TeamID, dbHomeTeamSeason);
								}

								if (!dbGames.TryGetValue(feedGame.GamePk, out Model.Game dbGame))
								{
									if (dbAllGameIds.Contains(feedGame.GamePk))
									{
										// GAME IS RETURNED FOR MULTIPLE ASSOCIATIONS
										dbGame = context.Games.Single(x => x.GameID == feedGame.GamePk);
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
											Association = dbAssociation,
											AssociationSeason = dbAssociationSeason,
											GameTypeID = feedGame.GameType,
											AwayTeam = dbAwayTeam,
											HomeTeam = dbHomeTeam,
											AwayTeamSeason = dbAwayTeamSeason,
											HomeTeamSeason = dbHomeTeamSeason,
											Venue = dbVenue,
											VenueSeason = dbVenueSeason,
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
										dbGames.Add(dbGame.GameID, dbGame);
										dbAllGameIds.Add(dbGame.GameID);
										context.Games.Add(dbGame);
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
		}
	}
}
