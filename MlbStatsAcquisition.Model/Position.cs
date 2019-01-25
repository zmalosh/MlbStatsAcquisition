using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class Position
	{
		public string PositionAbbr { get; set; }
		public string ShortName { get; set; }
		public string PositionType { get; set; }
		public string DisplayName { get; set; }
		public string FullName { get; set; }
		public string FormalName { get; set; }
		public bool IsPitcher { get; set; }
		public bool IsFielder { get; set; }
		public bool IsOutfield { get; set; }
		public string Code { get; set; }
	}
}
