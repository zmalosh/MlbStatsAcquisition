using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class GamePlayRunner
	{
		public int GamePlayRunnerID { get; set; }
		public int GamePlayID { get; set; }
		public int RunnerID { get; set; }
		public RunnerLocation StartRunnerLocation { get; set; }
		public RunnerLocation? EndRunnerLocation { get; set; }
		public int? PitcherResponsibleID { get; set; }
		public string PlayEvent { get; set; }
		public string PlayEventType { get; set; }
		public short PlayIndex { get; set; }
		public string MovementReason { get; set; }
		public bool IsScore { get; set; }
		public bool? IsEarned { get; set; }
		public bool? IsTeamUnearned { get; set; }
		public bool? IsOut { get; set; }
		public byte? OutNumber { get; set; }
		public RunnerLocation? OutLocation { get; set; }
		public int? FielderID { get; set; }
		public int? PutOuterID { get; set; }
		public int? AssisterID { get; set; }
		public int? ErrorPlayerID { get; set; }
		public string FielderPos { get; set; }
		public string PutOuterPos { get; set; }
		public string AssisterPos { get; set; }
		public string ErrorPlayerPos { get; set; }

		public virtual GamePlay GamePlay { get; set; }
		public virtual Player Runner { get; set; }
		public virtual Player PitcherResponsible { get; set; }
		public virtual ICollection<GamePlayFieldingCredit> FieldingCredits { get; set; }
	}
}
