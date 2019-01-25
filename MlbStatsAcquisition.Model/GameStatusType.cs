using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class GameStatusType
	{
		public int GameStatusTypeID { get; set; }
		public string GameStatusCode { get; set; }
		public string AbstractGameCode { get; set; }
		public string CodedGameState { get; set; }
		public string AbstractGameState { get; set; }
		public string DetailedState { get; set; }
		public string Reason { get; set; }
	}
}
