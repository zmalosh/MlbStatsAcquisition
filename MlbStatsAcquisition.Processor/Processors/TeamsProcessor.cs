using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class TeamsProcessor : IProcessor
	{
		private int Season { get; set; }
		private bool IsCurrentSeason { get; set; }

		public TeamsProcessor(int? season = null)
		{
			int currentSeason = DateTime.Now.Year;
			this.Season = season ?? currentSeason;
			this.IsCurrentSeason = this.Season == currentSeason;
		}

		public void Run(Model.MlbStatsContext context)
		{
			Console.WriteLine($"TeamsProcessor - {this.Season}");

			var url = Feeds.TeamsFeed.GetFeedUrl(this.Season);
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.TeamsFeed.FromJson(rawJson);

			var dbAssociations = context.Associations.ToDictionary(x => x.AssociationID);
			var dbLeagues = context.Leagues.ToDictionary(x => x.LeagueID);
			var dbDivisions = context.Divisions.ToDictionary(x => x.DivisionID);
			var dbVenues = context.Venues.ToDictionary(x => x.VenueID);
			var dbTeams = context.Teams.ToDictionary(x => x.TeamID);

			var dbVenueSeasons = context.VenueSeasons.Where(x => x.Season == this.Season).ToDictionary(x => x.VenueID);
			var dbAssociationSeasons = context.AssociationSeasons.Where(x => x.Season == this.Season).ToDictionary(y => y.AssociationID);
			var dbLeagueSeasons = context.LeagueSeasons.Where(x => x.Season == this.Season).ToDictionary(y => y.LeagueID);
			var dbDivisionSeasons = context.DivisionSeasons.Where(x => x.Season == this.Season).ToDictionary(y => y.DivisionID);
			var dbTeamSeasons = context.TeamSeasons.Where(x => x.Season == this.Season).ToDictionary(y => y.TeamID);

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
						LeagueLink = feedTeam.League.Link
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
						DivisionLink = feedTeam.Division.Link
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
						LeagueLink = feedTeam.SpringLeague.Link
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
				else if (this.IsCurrentSeason) // ONLY UPDATE TEAM ENTRY IF IT IS THE CURRENT SEASON
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
				}

				// ENTER SEASON BASED PROCESSING. CURRENT DATA CAN BE UPDATED ABOVE. SEASON BASED DATA SHOULD NEVER NEED TO BE UPDATED (FOR LEAGUE STRUCTURE)

				Model.VenueSeason dbVenueSeason = null;
				if (feedTeam.Venue?.Id != null && !dbVenueSeasons.TryGetValue(feedTeam.Venue.Id.Value, out dbVenueSeason))
				{
					dbVenueSeason = new Model.VenueSeason
					{
						VenueID = feedTeam.Venue.Id.Value,
						Season = this.Season,
						VenueName = feedTeam.Venue.Name
					};
					dbVenueSeasons.Add(feedTeam.Venue.Id.Value, dbVenueSeason);
					context.VenueSeasons.Add(dbVenueSeason);
				}

				if (!dbAssociationSeasons.TryGetValue(feedTeam.Sport.Id.Value, out Model.AssociationSeason dbAssociationSeason))
				{
					dbAssociationSeason = new Model.AssociationSeason
					{
						Association = dbAssociation,
						Season = this.Season,
						AssociationName = feedTeam.Sport.Name
					};
					dbAssociationSeasons.Add(dbAssociation.AssociationID, dbAssociationSeason);
					context.AssociationSeasons.Add(dbAssociationSeason);
				}

				Model.LeagueSeason dbLeagueSeason = null;
				if (feedTeam.League?.Id != null && !dbLeagueSeasons.TryGetValue(feedTeam.League.Id.Value, out dbLeagueSeason))
				{
					dbLeagueSeason = new Model.LeagueSeason
					{
						League = dbLeague,
						Season = this.Season,
						LeagueName = feedTeam.League.Name,
						AssociationSeason = dbAssociationSeason
					};
					dbLeagueSeasons.Add(dbLeague.LeagueID, dbLeagueSeason);
					context.LeagueSeasons.Add(dbLeagueSeason);
				}

				Model.LeagueSeason dbSpringLeagueSeason = null;
				if (feedTeam.SpringLeague?.Id != null && !dbLeagueSeasons.TryGetValue(feedTeam.SpringLeague.Id.Value, out dbSpringLeagueSeason))
				{
					dbSpringLeagueSeason = new Model.LeagueSeason
					{
						League = dbSpringLeague,
						Season = this.Season,
						LeagueName = feedTeam.SpringLeague.Name,
						AssociationSeason = dbAssociationSeason
					};
					dbLeagueSeasons.Add(dbSpringLeague.LeagueID, dbSpringLeagueSeason);
					context.LeagueSeasons.Add(dbSpringLeagueSeason);
				}

				Model.DivisionSeason dbDivisionSeason = null;
				if (feedTeam.Division?.Id != null && !dbDivisionSeasons.TryGetValue(feedTeam.Division.Id.Value, out dbDivisionSeason))
				{
					dbDivisionSeason = new Model.DivisionSeason
					{
						Division = dbDivision,
						Season = this.Season,
						DivisionName = feedTeam.Division.Name,
						LeagueSeason = dbLeagueSeason
					};
					dbDivisionSeasons.Add(dbDivision.DivisionID, dbDivisionSeason);
					context.DivisionSeasons.Add(dbDivisionSeason);
				}

				Model.TeamSeason dbTeamSeason = null;
				if (!dbTeamSeasons.TryGetValue(feedTeam.Id, out dbTeamSeason))
				{
					dbTeamSeason = new Model.TeamSeason
					{
						Team = dbTeam,
						Season = this.Season,
						AssociationSeason = dbAssociationSeason,
						LeagueSeason = dbLeagueSeason,
						DivisionSeason = dbDivisionSeason,
						VenueSeason = dbVenueSeason,
						FileCode = feedTeam.FileCode,
						FullName = feedTeam.Name,
						SpringLeagueSeason = dbSpringLeagueSeason,
						TeamAbbr = feedTeam.Abbreviation,
						TeamCode = feedTeam.TeamCode,
						TeamLocation = feedTeam.LocationName,
						TeamName = feedTeam.TeamName
					};
					dbTeamSeasons.Add(dbTeam.TeamID, dbTeamSeason);
					context.TeamSeasons.Add(dbTeamSeason);
				}
			}

			var feedChildTeams = feed.Teams.Where(x => x.ParentOrgId.HasValue);
			foreach (var feedChildTeam in feedChildTeams)
			{
				var dbChildTeamSeason = dbTeamSeasons[feedChildTeam.Id];
				if (!dbTeamSeasons.TryGetValue(feedChildTeam.ParentOrgId.Value, out Model.TeamSeason dbParentTeamSeason))
				{
					dbParentTeamSeason = new Model.TeamSeason
					{
						TeamID = feedChildTeam.ParentOrgId.Value,
						Season = this.Season,
						TeamName = feedChildTeam.ParentOrgName
					};
					dbTeamSeasons.Add(feedChildTeam.ParentOrgId.Value, dbParentTeamSeason);
					context.TeamSeasons.Add(dbParentTeamSeason);
				}
				dbChildTeamSeason.ParentOrgSeason = dbParentTeamSeason;
			}

			context.SaveChanges();
		}
	}
}
