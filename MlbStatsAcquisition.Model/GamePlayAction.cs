using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class GamePlayAction : MlbStatsEntity
	{
		public int GamePlayID { get; set; }
		public byte GamePlayEventIndex { get; set; }
		public string Description { get; set; }
		public string EventName { get; set; }
		public bool IsScore { get; set; }
		public bool IsPitch { get; set; }
		public bool HasReview { get; set; }
		public int? ActionTakerID { get; set; }
		public short? SubOrder { get; set; }
		public short? TimeSec { get; set; }

		public virtual GamePlay GamePlay { get; set; }
		public virtual Player ActionTaker { get; set; }
	}
}
