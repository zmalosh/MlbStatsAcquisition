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

		public virtual ICollection<DivisionSeason> DivisionSeasons { get; set; }
	}
}
