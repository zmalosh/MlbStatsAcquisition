using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class DivisionSeason : MlbStatsEntity
	{
		public int DivisionID { get; set; }
		public int Season { get; set; }
		public string DivisionName { get; set; }
		public int LeagueID { get; set; }

		public virtual Division Division { get; set; }
		public virtual LeagueSeason LeagueSeason { get; set; }
		public virtual ICollection<TeamSeason> TeamSeasons { get; set; }
	}
}
