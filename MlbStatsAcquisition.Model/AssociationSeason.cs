using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class AssociationSeason : MlbStatsEntity
	{
		public int AssociationID { get; set; }
		public int Season { get; set; }
		public string AssociationName { get; set; }

		public virtual Association Association { get; set; }
		public virtual ICollection<LeagueSeason> LeagueSeasons { get; set; }
		public virtual ICollection<TeamSeason> TeamSeasons { get; set; }
		public virtual ICollection<Game> Games { get; set; }
	}
}
