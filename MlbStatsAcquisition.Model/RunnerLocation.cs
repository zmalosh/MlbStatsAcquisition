using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	[Flags]
	public enum RunnerLocation
	{
		None = 0, // Home_Start
		First = 1,
		Second = 2,
		Third = 4,
		Home_End = 8
	}
}
