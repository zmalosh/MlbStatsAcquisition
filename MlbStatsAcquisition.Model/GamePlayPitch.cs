using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class GamePlayPitch : MlbStatsEntity
	{
		public int GamePlayID { get; set; }
		public byte GamePlayEventIndex { get; set; }
		public byte PitchNumber { get; set; }
		public int PitcherID { get; set; }
		public int BatterID { get; set; }
		public byte Balls { get; set; }
		public byte Strikes { get; set; }
		public byte Outs { get; set; }
		public byte PitchResultTypeID { get; set; }
		public byte PitchTypeID { get; set; }
		public bool IsInPlay { get; set; }
		public bool IsStrike { get; set; }
		public bool IsBall { get; set; }
		public bool HasReview { get; set; }
		public short? TimeSec { get; set; }
		public double? StartSpeed { get; set; }
		public double? EndSpeed { get; set; }
		public double? NastyFactor { get; set; }
		public double? StrikeZoneTop { get; set; }
		public double? StrikeZoneBottom { get; set; }
		public double? P_A_X { get; set; }
		public double? P_A_Y { get; set; }
		public double? P_A_Z { get; set; }
		public double? P_PFX_X { get; set; }
		public double? P_PFX_Z { get; set; }
		public double? P_P_X { get; set; }
		public double? P_P_Z { get; set; }
		public double? P_V_X0 { get; set; }
		public double? P_V_Y0 { get; set; }
		public double? P_V_Z0 { get; set; }
		public double? P_X { get; set; }
		public double? P_Y { get; set; }
		public double? P_X0 { get; set; }
		public double? P_Y0 { get; set; }
		public double? P_Z0 { get; set; }
		public double? P_BreakAngle { get; set; }
		public double? P_BreakLength { get; set; }
		public double? P_Break_y { get; set; }
		public double? P_SpinRate { get; set; }
		public double? P_SpinDirection { get; set; }
		public byte? P_TypeConfidence { get; set; }
		public byte? P_Zone { get; set; }
		public double? H_LaunchSpeed { get; set; }
		public double? H_LaunchAngle { get; set; }
		public double? H_TotalDistance { get; set; }
		public byte? H_TrajectoryTypeID { get; set; }
		public string H_Hardness { get; set; }
		public byte H_Location { get; set; }
		public double? H_Coord_X { get; set; }
		public double? H_Coord_Y { get; set; }
		public Guid MlbPlayID { get; set; }

		public virtual GamePlay GamePlay { get; set; }
		public virtual Player Batter { get; set; }
		public virtual Player Pitcher { get; set; }
	}
}
