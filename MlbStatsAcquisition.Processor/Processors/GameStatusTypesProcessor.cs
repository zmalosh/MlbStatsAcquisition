﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class GameStatusTypesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			var url = Feeds.GameStatusTypesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.GameStatusTypesFeed.FromJson(rawJson);

			var dbGameStatusTypes = context.GameStatusTypes.ToDictionary(x => x.GameStatusCode);
			foreach (var feedGameStatusType in feed)
			{
				if (!dbGameStatusTypes.TryGetValue(feedGameStatusType.StatusCode, out Model.GameStatusType dbGameStatusType))
				{
					dbGameStatusType = new Model.GameStatusType
					{
						GameStatusCode = feedGameStatusType.StatusCode,
						AbstractGameCode = feedGameStatusType.AbstractGameCode,
						AbstractGameState = feedGameStatusType.AbstractGameState,
						CodedGameState = feedGameStatusType.CodedGameState,
						DetailedState = feedGameStatusType.DetailedState,
						Reason = feedGameStatusType.Reason
					};
					dbGameStatusTypes.Add(dbGameStatusType.GameStatusCode, dbGameStatusType);
					context.GameStatusTypes.Add(dbGameStatusType);
				}
				else
				{
					; // TODO: VERIFY NO CHANGES TO PROPERTIES
				}
			}
			context.SaveChanges();
		}
	}
}
