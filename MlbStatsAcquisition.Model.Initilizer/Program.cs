using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model.Initilizer
{
	class Program
	{
		private const int MaxYear = 2018;
		private const int MinYear = 2017;
		static void Main(string[] args)
		{
			var startTime = DateTime.Now;
			var context = GetNewContext();

			Processor.Processors.IProcessor processor;
			List<Processor.Processors.IProcessor> processors;

			if (!context.Database.Exists())
			{
				var init = new MlbStatsContextInitilizer();
				init.InitializeDatabase(context);
			}

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
			for (int i = MaxYear; i >= MinYear; i--)
			{
				processors.Add(new Processor.Processors.TeamsProcessor(i));
			}
			foreach (var p in processors)
			{
				p.Run(context);
			}

			var associationIds = context.Associations.Where(x => x.IsEnabled).Select(x => x.AssociationID).ToList();

			//ITERATE OVER CALENDAR YEARS, NOT SEASONS
			for (int year = MaxYear; year >= MinYear; year--)
			{
				processor = new Processor.Processors.GameScheduleProcessor(year, associationIds);
				processor.Run(context);
				context = GetNewContext();

				var gameDatas = context.Games.Where(x => x.GameTime.Year == year)
											.OrderBy(x => x.GameTime)
											.Select(x => new { x.GameID, x.GameTime, Home = x.HomeTeamSeason != null ? x.HomeTeamSeason.TeamAbbr : "", Away = x.AwayTeamSeason != null ? x.AwayTeamSeason.TeamAbbr : "" })
											.ToList();

				foreach (var gameData in gameDatas)
				{
					Console.WriteLine($"{gameData.GameTime.ToShortDateString()} - {gameData.GameID} - {gameData.Away?.PadRight(3, ' ')} @ {gameData.Home?.PadRight(3, ' ')}");
					processor = new Processor.Processors.BoxscoreProcessor(gameData.GameID);
					processor.Run(context);
					context = GetNewContext();
				}
			}

			context.Dispose();
			var endTime = DateTime.Now;
			Console.WriteLine($"FIN - {(endTime - startTime).TotalMinutes} MIN");
			Console.ReadKey();
		}

		private static MlbStatsContext GetNewContext()
		{
			var context = new MlbStatsContext();
			context.Configuration.AutoDetectChangesEnabled = false;
			return context;
		}
	}
}
