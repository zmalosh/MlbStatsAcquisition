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
		public string AssociationAbbr { get; set; }
		public bool IsEnabled { get; set; }
		public string AssociationCode { get; set; }
		public string AssociationLink { get; set; }

		public virtual ICollection<League> Leagues { get; set; }
		public virtual ICollection<Team> Teams { get; set; }
		public virtual ICollection<AssociationSeason> AssociationSeasons { get; set; }
		public virtual ICollection<Game> Games { get; set; }
	}
}
