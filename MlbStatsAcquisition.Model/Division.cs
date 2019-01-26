using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class Division : MlbStatsEntity
	{
		public int DivisionID { get; set; }
		public string DivisionName { get; set; }
		public string DivisionLink { get; set; }
		public int LeagueID { get; set; }
		
		public virtual League League { get; set; }
		public ICollection<Team> Teams { get; set; }
	}
}
