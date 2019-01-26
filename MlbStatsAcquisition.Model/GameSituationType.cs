using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class GameSituationType
	{
		public int GameSituationTypeID { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public bool IsTeam { get; set; }
		public bool IsBatting { get; set; }
		public bool IsFielding { get; set; }
		public bool IsPitching { get; set; }
		public int? SortOrder { get; set; }
		public string NavMenuGroup { get; set; }
	}
}
