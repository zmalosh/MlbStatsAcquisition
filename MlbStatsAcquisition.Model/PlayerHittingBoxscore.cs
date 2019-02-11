using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class PlayerHittingBoxscore : MlbStatsEntity
	{
		public int GameID { get; set; }
		public int PlayerID { get; set; }
		public int TeamID { get; set; }
		public int Season { get; set; }

		public virtual Game Game { get; set; }
		public virtual PlayerTeamSeason PlayerTeamSeason { get; set; }
	}
}
