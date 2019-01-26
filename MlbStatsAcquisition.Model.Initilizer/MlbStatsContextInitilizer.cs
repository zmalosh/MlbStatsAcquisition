using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model.Initilizer
{
	public class MlbStatsContextInitilizer : DropCreateDatabaseAlways<MlbStatsContext>
	{
		public override void InitializeDatabase(MlbStatsContext context)
		{
			base.InitializeDatabase(context);

			var processors = new List<Processor.Processors.IProcessor>
			{
				 new Processor.Processors.VenuesProcessor(),
				 new Processor.Processors.StatTypesProcessor(),
				 new Processor.Processors.PositionsProcessor(),
				 new Processor.Processors.GameEventTypesProcessor(),
				 new Processor.Processors.GameStatusTypesProcessor(),
				 new Processor.Processors.GameTypesProcessor(),
				 new Processor.Processors.HitTrajectoryTypesProcessor(),
				 new Processor.Processors.JobTypesProcessor(),
				 new Processor.Processors.PitchResultTypesProcessor(),
				 new Processor.Processors.PitchTypesProcessor(),
				 new Processor.Processors.ReviewReasonTypesProcessor(),
				 new Processor.Processors.GameSituationTypesProcessor(),
				 new Processor.Processors.SkyTypesProcessor(),
				 new Processor.Processors.WindTypesProcessor()
			};

			foreach (var processor in processors)
			{
				processor.Run();
			}
		}
	}
}
