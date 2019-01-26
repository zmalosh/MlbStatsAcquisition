using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class WindType : MlbStatsEntity
	{
		public int WindTypeID { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
	}
}
