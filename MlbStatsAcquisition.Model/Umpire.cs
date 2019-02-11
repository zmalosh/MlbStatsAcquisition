using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class Umpire
	{
		public int UmpireID { get; set; }
		public string UmpireName { get; set; }
		public string UmpireLink { get; set; }

		public virtual ICollection<UmpireAssignment> UmpireAssignments { get; set; }
	}
}
