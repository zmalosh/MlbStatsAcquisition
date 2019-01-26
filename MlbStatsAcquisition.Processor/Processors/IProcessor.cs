using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public interface IProcessor
	{
		void Run(Model.MlbStatsContext context);
	}
}
