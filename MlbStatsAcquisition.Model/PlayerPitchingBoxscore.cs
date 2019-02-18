using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class PlayerPitchingBoxscore : MlbStatsEntity
	{
		public int GameID { get; set; }
		public int PlayerID { get; set; }
		public int TeamID { get; set; }
		public int Season { get; set; }
		public bool GamePitched { get; set; }
		public bool? IsStart { get; set; }
		public bool? IsWin { get; set; }
		public bool? IsLoss { get; set; }
		public bool? IsSave { get; set; }
		public bool? IsSaveOpp { get; set; }
		public bool? IsHold { get; set; }
		public bool? IsBlownSave { get; set; }
		public bool? IsCompleteGame { get; set; }
		public bool? IsShutout { get; set; }
		public bool? IsFinish { get; set; }
		public byte? Hits { get; set; }
		public byte? AtBats { get; set; }
		public byte? BattersFaced { get; set; }
		public byte? Runs { get; set; }
		public byte? EarnedRuns { get; set; }
		public byte? StrikeOuts { get; set; }
		public byte? BaseOnBalls { get; set; }
		public byte? PitchCount { get; set; }
		public byte? Balls { get; set; }
		public byte? Strikes { get; set; }
		public byte? Outs { get; set; }
		public byte? GroundOuts { get; set; }
		public byte? Doubles { get; set; }
		public byte? Triples { get; set; }
		public byte? HomeRuns { get; set; }
		public byte? IntentionalWalks { get; set; }
		public byte? CaughtStealing { get; set; }
		public byte? StolenBases { get; set; }
		public byte? BattersHit { get; set; }
		public byte? WildPitches { get; set; }
		public byte? Pickoffs { get; set; }
		public byte? AirOuts { get; set; }
		public byte? RunsBattedIn { get; set; }
		public byte? InheritedRunners { get; set; }
		public byte? InheritedRunnersScored { get; set; }
		public byte? CathersInterference { get; set; }
		public byte? SacBunts { get; set; }
		public byte? SacFlies { get; set; }
		public byte? FlyOuts { get; set; }
		public string InningsPitched { get; set; }

		public virtual Game Game { get; set; }
		public virtual PlayerTeamSeason PlayerTeamSeason { get; set; }
	}
}
