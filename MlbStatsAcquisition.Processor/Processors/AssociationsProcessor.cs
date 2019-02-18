using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class AssociationsProcessor : IProcessor
	{
		public static readonly List<int> readonlyDisabledAssociationIds = new List<int>
		{
			5248, // NBA
			5150, // NHL
			5300, // INTL HOCKEY
			5302, // INTL BASKETBALL
		};

		public void Run(Model.MlbStatsContext context)
		{
			var url = Feeds.AssociationsFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.AssociationsFeed.FromJson(rawJson);

			var dbAssociations = context.Associations.ToDictionary(x => x.AssociationID);
			foreach (var feedAssociation in feed.Associations)
			{
				if (!dbAssociations.TryGetValue(feedAssociation.Id, out Model.Association dbAssociation))
				{
					dbAssociation = new Model.Association
					{
						AssociationID = feedAssociation.Id,
						AssociationName = feedAssociation.Name,
						AssociationLink = feedAssociation.Link,
						AssociationAbbr = feedAssociation.Abbreviation,
						AssociationCode = feedAssociation.Code,
						IsEnabled = !readonlyDisabledAssociationIds.Contains(feedAssociation.Id)
					};
					dbAssociations.Add(feedAssociation.Id, dbAssociation);
					context.Associations.Add(dbAssociation);
				}
				else
				{
					if (!string.Equals(dbAssociation.AssociationName, feedAssociation.Name))
					{
						dbAssociation.AssociationName = feedAssociation.Name;
					}
					if (!string.Equals(dbAssociation.AssociationAbbr, feedAssociation.Abbreviation))
					{
						dbAssociation.AssociationAbbr = feedAssociation.Abbreviation;
					}
					if (!string.Equals(dbAssociation.AssociationCode, feedAssociation.Code))
					{
						dbAssociation.AssociationCode = feedAssociation.Code;
					}
				}
			}
			context.SaveChanges();
		}
	}
}
