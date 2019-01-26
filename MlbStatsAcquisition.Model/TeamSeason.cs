using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class TeamSeason : MlbStatsEntity
	{
		public int TeamID { get; set; }
		public int Season { get; set; }
		public string TeamName { get; set; }
		public string TeamLocation { get; set; }
		public string TeamAbbr { get; set; }
		public string TeamCode { get; set; }
		public int? AssociationID { get; set; }
		public int? LeagueID { get; set; }
		public int? DivisionID { get; set; }
		public int? SpringLeagueID { get; set; }
		public int? VenueID { get; set; }
		public int? ParentOrgID { get; set; }
		public string FileCode { get; set; }
		public string FullName { get; set; }

		public virtual Team Team { get; set; }
		public virtual AssociationSeason AssociationSeason { get; set; }
		public virtual LeagueSeason LeagueSeason { get; set; }
		public virtual DivisionSeason DivisionSeason { get; set; }
		public virtual LeagueSeason SpringLeagueSeason { get; set; }
		public virtual TeamSeason ParentOrgSeason { get; set; }
		public virtual VenueSeason VenueSeason { get; set; }
		public virtual ICollection<TeamSeason> ChildOrgSeasons { get; set; }
		public virtual ICollection<Game> AwayGames { get; set; }
		public virtual ICollection<Game> HomeGames { get; set; }
	}
}
