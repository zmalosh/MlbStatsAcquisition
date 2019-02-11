using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class UmpireAssignment
	{
		public int UmpireID { get; set; }
		public int GameID { get; set; }
		public UmpireType UmpireType { get; set; }

		public virtual Umpire Umpire { get; set; }
		public virtual Game Game { get; set; }
		public virtual RefUmpireType RefUmpireType { get; set; }
	}
}
