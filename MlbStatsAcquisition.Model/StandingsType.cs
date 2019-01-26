using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class StandingsType : MlbStatsEntity
	{
		public int StandingsTypeID { get; set; }
		public string StandingsTypeName { get; set; }
		public string Description { get; set; }
	}
}
