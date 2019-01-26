using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class GameScheduleFeed
	{
		public static string GetFeedUrl(int season, int associationId)
		{
			return string.Format($"http://statsapi.mlb.com/api/v1/schedule?startDate=01/01/{season}&endDate=12/31/{season}&sportId={associationId}");
		}

		public static GameScheduleFeed FromJson(string json) => JsonConvert.DeserializeObject<GameScheduleFeed>(json, Converter.Settings);

		public int TotalItems { get; set; }

		[JsonProperty("totalEvents")]
		public int TotalEvents { get; set; }

		[JsonProperty("totalGames")]
		public int TotalGames { get; set; }

		[JsonProperty("totalGamesInProgress")]
		public int TotalGamesInProgress { get; set; }

		[JsonProperty("dates")]
		public List<FeedDate> Dates { get; set; }

		public partial class FeedDate
		{
			[JsonProperty("date")]
			public DateTimeOffset DateDate { get; set; }

			[JsonProperty("totalItems")]
			public int TotalItems { get; set; }

			[JsonProperty("totalEvents")]
			public int TotalEvents { get; set; }

			[JsonProperty("totalGames")]
			public int TotalGames { get; set; }

			[JsonProperty("totalGamesInProgress")]
			public int TotalGamesInProgress { get; set; }

			[JsonProperty("games")]
			public List<FeedGame> Games { get; set; }
		}

		public class FeedGame
		{
			[JsonProperty("gamePk")]
			public int GamePk { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }

			[JsonProperty("gameType")]
			public string GameType { get; set; }

			[JsonIgnore]
			public int Season { get { return int.TryParse(this.RawSeason, out int itp) ? itp : int.TryParse(this.RawSeason.Split('.')[0], out itp) ? itp : this.GameDate.Year; } }

			[JsonProperty("season")]
			public string RawSeason { get; set; }

			[JsonProperty("gameDate")]
			public DateTime GameDate { get; set; }

			[JsonProperty("status")]
			public FeedGameStatus Status { get; set; }

			[JsonProperty("teams")]
			public FeedGameParticipants Teams { get; set; }

			[JsonProperty("venue")]
			public FeedVenue Venue { get; set; }

			[JsonProperty("isTie", NullValueHandling = NullValueHandling.Ignore)]
			public bool? IsTie { get; set; }

			[JsonProperty("gameNumber")]
			public int GameNumber { get; set; }

			[JsonIgnore]
			public bool IsDoubleHeader { get { return string.Equals(this.DoubleHeader, "Y", StringComparison.InvariantCultureIgnoreCase); } }

			[JsonProperty("doubleHeader")]
			public string DoubleHeader { get; set; }

			[JsonProperty("gamedayType")]
			public string GamedayType { get; set; }

			[JsonProperty("tiebreaker")]
			public string Tiebreaker { get; set; }

			[JsonProperty("calendarEventID")]
			public string CalendarEventId { get; set; }

			[JsonProperty("seasonDisplay")]
			public string SeasonDisplay { get; set; }

			[JsonIgnore]
			public bool? IsDayGame { get { return string.IsNullOrEmpty(this._dayNight) ? (bool?)null : string.Equals(this._dayNight, "day", StringComparison.InvariantCultureIgnoreCase) ? true : false; } }

			[JsonProperty("dayNight")]
			private string _dayNight { get; set; }

			[JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
			public string Description { get; set; }

			[JsonProperty("scheduledInnings")]
			public int ScheduledInnings { get; set; }

			[JsonProperty("gamesInSeries", NullValueHandling = NullValueHandling.Ignore)]
			public int? GamesInSeries { get; set; }

			[JsonProperty("seriesGameNumber", NullValueHandling = NullValueHandling.Ignore)]
			public int? SeriesGameNumber { get; set; }

			[JsonProperty("seriesDescription")]
			public string SeriesDescription { get; set; }

			[JsonProperty("recordSource")]
			public string RecordSource { get; set; }

			[JsonIgnore]
			public bool IfNecessary { get { return string.Equals("N", this._ifNecessary, StringComparison.InvariantCultureIgnoreCase); } }

			[JsonProperty("ifNecessary")]
			private string _ifNecessary { get; set; }

			[JsonProperty("ifNecessaryDescription")]
			public string IfNecessaryDescription { get; set; }

			[JsonProperty("rescheduleDate", NullValueHandling = NullValueHandling.Ignore)]
			public DateTime? RescheduleDate { get; set; }

			[JsonProperty("rescheduledFrom", NullValueHandling = NullValueHandling.Ignore)]
			public DateTime? RescheduledFrom { get; set; }

			[JsonProperty("resumeDate", NullValueHandling = NullValueHandling.Ignore)]
			public DateTime? ResumeDate { get; set; }

			[JsonProperty("resumedFrom", NullValueHandling = NullValueHandling.Ignore)]
			public DateTime? ResumedFrom { get; set; }
		}

		public partial class FeedGameStatus
		{
			[JsonProperty("abstractGameState")]
			public string AbstractGameState { get; set; }

			[JsonProperty("codedGameState")]
			public string CodedGameState { get; set; }

			[JsonProperty("detailedState")]
			public string DetailedState { get; set; }

			[JsonProperty("statusCode")]
			public string StatusCode { get; set; }

			[JsonProperty("abstractGameCode")]
			public string AbstractGameCode { get; set; }

			[JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]
			public string Reason { get; set; }

			[JsonProperty("startTimeTBD", NullValueHandling = NullValueHandling.Ignore)]
			public bool? StartTimeTbd { get; set; }
		}

		public partial class FeedGameParticipants
		{
			[JsonProperty("away")]
			public FeedTeam Away { get; set; }

			[JsonProperty("home")]
			public FeedTeam Home { get; set; }
		}

		public partial class FeedTeam
		{
			[JsonProperty("leagueRecord")]
			public FeedLeagueRecord LeagueRecord { get; set; }

			[JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
			public int? Score { get; set; }

			[JsonProperty("team")]
			public FeedVenue Team { get; set; }

			[JsonProperty("isWinner", NullValueHandling = NullValueHandling.Ignore)]
			public bool? IsWinner { get; set; }

			[JsonProperty("splitSquad")]
			public bool SplitSquad { get; set; }

			[JsonProperty("seriesNumber", NullValueHandling = NullValueHandling.Ignore)]
			public int? SeriesNumber { get; set; }
		}

		public class FeedLeagueRecord
		{
			[JsonProperty("wins")]
			public int Wins { get; set; }

			[JsonProperty("losses")]
			public int Losses { get; set; }

			[JsonProperty("pct")]
			public string Pct { get; set; }
		}

		public class FeedVenue
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this GameScheduleFeed self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
