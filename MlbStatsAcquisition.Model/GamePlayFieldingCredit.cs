using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class GamePlayFieldingCredit
	{
		public int FieldingCreditID { get; set; }
		public int PlayRunnerID { get; set; }
		public GamePlayFieldingCreditType CreditType { get; set; }
		public int FielderID { get; set; }
		public string PosAbbr { get; set; }

		public virtual GamePlayRunner PlayRunner { get; set; }
		public virtual Player Fielder { get; set; }
		public virtual RefGamePlayFieldingCreditType RefCreditType { get; set; }
	}
}
