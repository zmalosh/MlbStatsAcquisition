using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class VenueSeason
	{
		public int VenueID { get; set; }
		public int Season { get; set; }
		public string VenueName { get; set; }

		public virtual Venue Venue { get; set; }
		public virtual ICollection<TeamSeason> TeamSeasons { get; set; }
	}
}
