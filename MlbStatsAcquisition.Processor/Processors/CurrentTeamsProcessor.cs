using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class CurrentTeamsProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			Feeds.TeamsFeed feed;
			using (var client = new WebClient())
			{
				var url = Feeds.TeamsFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.TeamsFeed.FromJson(rawJson);
			}

			var dbAssociations = context.Associations.ToDictionary(x => x.AssociationID);
			var dbLeagues = context.Leagues.ToDictionary(x => x.LeagueID);
			var dbDivisions = context.Divisions.ToDictionary(x => x.DivisionID);
			var dbVenues = context.Venues.ToDictionary(x => x.VenueID);
			var dbTeams = context.Teams.ToDictionary(x => x.TeamID);

			var feedTeams = feed.Teams.OrderBy(x => x.ParentOrgId.HasValue ? 1 : 0).ThenBy(y => y.Sport.Id).ToList();

			foreach (var feedTeam in feedTeams)
			{
				if (!dbAssociations.TryGetValue(feedTeam.Sport.Id.Value, out Model.Association dbAssociation))
				{
					dbAssociation = new Model.Association
					{
						AssociationID = feedTeam.Sport.Id.Value,
						AssociationName = feedTeam.Sport.Name,
						AssociationLink = feedTeam.Sport.Link
					};
					dbAssociations.Add(dbAssociation.AssociationID, dbAssociation);
					context.Associations.Add(dbAssociation);
				}

				Model.League dbLeague = null;
				if (feedTeam.League?.Id != null && !dbLeagues.TryGetValue(feedTeam.League.Id.Value, out dbLeague))
				{
					dbLeague = new Model.League
					{
						LeagueID = feedTeam.League.Id.Value,
						LeagueName = feedTeam.League.Name,
						LeagueLink = feedTeam.League.Link,
						Association = dbAssociation
					};
					dbLeagues.Add(dbLeague.LeagueID, dbLeague);
					context.Leagues.Add(dbLeague);
				}

				Model.Division dbDivision = null;
				if (feedTeam.Division?.Id != null && !dbDivisions.TryGetValue(feedTeam.Division.Id.Value, out dbDivision))
				{
					dbDivision = new Model.Division
					{
						DivisionID = feedTeam.Division.Id.Value,
						DivisionName = feedTeam.Division.Name,
						DivisionLink = feedTeam.Division.Link,
						League = dbLeague
					};
					dbDivisions.Add(dbDivision.DivisionID, dbDivision);
					context.Divisions.Add(dbDivision);
				}

				Model.Venue dbVenue = null;
				if (feedTeam.Venue?.Id != null && !dbVenues.TryGetValue(feedTeam.Venue.Id.Value, out dbVenue))
				{
					dbVenue = new Model.Venue
					{
						VenueID = feedTeam.Venue.Id.Value,
						VenueName = feedTeam.Venue.Name,
						VenueLink = feedTeam.Venue.Link
					};
					dbVenues.Add(dbVenue.VenueID, dbVenue);
					context.Venues.Add(dbVenue);
				}

				Model.League dbSpringLeague = null;
				if (feedTeam.SpringLeague?.Id != null && !dbLeagues.TryGetValue(feedTeam.SpringLeague.Id.Value, out dbSpringLeague))
				{
					dbSpringLeague = new Model.League
					{
						LeagueID = feedTeam.SpringLeague.Id.Value,
						LeagueName = feedTeam.SpringLeague.Name,
						LeagueLink = feedTeam.SpringLeague.Link,
						Association = dbAssociation
					};
					dbLeagues.Add(dbSpringLeague.LeagueID, dbSpringLeague);
					context.Leagues.Add(dbSpringLeague);
				}

				Model.Team dbParentOrg = null;
				if (feedTeam.ParentOrgId.HasValue && !dbTeams.TryGetValue(feedTeam.ParentOrgId.Value, out dbParentOrg))
				{
					dbParentOrg = new Model.Team
					{
						TeamID = feedTeam.ParentOrgId.Value,
						TeamFullName = feedTeam.ParentOrgName
					};
					dbTeams.Add(dbParentOrg.TeamID, dbParentOrg);
					context.Teams.Add(dbParentOrg);
				}

				if (!dbTeams.TryGetValue(feedTeam.Id, out Model.Team dbTeam))
				{
					dbTeam = new Model.Team
					{
						TeamID = feedTeam.Id,
						Association = dbAssociation,
						League = dbLeague,
						SpringLeague = dbSpringLeague,
						Venue = dbVenue,
						Division = dbDivision,
						ParentOrg = dbParentOrg,
						FileCode = feedTeam.FileCode,
						FirstSeason = feedTeam.FirstYearOfPlay,
						IsActive = feedTeam.Active,
						IsAllStar = feedTeam.IsAllStarTeam,
						TeamAbbr = feedTeam.Abbreviation,
						TeamCode = feedTeam.TeamCode,
						TeamFullName = feedTeam.Name,
						TeamLocation = feedTeam.LocationName,
						TeamName = feedTeam.Name,
					};
					dbTeams.Add(dbTeam.TeamID, dbTeam);
					context.Teams.Add(dbTeam);
				}
				else
				{
					if (dbTeam.FileCode != feedTeam.FileCode ||
						dbTeam.FirstSeason != feedTeam.FirstYearOfPlay ||
						dbTeam.IsActive != feedTeam.Active ||
						dbTeam.IsAllStar != feedTeam.IsAllStarTeam ||
						dbTeam.TeamAbbr != feedTeam.Abbreviation ||
						dbTeam.TeamCode != feedTeam.TeamCode ||
						dbTeam.TeamFullName != feedTeam.Name ||
						dbTeam.TeamLocation != feedTeam.LocationName ||
						dbTeam.TeamName != feedTeam.Name)
					{

						dbTeam.FileCode = feedTeam.FileCode;
						dbTeam.FirstSeason = feedTeam.FirstYearOfPlay;
						dbTeam.IsActive = feedTeam.Active;
						dbTeam.IsAllStar = feedTeam.IsAllStarTeam;
						dbTeam.TeamAbbr = feedTeam.Abbreviation;
						dbTeam.TeamCode = feedTeam.TeamCode;
						dbTeam.TeamFullName = feedTeam.Name;
						dbTeam.TeamLocation = feedTeam.LocationName;
						dbTeam.TeamName = feedTeam.Name;
					}

					if (dbTeam.AssociationID != dbAssociation.AssociationID)
					{
						dbTeam.Association = dbAssociation;
					}

					if (dbLeague == null)
					{
						if (dbTeam.LeagueID.HasValue)
						{
							dbTeam.LeagueID = null;
						}
					}
					else if (dbTeam.LeagueID != dbLeague.LeagueID)
					{
						dbTeam.League = dbLeague;
					}

					if (dbDivision == null)
					{
						if (dbTeam.DivisionID.HasValue)
						{
							dbTeam.DivisionID = null;
						}
					}
					else if (dbTeam.DivisionID != dbDivision.DivisionID)
					{
						dbTeam.Division = dbDivision;
					}

					if (dbVenue == null)
					{
						if (dbTeam.VenueID.HasValue)
						{
							dbTeam.VenueID = null;
						}
					}
					else if (dbTeam.VenueID != dbVenue.VenueID)
					{
						dbTeam.Venue = dbVenue;
					}

					if (dbSpringLeague == null)
					{
						if (dbTeam.SpringLeagueID.HasValue)
						{
							dbTeam.SpringLeagueID = null;
						}
					}
					else if (dbTeam.SpringLeagueID != dbSpringLeague.LeagueID)
					{
						dbTeam.SpringLeague = dbSpringLeague;
					}
				}
			}
			context.SaveChanges();
		}
	}
}
