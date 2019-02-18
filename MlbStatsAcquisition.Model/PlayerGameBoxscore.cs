using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class PlayerGameBoxscore : MlbStatsEntity
	{
		public int GameID { get; set; }
		public int PlayerID { get; set; }
		public int TeamID { get; set; }
		public int Season { get; set; }
		public int? JerseyNumber { get; set; }
		public string PosAbbr { get; set; }
		public string Status { get; set; }
		public bool? IsOnBench { get; set; }
		public bool? IsSub { get; set; }
		public string AllPositions { get; set; }
		public bool HasHitting { get; set; }
		public bool HasPitching { get; set; }
		public bool HasFielding { get; set; }


		public virtual Game Game { get; set; }
		public virtual PlayerTeamSeason PlayerTeamSeason { get; set; }
		public virtual Position Position { get; set; }
	}
}
