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
		public bool GamePlayed { get; set; }
		public byte? FlyOuts { get; set; }
		public byte? GroundOuts { get; set; }
		public byte? Runs { get; set; }
		public byte? Doubles { get; set; }
		public byte? Triples { get; set; }
		public byte? HomeRuns { get; set; }
		public byte? StrikeOuts { get; set; }
		public byte? Walks { get; set; }
		public byte? IntentionalWalks { get; set; }
		public byte? Hits { get; set; }
		public byte? HitByPitches { get; set; }
		public byte? AtBats { get; set; }
		public byte? CaughtStealing { get; set; }
		public byte? StolenBases { get; set; }
		public byte? GroundIntoDoublePlay { get; set; }
		public byte? GroundIntoTriplePlay { get; set; }
		public byte? TotalBases { get; set; }
		public byte? RunsBattedIn { get; set; }
		public byte? RunnersLeftOnBase { get; set; }
		public byte? SacBunts { get; set; }
		public byte? SacFlies { get; set; }
		public byte? CatcherInterferences { get; set; }
		public byte? Pickoffs { get; set; }

		public virtual Game Game { get; set; }
		public virtual PlayerTeamSeason PlayerTeamSeason { get; set; }
	}
}
