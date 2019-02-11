using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public enum UmpireType
	{
		Unknown = 0,
		FirstBase = 1,
		SecondBase = 2,
		ThirdBase = 3,
		HomePlate = 4,
		LeftField = 5,
		RightField = 6
	}

	public class RefUmpireType
	{
		public UmpireType UmpireType { get; set; }
		public string Name { get; set; }

		public virtual ICollection<UmpireAssignment> UmpireAssignments { get; set; }
	}
}
