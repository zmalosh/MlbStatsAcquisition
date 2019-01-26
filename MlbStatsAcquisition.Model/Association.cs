using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class Association : MlbStatsEntity
	{
		public int AssociationID { get; set; }
		public string AssociationName { get; set; }
		public string AssociationLink { get; set; }

		public ICollection<League> Leagues { get; set; }
		public ICollection<Team> Teams { get; set; }
	}
}
