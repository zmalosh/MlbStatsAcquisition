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
		private const string LocalPath = "C:\\FileCache\\MlbStatsAcquisition\\";
		private const string WebBaseUrl = "http://statsapi.mlb.com/api/v1/";

		public static string GetRawJsonFromUrl(string url)
		{
			string rawJson = null;
			var relativePath = url.Replace(WebBaseUrl, string.Empty).Replace("/", "_").Replace("?", "_").Replace("=", "_").Replace("&", "_");
			var fullLocalPath = string.Concat(LocalPath, relativePath, ".json");
			if (System.IO.File.Exists(fullLocalPath))
			{
				rawJson = System.IO.File.ReadAllText(fullLocalPath);
			}
			else
			{
				using (var client = new WebClient())
				{
					try
					{
						rawJson = client.DownloadString(url);
					}
					catch (WebException ex)
					{
						try
						{
							rawJson = client.DownloadString(url);
						}
						catch (Exception ex2)
						{
							string failDir = "FailedGameIDs";
							if (!System.IO.Directory.Exists(failDir))
							{
								System.IO.Directory.CreateDirectory(failDir);
							}
							var filePath = string.Format($"{failDir}\\{relativePath}.nobueno");
							System.IO.File.Create(filePath);
							return null;
						}
					}
					rawJson = client.DownloadString(url);
					var dir = System.IO.Path.GetDirectoryName(fullLocalPath);
					if (!System.IO.Directory.Exists(dir))
					{
						System.IO.Directory.CreateDirectory(dir);
					}
					System.IO.File.WriteAllText(fullLocalPath, rawJson);
				}
			}
			return rawJson;
		}
	}
}
