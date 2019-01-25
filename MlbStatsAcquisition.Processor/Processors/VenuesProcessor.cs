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
		public void Run()
		{
			Feeds.VenuesFeed feed;
			using (var client = new WebClient())
			{
				var url = Feeds.VenuesFeed.GetFeedUrl();
				var rawJson = client.DownloadString(url);
				feed = Feeds.VenuesFeed.FromJson(rawJson);
			}
			using (var context = new Model.MlbStatsContext())
			{
				var dbVenues = context.Venues.ToDictionary(x => x.VenueId);
				foreach (var feedVenue in feed.Venues)
				{
					if (!dbVenues.TryGetValue(feedVenue.Id, out Model.Venue dbVenue))
					{
						dbVenue = new Model.Venue
						{
							VenueId = feedVenue.Id,
							VenueName = feedVenue.Name,
							VenueLink = feedVenue.Link
						};
						dbVenues.Add(dbVenue.VenueId, dbVenue);
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
}
