using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class LeagueSeason
	{
		public int LeagueID { get; set; }
		public int Season { get; set; }
		public string LeagueName { get; set; }
		public int? AssociationID { get; set; }

		public virtual League League { get; set; }
		public virtual AssociationSeason AssociationSeason { get; set; }
		public ICollection<DivisionSeason> DivisionSeasons { get; set; }
		public virtual ICollection<TeamSeason> TeamSeasons { get; set; }
		public virtual ICollection<TeamSeason> SpringTeamSeasons { get; set; }
	}
}
