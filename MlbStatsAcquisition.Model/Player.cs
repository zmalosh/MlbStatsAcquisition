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
		public string FullName { get; set; }
		public string PlayerLink { get; set; }

		public virtual ICollection<PlayerTeamSeason> PlayerTeamSeasons { get; set; }
		public virtual ICollection<GamePlay> BatterPlays { get; set; }
		public virtual ICollection<GamePlay> PitcherPlays { get; set; }
		public virtual ICollection<GamePlayRunner> PlaysOnBase { get; set; }
		public virtual ICollection<GamePlayRunner> PitcherResponsibleRunners { get; set; }
		public virtual ICollection<GamePlayFieldingCredit> FieldingCredits { get; set; }
		public virtual ICollection<GamePlayPitch> PitchesThrown { get; set; }
		public virtual ICollection<GamePlayPitch> PitchesFaced { get; set; }
		public virtual ICollection<GamePlayAction> ActionsTaken { get; set; }
	}
}
