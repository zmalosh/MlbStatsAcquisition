using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class League : MlbStatsEntity
	{
		public int LeagueID { get; set; }
		public string LeagueName { get; set; }
		public string LeagueLink { get; set; }

		public ICollection<LeagueSeason> LeagueSeasons { get; set; }
	}
}
