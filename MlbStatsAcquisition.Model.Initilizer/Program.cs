using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model.Initilizer
{
	class Program
	{
		static void Main(string[] args)
		{
			var context = GetNewContext();

			Processor.Processors.IProcessor processor;
			List<Processor.Processors.IProcessor> processors;

			if (!context.Database.Exists())
			{
				var init = new MlbStatsContextInitilizer();
				init.InitializeDatabase(context);

				processors = new List<Processor.Processors.IProcessor>
					{
						new Processor.Processors.AssociationsProcessor(),
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
						new Processor.Processors.WindTypesProcessor(),
						new Processor.Processors.StandingsTypesProcessor()
					};

				foreach (var p in processors)
				{
					p.Run(context);
				}

				processors = new List<Processor.Processors.IProcessor>();
				for (int i = 2018; i >= 2012; i--)
				{
					processors.Add(new Processor.Processors.TeamsProcessor(i));
				}
				foreach (var p in processors)
				{
					p.Run(context);
				}
			}

			var associationIds = context.Associations.Where(x => x.IsEnabled).Select(x => x.AssociationID).ToList();

			//ITERATE OVER CALENDAR YEARS, NOT SEASONS
			for (int year = 2018; year >= 2015; year--)
			{
				processor = new Processor.Processors.GameScheduleProcessor(year, associationIds);
				processor.Run(context);
				context = GetNewContext();

				processors = context.Games.Where(x => x.GameTime.Year == year)
											.ToList()
											.Select(x => (Processor.Processors.IProcessor)new Processor.Processors.BoxscoreProcessor(x.GameID))
											.ToList();
				processors.ForEach(x =>
				{
					x.Run(context);
					context = GetNewContext();
				});
			}

			context.Dispose();
		}

		private static MlbStatsContext GetNewContext()
		{
			var context = new MlbStatsContext();
			context.Configuration.AutoDetectChangesEnabled = false;
			return context;
		}
	}
}
