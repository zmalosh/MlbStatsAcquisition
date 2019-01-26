using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class Team : MlbStatsEntity
	{
		public int TeamID { get; set; }
		public int? AssociationID { get; set; }
		public int? LeagueID { get; set; }
		public int? DivisionID { get; set; }
		public string TeamAbbr { get; set; }
		public string TeamName { get; set; }
		public string TeamLocation { get; set; }
		public bool IsActive { get; set; }
		public int? VenueID { get; set; }
		public int? SpringLeagueID { get; set; }
		public int? ParentOrgID { get; set; }
		public int? FirstSeason { get; set; }
		public bool IsAllStar { get; set; }
		public string TeamFullName { get; set; }
		public string TeamCode { get; set; }
		public string FileCode { get; set; }

		public virtual Association Association { get; set; }
		public virtual League League { get; set; }
		public virtual Division Division { get; set; }
		public virtual Venue Venue { get; set; }
		public virtual League SpringLeague { get; set; }
		public virtual Team ParentOrg { get; set; }
		public virtual ICollection<Team> ChildOrgTeams { get; set; }
		public virtual ICollection<TeamSeason> TeamSeasons { get; set; }
	}
}
