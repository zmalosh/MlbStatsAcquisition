using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class Venue : MlbStatsEntity
	{
		public int VenueID { get; set; }
		public string VenueName { get; set; }
		public string VenueLink { get; set; }

		public virtual ICollection<Team> Teams { get; set; }
		public virtual ICollection<VenueSeason> VenueSeasons { get; set; }
		public virtual ICollection<Game> Games { get; set; }
	}
}
