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

			var venuesProcessor = new Processor.Processors.VenuesProcessor();
			venuesProcessor.Run();

			var statTypesProcessor = new Processor.Processors.StatTypesProcessor();
			statTypesProcessor.Run();

			var positionsProcessor = new Processor.Processors.PositionsProcessor();
			positionsProcessor.Run();

			var gameEventTypesProcessor = new Processor.Processors.GameEventTypesProcessor();
			gameEventTypesProcessor.Run();

			var gameStatusTypesProcessor = new Processor.Processors.GameStatusTypesProcessor();
			gameStatusTypesProcessor.Run();

			var gameTypesProcessor = new Processor.Processors.GameTypesProcessor();
			gameTypesProcessor.Run();

			var hitTrajectoryTypesProcessor = new Processor.Processors.HitTrajectoryTypesProcessor();
			hitTrajectoryTypesProcessor.Run();

			var jobTypesProcessor = new Processor.Processors.JobTypesProcessor();
			jobTypesProcessor.Run();
		}
	}
}
