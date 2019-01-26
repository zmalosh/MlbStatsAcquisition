using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class GameSituationTypesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			List<Feeds.GameSituationTypesFeed> feed;
			using (var client = new WebClient())
			{
				var url = Feeds.GameSituationTypesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.GameSituationTypesFeed.FromJson(rawJson);
			}

			var dbGameSituationTypes = context.GameSituationTypes.ToDictionary(x => x.Code);
			foreach (var feedGameSituationType in feed)
			{
				if (!dbGameSituationTypes.TryGetValue(feedGameSituationType.Code, out Model.GameSituationType dbGameSituationType))
				{
					dbGameSituationType = new Model.GameSituationType
					{
						Code = feedGameSituationType.Code,
						Description = feedGameSituationType.Description,
						IsBatting = feedGameSituationType.Batting,
						IsFielding = feedGameSituationType.Fielding,
						IsPitching = feedGameSituationType.Pitching,
						IsTeam = feedGameSituationType.Team,
						NavMenuGroup = feedGameSituationType.NavigationMenu,
						SortOrder = feedGameSituationType.SortOrder
					};
					dbGameSituationTypes.Add(dbGameSituationType.Code, dbGameSituationType);
					context.GameSituationTypes.Add(dbGameSituationType);
				}
				else
				{
					if (dbGameSituationType.Description != feedGameSituationType.Description
						|| dbGameSituationType.IsBatting != feedGameSituationType.Batting
						|| dbGameSituationType.IsFielding != feedGameSituationType.Fielding
						|| dbGameSituationType.IsPitching != feedGameSituationType.Pitching
						|| dbGameSituationType.IsTeam != feedGameSituationType.Team
						|| dbGameSituationType.NavMenuGroup != feedGameSituationType.NavigationMenu
						|| dbGameSituationType.SortOrder != feedGameSituationType.SortOrder)
					{
						dbGameSituationType.Code = feedGameSituationType.Code;
						dbGameSituationType.Description = feedGameSituationType.Description;
						dbGameSituationType.IsBatting = feedGameSituationType.Batting;
						dbGameSituationType.IsFielding = feedGameSituationType.Fielding;
						dbGameSituationType.IsPitching = feedGameSituationType.Pitching;
						dbGameSituationType.IsTeam = feedGameSituationType.Team;
						dbGameSituationType.NavMenuGroup = feedGameSituationType.NavigationMenu;
						dbGameSituationType.SortOrder = feedGameSituationType.SortOrder;
					}
				}
			}
			context.SaveChanges();
		}
	}
}
