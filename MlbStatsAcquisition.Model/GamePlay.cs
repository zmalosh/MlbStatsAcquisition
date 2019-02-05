using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class GamePlay
	{
		public int GamePlayID { get; set; }
		public int GameID { get; set; }
		public int GameAtBatIndex { get; set; }

		public virtual Game Game { get; set; }
	}
}
