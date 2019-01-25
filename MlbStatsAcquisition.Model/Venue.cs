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
		public int VenueId { get; set; }
		public string VenueName { get; set; }
		public string VenueLink { get; set; }
	}
}
