using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class BoxscoreFeed
	{
		public static string GetFeedUrl(int gid)
		{
			return string.Format($"http://statsapi.mlb.com/api/v1/game/{gid}/boxscore");
		}

		public static BoxscoreFeed FromJson(string json) => JsonConvert.DeserializeObject<BoxscoreFeed>(json, Converter.Settings);


		[JsonProperty("teams")]
		public TeamsNode Teams { get; set; }

		[JsonProperty("officials")]
		public List<OfficialElement> Officials { get; set; }

		[JsonProperty("info")]
		public List<NoteElement> Info { get; set; }

		[JsonProperty("pitchingNotes")]
		public List<object> PitchingNotes { get; set; }
		public class NoteElement
		{
			[JsonProperty("label")]
			public string Label { get; set; }

			[JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
			public string Value { get; set; }
		}

		public class OfficialElement
		{
			[JsonProperty("official")]
			public SimplePerson Official { get; set; }

			[JsonProperty("officialType")]
			public string OfficialType { get; set; }
		}

		public class SimplePerson
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("fullName")]
			public string FullName { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }
		}

		public class TeamsNode
		{
			[JsonProperty("away")]
			public GameTeam Away { get; set; }

			[JsonProperty("home")]
			public GameTeam Home { get; set; }
		}

		public class GameTeam
		{
			[JsonProperty("team")]
			public Team Team { get; set; }

			[JsonProperty("teamStats")]
			public TeamStats TeamStats { get; set; }

			[JsonProperty("players")]
			public Dictionary<string, GamePlayer> Players { get; set; }

			[JsonProperty("batters")]
			public List<int> Batters { get; set; }

			[JsonProperty("pitchers")]
			public List<int> Pitchers { get; set; }

			[JsonProperty("bench")]
			public List<int> Bench { get; set; }

			[JsonProperty("bullpen")]
			public List<int> Bullpen { get; set; }

			[JsonProperty("battingOrder")]
			public List<int> BattingOrder { get; set; }

			[JsonProperty("info")]
			public List<TeamInfo> Info { get; set; }

			[JsonProperty("note")]
			public List<NoteElement> Note { get; set; }
		}

		public class TeamInfo
		{
			[JsonProperty("title")]
			public string Title { get; set; }

			[JsonProperty("fieldList")]
			public List<NoteElement> FieldList { get; set; }
		}

		public class Position
		{
			[JsonProperty("code")]
			public string Code { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("type")]
			public string Type { get; set; }

			[JsonProperty("abbreviation")]
			public string Abbreviation { get; set; }
		}

		public class GameStatus
		{
			[JsonProperty("isCurrentBatter")]
			public bool IsCurrentBatter { get; set; }

			[JsonProperty("isCurrentPitcher")]
			public bool IsCurrentPitcher { get; set; }

			[JsonProperty("isOnBench")]
			public bool IsOnBench { get; set; }

			[JsonProperty("isSubstitute")]
			public bool IsSubstitute { get; set; }
		}

		public class SeasonStats
		{
			[JsonProperty("batting")]
			public Batting Batting { get; set; }

			[JsonProperty("pitching")]
			public SeasonStatsPitching Pitching { get; set; }

			[JsonProperty("fielding")]
			public Fielding Fielding { get; set; }
		}

		public class Batting
		{
			[JsonProperty("gamesPlayed", NullValueHandling = NullValueHandling.Ignore)]
			public int? GamesPlayed { get; set; }

			[JsonProperty("flyOuts")]
			public int FlyOuts { get; set; }

			[JsonProperty("groundOuts")]
			public int GroundOuts { get; set; }

			[JsonProperty("runs")]
			public int Runs { get; set; }

			[JsonProperty("doubles")]
			public int Doubles { get; set; }

			[JsonProperty("triples")]
			public int Triples { get; set; }

			[JsonProperty("homeRuns")]
			public int HomeRuns { get; set; }

			[JsonProperty("strikeOuts")]
			public int StrikeOuts { get; set; }

			[JsonProperty("baseOnBalls")]
			public int BaseOnBalls { get; set; }

			[JsonProperty("intentionalWalks")]
			public int IntentionalWalks { get; set; }

			[JsonProperty("hits")]
			public int Hits { get; set; }

			[JsonProperty("hitByPitch")]
			public int HitByPitch { get; set; }

			[JsonProperty("avg", NullValueHandling = NullValueHandling.Ignore)]
			public string Avg { get; set; }

			[JsonProperty("atBats")]
			public int AtBats { get; set; }

			[JsonProperty("obp", NullValueHandling = NullValueHandling.Ignore)]
			public string Obp { get; set; }

			[JsonProperty("slg", NullValueHandling = NullValueHandling.Ignore)]
			public string Slg { get; set; }

			[JsonProperty("ops", NullValueHandling = NullValueHandling.Ignore)]
			public string Ops { get; set; }

			[JsonProperty("caughtStealing")]
			public int CaughtStealing { get; set; }

			[JsonProperty("stolenBases")]
			public int StolenBases { get; set; }

			[JsonProperty("stolenBasePercentage", NullValueHandling = NullValueHandling.Ignore)]
			public string StolenBasePercentage { get; set; }

			[JsonProperty("groundIntoDoublePlay")]
			public int GroundIntoDoublePlay { get; set; }

			[JsonProperty("groundIntoTriplePlay")]
			public int GroundIntoTriplePlay { get; set; }

			[JsonProperty("totalBases")]
			public int TotalBases { get; set; }

			[JsonProperty("rbi")]
			public int Rbi { get; set; }

			[JsonProperty("leftOnBase")]
			public int LeftOnBase { get; set; }

			[JsonProperty("sacBunts")]
			public int SacBunts { get; set; }

			[JsonProperty("sacFlies")]
			public int SacFlies { get; set; }

			[JsonProperty("catchersInterference")]
			public int CatchersInterference { get; set; }

			[JsonProperty("pickoffs")]
			public int Pickoffs { get; set; }

			[JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
			public string Note { get; set; }
		}

		public class Fielding
		{
			[JsonProperty("assists")]
			public int Assists { get; set; }

			[JsonProperty("putOuts")]
			public int PutOuts { get; set; }

			[JsonProperty("errors")]
			public int Errors { get; set; }

			[JsonProperty("chances")]
			public int Chances { get; set; }

			[JsonProperty("fielding", NullValueHandling = NullValueHandling.Ignore)]
			public string FieldingFielding { get; set; }

			[JsonProperty("caughtStealing")]
			public int CaughtStealing { get; set; }

			[JsonProperty("passedBall")]
			public int PassedBall { get; set; }

			[JsonProperty("stolenBases")]
			public int StolenBases { get; set; }

			[JsonProperty("stolenBasePercentage")]
			public string StolenBasePercentage { get; set; }

			[JsonProperty("pickoffs")]
			public int Pickoffs { get; set; }
		}

		public class SeasonStatsPitching
		{
			[JsonProperty("gamesPlayed")]
			public int GamesPlayed { get; set; }

			[JsonProperty("gamesStarted")]
			public int GamesStarted { get; set; }

			[JsonProperty("groundOuts")]
			public int GroundOuts { get; set; }

			[JsonProperty("runs")]
			public int Runs { get; set; }

			[JsonProperty("doubles")]
			public int Doubles { get; set; }

			[JsonProperty("triples")]
			public int Triples { get; set; }

			[JsonProperty("homeRuns")]
			public int HomeRuns { get; set; }

			[JsonProperty("strikeOuts")]
			public int StrikeOuts { get; set; }

			[JsonProperty("baseOnBalls")]
			public int BaseOnBalls { get; set; }

			[JsonProperty("intentionalWalks")]
			public int IntentionalWalks { get; set; }

			[JsonProperty("hits")]
			public int Hits { get; set; }

			[JsonProperty("atBats")]
			public int AtBats { get; set; }

			[JsonProperty("caughtStealing")]
			public int CaughtStealing { get; set; }

			[JsonProperty("stolenBases")]
			public int StolenBases { get; set; }

			[JsonProperty("stolenBasePercentage", NullValueHandling = NullValueHandling.Ignore)]
			public string StolenBasePercentage { get; set; }

			[JsonProperty("era", NullValueHandling = NullValueHandling.Ignore)]
			public string Era { get; set; }

			[JsonProperty("inningsPitched")]
			public string InningsPitched { get; set; }

			[JsonProperty("wins", NullValueHandling = NullValueHandling.Ignore)]
			public int? Wins { get; set; }

			[JsonProperty("losses", NullValueHandling = NullValueHandling.Ignore)]
			public int? Losses { get; set; }

			[JsonProperty("saves", NullValueHandling = NullValueHandling.Ignore)]
			public int? Saves { get; set; }

			[JsonProperty("saveOpportunities")]
			public int SaveOpportunities { get; set; }

			[JsonProperty("holds", NullValueHandling = NullValueHandling.Ignore)]
			public int? Holds { get; set; }

			[JsonProperty("blownSaves", NullValueHandling = NullValueHandling.Ignore)]
			public int? BlownSaves { get; set; }

			[JsonProperty("earnedRuns")]
			public int EarnedRuns { get; set; }

			[JsonProperty("whip", NullValueHandling = NullValueHandling.Ignore)]
			public string Whip { get; set; }

			[JsonProperty("outs")]
			public int Outs { get; set; }

			[JsonProperty("gamesPitched")]
			public int GamesPitched { get; set; }

			[JsonProperty("completeGames")]
			public int CompleteGames { get; set; }

			[JsonProperty("shutouts")]
			public int Shutouts { get; set; }

			[JsonProperty("hitBatsmen")]
			public int HitBatsmen { get; set; }

			[JsonProperty("wildPitches")]
			public int WildPitches { get; set; }

			[JsonProperty("pickoffs")]
			public int Pickoffs { get; set; }

			[JsonProperty("airOuts")]
			public int AirOuts { get; set; }

			[JsonProperty("rbi")]
			public int Rbi { get; set; }

			[JsonProperty("winPercentage", NullValueHandling = NullValueHandling.Ignore)]
			public string WinPercentage { get; set; }

			[JsonProperty("gamesFinished")]
			public int GamesFinished { get; set; }

			[JsonProperty("strikeoutWalkRatio", NullValueHandling = NullValueHandling.Ignore)]
			public string StrikeoutWalkRatio { get; set; }

			[JsonProperty("strikeoutsPer9Inn", NullValueHandling = NullValueHandling.Ignore)]
			public string StrikeoutsPer9Inn { get; set; }

			[JsonProperty("walksPer9Inn", NullValueHandling = NullValueHandling.Ignore)]
			public string WalksPer9Inn { get; set; }

			[JsonProperty("hitsPer9Inn", NullValueHandling = NullValueHandling.Ignore)]
			public string HitsPer9Inn { get; set; }

			[JsonProperty("inheritedRunners")]
			public int InheritedRunners { get; set; }

			[JsonProperty("inheritedRunnersScored")]
			public int InheritedRunnersScored { get; set; }

			[JsonProperty("catchersInterference")]
			public int CatchersInterference { get; set; }

			[JsonProperty("sacBunts")]
			public int SacBunts { get; set; }

			[JsonProperty("sacFlies")]
			public int SacFlies { get; set; }

			[JsonProperty("numberOfPitches", NullValueHandling = NullValueHandling.Ignore)]
			public int? NumberOfPitches { get; set; }

			[JsonProperty("battersFaced", NullValueHandling = NullValueHandling.Ignore)]
			public int? BattersFaced { get; set; }

			[JsonProperty("pitchesThrown", NullValueHandling = NullValueHandling.Ignore)]
			public int? PitchesThrown { get; set; }

			[JsonProperty("balls", NullValueHandling = NullValueHandling.Ignore)]
			public int? Balls { get; set; }

			[JsonProperty("strikes", NullValueHandling = NullValueHandling.Ignore)]
			public int? Strikes { get; set; }

			[JsonProperty("flyOuts", NullValueHandling = NullValueHandling.Ignore)]
			public int? FlyOuts { get; set; }

			[JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
			public string Note { get; set; }
		}

		public class Status
		{
			[JsonProperty("code")]
			public string Code { get; set; }

			[JsonProperty("description")]
			public string Description { get; set; }
		}

		public class GamePlayer
		{
			[JsonProperty("person")]
			public SimplePerson Person { get; set; }

			[JsonIgnore]
			public int? JerseyNumber { get { return int.TryParse(this._jerseyNumber, out int itp) ? itp : (int?)null; } }

			[JsonProperty("jerseyNumber")]
			private string _jerseyNumber { get; set; }

			[JsonProperty("position")]
			public Position Position { get; set; }

			[JsonProperty("stats")]
			public GamePlayerStats Stats { get; set; }

			[JsonProperty("status")]
			public Status Status { get; set; }

			[JsonProperty("parentTeamId")]
			public int ParentTeamId { get; set; }

			[JsonProperty("battingOrder")]
			public int BattingOrder { get; set; }

			[JsonProperty("seasonStats")]
			public SeasonStats SeasonStats { get; set; }

			[JsonProperty("gameStatus")]
			public GameStatus GameStatus { get; set; }

			[JsonProperty("allPositions")]
			public List<Position> AllPositions { get; set; }
		}

		public class GamePlayerStats
		{
			[JsonProperty("batting")]
			public Batting Batting { get; set; }

			[JsonProperty("pitching")]
			public GameStatsPitching Pitching { get; set; }

			[JsonProperty("fielding")]
			public Fielding Fielding { get; set; }
		}

		public class GameStatsPitching
		{
			[JsonProperty("gamesPlayed")]
			public int GamesPlayed { get; set; }

			[JsonProperty("gamesStarted")]
			public int GamesStarted { get; set; }

			[JsonProperty("groundOuts")]
			public int GroundOuts { get; set; }

			[JsonProperty("runs")]
			public int Runs { get; set; }

			[JsonProperty("doubles")]
			public int Doubles { get; set; }

			[JsonProperty("triples")]
			public int Triples { get; set; }

			[JsonProperty("homeRuns")]
			public int HomeRuns { get; set; }

			[JsonProperty("strikeOuts")]
			public int StrikeOuts { get; set; }

			[JsonProperty("baseOnBalls")]
			public int BaseOnBalls { get; set; }

			[JsonProperty("intentionalWalks")]
			public int IntentionalWalks { get; set; }

			[JsonProperty("hits")]
			public int Hits { get; set; }

			[JsonProperty("atBats")]
			public int AtBats { get; set; }

			[JsonProperty("caughtStealing")]
			public int CaughtStealing { get; set; }

			[JsonProperty("stolenBases")]
			public int StolenBases { get; set; }

			[JsonProperty("stolenBasePercentage", NullValueHandling = NullValueHandling.Ignore)]
			public string StolenBasePercentage { get; set; }

			[JsonProperty("era", NullValueHandling = NullValueHandling.Ignore)]
			public string Era { get; set; }

			[JsonProperty("inningsPitched")]
			public string InningsPitched { get; set; }

			[JsonProperty("wins", NullValueHandling = NullValueHandling.Ignore)]
			public int? Wins { get; set; }

			[JsonProperty("losses", NullValueHandling = NullValueHandling.Ignore)]
			public int? Losses { get; set; }

			[JsonProperty("saves", NullValueHandling = NullValueHandling.Ignore)]
			public int? Saves { get; set; }

			[JsonProperty("saveOpportunities")]
			public int SaveOpportunities { get; set; }

			[JsonProperty("holds", NullValueHandling = NullValueHandling.Ignore)]
			public int? Holds { get; set; }

			[JsonProperty("blownSaves", NullValueHandling = NullValueHandling.Ignore)]
			public int? BlownSaves { get; set; }

			[JsonProperty("earnedRuns")]
			public int EarnedRuns { get; set; }

			[JsonProperty("whip", NullValueHandling = NullValueHandling.Ignore)]
			public string Whip { get; set; }

			[JsonProperty("outs")]
			public int Outs { get; set; }

			[JsonProperty("gamesPitched")]
			public int GamesPitched { get; set; }

			[JsonProperty("completeGames")]
			public int CompleteGames { get; set; }

			[JsonProperty("shutouts")]
			public int Shutouts { get; set; }

			[JsonProperty("hitBatsmen")]
			public int HitBatsmen { get; set; }

			[JsonProperty("wildPitches")]
			public int WildPitches { get; set; }

			[JsonProperty("pickoffs")]
			public int Pickoffs { get; set; }

			[JsonProperty("airOuts")]
			public int AirOuts { get; set; }

			[JsonProperty("rbi")]
			public int Rbi { get; set; }

			[JsonProperty("gamesFinished")]
			public int GamesFinished { get; set; }

			[JsonProperty("inheritedRunners")]
			public int InheritedRunners { get; set; }

			[JsonProperty("inheritedRunnersScored")]
			public int InheritedRunnersScored { get; set; }

			[JsonProperty("catchersInterference")]
			public int CatchersInterference { get; set; }

			[JsonProperty("sacBunts")]
			public int SacBunts { get; set; }

			[JsonProperty("sacFlies")]
			public int SacFlies { get; set; }

			[JsonProperty("numberOfPitches", NullValueHandling = NullValueHandling.Ignore)]
			public int? NumberOfPitches { get; set; }

			[JsonProperty("battersFaced", NullValueHandling = NullValueHandling.Ignore)]
			public int? BattersFaced { get; set; }

			[JsonProperty("pitchesThrown", NullValueHandling = NullValueHandling.Ignore)]
			public int? PitchesThrown { get; set; }

			[JsonProperty("balls", NullValueHandling = NullValueHandling.Ignore)]
			public int? Balls { get; set; }

			[JsonProperty("strikes", NullValueHandling = NullValueHandling.Ignore)]
			public int? Strikes { get; set; }

			[JsonProperty("flyOuts", NullValueHandling = NullValueHandling.Ignore)]
			public int? FlyOuts { get; set; }

			[JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
			public string Note { get; set; }
		}

		public class Team
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }

			[JsonProperty("season")]
			public int Season { get; set; }

			[JsonProperty("venue")]
			public Division Venue { get; set; }

			[JsonProperty("teamCode")]
			public string TeamCode { get; set; }

			[JsonProperty("fileCode")]
			public string FileCode { get; set; }

			[JsonProperty("abbreviation")]
			public string Abbreviation { get; set; }

			[JsonProperty("teamName")]
			public string TeamName { get; set; }

			[JsonProperty("locationName")]
			public string LocationName { get; set; }

			[JsonProperty("firstYearOfPlay")]
			public int FirstYearOfPlay { get; set; }

			[JsonProperty("league")]
			public Division League { get; set; }

			[JsonProperty("division")]
			public Division Division { get; set; }

			[JsonProperty("sport")]
			public Division Sport { get; set; }

			[JsonProperty("shortName")]
			public string ShortName { get; set; }

			[JsonProperty("record")]
			public TeamRecord Record { get; set; }

			[JsonProperty("springLeague")]
			public Division SpringLeague { get; set; }

			[JsonProperty("allStarStatus")]
			public string AllStarStatus { get; set; }

			[JsonProperty("active")]
			public bool Active { get; set; }
		}

		public class Division
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }

			[JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
			public string Abbreviation { get; set; }
		}

		public class TeamRecord
		{
			[JsonProperty("gamesPlayed")]
			public int GamesPlayed { get; set; }

			[JsonProperty("wildCardGamesBack")]
			public string WildCardGamesBack { get; set; }

			[JsonProperty("leagueGamesBack")]
			public string LeagueGamesBack { get; set; }

			[JsonProperty("springLeagueGamesBack")]
			public string SpringLeagueGamesBack { get; set; }

			[JsonProperty("sportGamesBack")]
			public string SportGamesBack { get; set; }

			[JsonProperty("divisionGamesBack")]
			public string DivisionGamesBack { get; set; }

			[JsonProperty("conferenceGamesBack")]
			public string ConferenceGamesBack { get; set; }

			[JsonProperty("leagueRecord")]
			public LeagueRecord LeagueRecord { get; set; }

			// NO IDEA WHAT THIS IS.... NULL IN ALL SAMPLES
			// [JsonProperty("records")]
			// public object Records { get; set; }

			[JsonProperty("divisionLeader")]
			public bool DivisionLeader { get; set; }

			[JsonProperty("wins")]
			public int Wins { get; set; }

			[JsonProperty("losses")]
			public int Losses { get; set; }

			[JsonProperty("winningPercentage")]
			public string WinningPercentage { get; set; }
		}

		public class LeagueRecord
		{
			[JsonProperty("wins")]
			public int Wins { get; set; }

			[JsonProperty("losses")]
			public int Losses { get; set; }

			[JsonProperty("pct")]
			public string Pct { get; set; }
		}

		public class TeamStats
		{
			[JsonProperty("batting")]
			public Batting Batting { get; set; }

			[JsonProperty("pitching")]
			public TeamStatsPitching Pitching { get; set; }

			[JsonProperty("fielding")]
			public Fielding Fielding { get; set; }
		}

		public class TeamStatsPitching
		{
			[JsonProperty("groundOuts")]
			public int GroundOuts { get; set; }

			[JsonProperty("runs")]
			public int Runs { get; set; }

			[JsonProperty("doubles")]
			public int Doubles { get; set; }

			[JsonProperty("triples")]
			public int Triples { get; set; }

			[JsonProperty("homeRuns")]
			public int HomeRuns { get; set; }

			[JsonProperty("strikeOuts")]
			public int StrikeOuts { get; set; }

			[JsonProperty("baseOnBalls")]
			public int BaseOnBalls { get; set; }

			[JsonProperty("intentionalWalks")]
			public int IntentionalWalks { get; set; }

			[JsonProperty("hits")]
			public int Hits { get; set; }

			[JsonProperty("atBats")]
			public int AtBats { get; set; }

			[JsonProperty("caughtStealing")]
			public int CaughtStealing { get; set; }

			[JsonProperty("stolenBases")]
			public int StolenBases { get; set; }

			[JsonProperty("era")]
			public string Era { get; set; }

			[JsonProperty("inningsPitched")]
			public string InningsPitched { get; set; }

			[JsonProperty("saveOpportunities")]
			public int SaveOpportunities { get; set; }

			[JsonProperty("earnedRuns")]
			public int EarnedRuns { get; set; }

			[JsonProperty("whip")]
			public string Whip { get; set; }

			[JsonProperty("battersFaced")]
			public int BattersFaced { get; set; }

			[JsonProperty("outs")]
			public int Outs { get; set; }

			[JsonProperty("completeGames")]
			public int CompleteGames { get; set; }

			[JsonProperty("shutouts")]
			public int Shutouts { get; set; }

			[JsonProperty("hitBatsmen")]
			public int HitBatsmen { get; set; }

			[JsonProperty("wildPitches")]
			public int WildPitches { get; set; }

			[JsonProperty("pickoffs")]
			public int Pickoffs { get; set; }

			[JsonProperty("airOuts")]
			public int AirOuts { get; set; }

			[JsonProperty("rbi")]
			public int Rbi { get; set; }

			[JsonProperty("inheritedRunners")]
			public int InheritedRunners { get; set; }

			[JsonProperty("inheritedRunnersScored")]
			public int InheritedRunnersScored { get; set; }

			[JsonProperty("catchersInterference")]
			public int CatchersInterference { get; set; }

			[JsonProperty("sacBunts")]
			public int SacBunts { get; set; }

			[JsonProperty("sacFlies")]
			public int SacFlies { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this BoxscoreFeed self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
