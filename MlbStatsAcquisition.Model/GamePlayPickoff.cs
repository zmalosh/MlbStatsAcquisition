using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class GamePlayPickoff : MlbStatsEntity
	{
		public int GamePlayID { get; set; }
		public byte GamePlayEventIndex { get; set; }
		public byte GameEventTypeID { get; set; }
		public bool IsFromCatcher { get; set; }
		public bool HasReview { get; set; }
		public Guid? MlbPlayID { get; set; }

		public virtual GamePlay GamePlay { get; set; }
	}
}
