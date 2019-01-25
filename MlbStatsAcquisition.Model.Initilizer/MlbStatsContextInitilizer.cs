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
		}
	}
}
