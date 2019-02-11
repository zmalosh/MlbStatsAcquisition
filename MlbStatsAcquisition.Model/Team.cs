using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class Team : MlbStatsEntity
	{
		public int TeamID { get; set; }
		public string TeamAbbr { get; set; }
		public string TeamName { get; set; }
		public string TeamLocation { get; set; }
		public bool IsActive { get; set; }
		public int? FirstSeason { get; set; }
		public bool IsAllStar { get; set; }
		public string TeamFullName { get; set; }
		public string TeamCode { get; set; }
		public string FileCode { get; set; }

		public virtual ICollection<TeamSeason> TeamSeasons { get; set; }
	}
}
