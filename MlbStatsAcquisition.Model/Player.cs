using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class Player : MlbStatsEntity
	{
		public int PlayerID { get; set; }

		public virtual ICollection<PlayerTeamSeason> PlayerSeasons { get; set; }
	}
}
