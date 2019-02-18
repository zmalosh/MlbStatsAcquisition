using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor
{
	public static class JsonUtility
	{
		public static string GetRawJsonFromUrl(string url)
		{
			string rawJson = null;
			using (var client = new WebClient())
			{
				url = Feeds.AssociationsFeed.GetFeedUrl();
				rawJson = client.DownloadString(url);
			}
			return rawJson;
		}
	}
}
