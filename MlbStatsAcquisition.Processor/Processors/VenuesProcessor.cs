using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Processors
{
	public class VenuesProcessor : IProcessor
	{
		public void Run(Model.MlbStatsContext context)
		{
			var url = Feeds.VenuesFeed.GetFeedUrl();
			var rawJson = JsonUtility.GetRawJsonFromUrl(url);
			var feed = Feeds.VenuesFeed.FromJson(rawJson);

			var dbVenues = context.Venues.ToDictionary(x => x.VenueID);
			foreach (var feedVenue in feed.Venues)
			{
				if (!dbVenues.TryGetValue(feedVenue.Id, out Model.Venue dbVenue))
				{
					dbVenue = new Model.Venue
					{
						VenueID = feedVenue.Id,
						VenueName = feedVenue.Name,
						VenueLink = feedVenue.Link
					};
					dbVenues.Add(dbVenue.VenueID, dbVenue);
					context.Venues.Add(dbVenue);
				}

				if (dbVenue.VenueName != feedVenue.Name)
				{
					dbVenue.VenueName = feedVenue.Name;
				}
			}
			context.SaveChanges();
		}
	}
}
