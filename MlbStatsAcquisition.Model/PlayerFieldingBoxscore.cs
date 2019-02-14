using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class PlayerFieldingBoxscore : MlbStatsEntity
	{
		public int GameID { get; set; }
		public int PlayerID { get; set; }
		public int TeamID { get; set; }
		public int Season { get; set; }
		public string PosAbbr { get; set; }
		public byte? Assists { get; set; }
		public byte? PutOuts { get; set; }
		public byte? Errors { get; set; }
		public byte? Chances { get; set; }
		public byte? CaughtStealing { get; set; }
		public byte? PassedBall { get; set; }
		public byte? StolenBases { get; set; }
		public byte? Pickoffs { get; set; }

		public virtual Game Game { get; set; }
		public virtual PlayerTeamSeason PlayerTeamSeason { get; set; }
		public virtual Position Position { get; set; }
	}
}
