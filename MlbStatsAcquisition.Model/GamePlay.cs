using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class GamePlay : MlbStatsEntity
	{
		public int GamePlayID { get; set; }
		public int GameID { get; set; }
		public short GamePlayIndex { get; set; }
		public byte Inning { get; set; }
		public int Season { get; set; }
		public string PlayType { get; set; }
		public string PlayEventType { get; set; }
		public string PlayEvent { get; set; }
		public bool IsInningTop { get; set; }
		public byte OutsEnd { get; set; }
		public byte OutsStart { get; set; }
		public bool IsReview { get; set; }
		public byte Strikes { get; set; }
		public byte Balls { get; set; }
		public byte RunsScored { get; set; }
		public byte ScoreHome { get; set; }
		public byte ScoreAway { get; set; }
		public int BatterID { get; set; }
		public int PitcherID { get; set; }
		public char BatterHand { get; set; }
		public char PitcherHand { get; set; }
		public string GamePlayDescription { get; set; }
		public string BatterSplit { get; set; }
		public string PitcherSplit { get; set; }
		public RunnerLocation RunnerStatusStart { get; set; }
		public RunnerLocation RunnerStatusEnd { get; set; }

		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }

		public virtual Game Game { get; set; }
		public virtual Player Batter { get; set; }
		public virtual Player Pitcher { get; set; }
		public virtual ICollection<GamePlayRunner> Runners { get; set; }
		public virtual ICollection<GamePlayPitch> Pitches { get; set; }
		public virtual ICollection<GamePlayAction> Actions { get; set; }
		public virtual ICollection<GamePlayPickoff> Pickoffs { get; set; }
	}
}
