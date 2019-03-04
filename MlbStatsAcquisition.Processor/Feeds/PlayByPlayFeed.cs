using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MlbStatsAcquisition.Processor.Feeds
{
	public class PlayByPlayFeed
	{
		public static string GetFeedUrl(int gameId)
		{
			return string.Format($"http://statsapi.mlb.com/api/v1/game/{gameId}/playByPlay");
		}

		public static PlayByPlayFeed FromJson(string json) => JsonConvert.DeserializeObject<PlayByPlayFeed>(json, Converter.Settings);

		// ONLY AllPlays KEPT IN FEED PARSER... DATA IN ALL OTHER NODES REDUNDANT WITH DATA IN AllPlays
		[JsonProperty("allPlays")]
		public List<FeedPlay> AllPlays { get; set; }

		public class FeedPlay
		{
			[JsonProperty("result")]
			public Result Result { get; set; }

			[JsonProperty("about")]
			public About About { get; set; }

			[JsonProperty("count")]
			public Count Count { get; set; }

			[JsonProperty("matchup")]
			public Matchup Matchup { get; set; }

			[JsonProperty("pitchIndex")]
			public List<int> PitchIndex { get; set; }

			[JsonProperty("actionIndex")]
			public List<int> ActionIndex { get; set; }

			[JsonProperty("runnerIndex")]
			public List<int> RunnerIndex { get; set; }

			[JsonProperty("runners")]
			public List<Runner> Runners { get; set; }

			[JsonProperty("playEvents")]
			public List<PlayEvent> PlayEvents { get; set; }

			[JsonProperty("atBatIndex")]
			public short AtBatIndex { get; set; }

			[JsonProperty("playEndTime")]
			public DateTimeOffset PlayEndTime { get; set; }
		}

		public class About
		{
			[JsonProperty("atBatIndex")]
			public int AtBatIndex { get; set; }

			[JsonProperty("halfInning")]
			public string HalfInning { get; set; }

			[JsonProperty("inning")]
			public byte Inning { get; set; }

			[JsonProperty("startTime")]
			public DateTime? StartTime { get; set; }

			[JsonProperty("endTime")]
			public DateTime? EndTime { get; set; }

			[JsonProperty("isComplete")]
			public bool IsComplete { get; set; }

			[JsonProperty("isScoringPlay")]
			public bool IsScoringPlay { get; set; }

			[JsonProperty("hasReview")]
			public bool HasReview { get; set; }

			[JsonProperty("hasOut")]
			public bool HasOut { get; set; }

			[JsonProperty("captivatingIndex")]
			public int CaptivatingIndex { get; set; }
		}

		public class Count
		{
			[JsonProperty("balls", NullValueHandling = NullValueHandling.Ignore)]
			public int? Balls { get; set; }

			[JsonProperty("strikes", NullValueHandling = NullValueHandling.Ignore)]
			public int? Strikes { get; set; }

			[JsonProperty("outs", NullValueHandling = NullValueHandling.Ignore)]
			public int? Outs { get; set; }
		}

		public class Matchup
		{
			// HITTER AND PITCHER HOT/COLD ZONES REMOVED TO OPTIMIZE PARSING.... GENERALLY NULL ANYWAY
			[JsonProperty("batter")]
			public SimplePlayer Batter { get; set; }

			[JsonProperty("batSide")]
			public CodedDataNode BatSide { get; set; }

			[JsonProperty("pitcher")]
			public SimplePlayer Pitcher { get; set; }

			[JsonProperty("pitchHand")]
			public CodedDataNode PitchHand { get; set; }

			[JsonProperty("splits")]
			public Splits Splits { get; set; }
		}

		public class CodedDataNode
		{
			[JsonProperty("code")]
			public string Code { get; set; }

			[JsonProperty("description")]
			public string Description { get; set; }
		}

		public class SimplePlayer
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("fullName")]
			public string FullName { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }
		}

		public class EntityIdNode
		{
			[JsonProperty("id")]
			public int Id { get; set; }

			[JsonProperty("link")]
			public string Link { get; set; }
		}

		public class Splits
		{
			[JsonProperty("batter")]
			public string Batter { get; set; }

			[JsonProperty("pitcher")]
			public string Pitcher { get; set; }

			[JsonProperty("menOnBase")]
			public string MenOnBase { get; set; }
		}

		public class PlayEvent
		{
			[JsonProperty("details")]
			public EventDetails Details { get; set; }

			[JsonProperty("count")]
			public Count Count { get; set; }

			[JsonProperty("pitchData", NullValueHandling = NullValueHandling.Ignore)]
			public PitchData PitchData { get; set; }

			[JsonProperty("index")]
			public int Index { get; set; }

			[JsonProperty("pfxId", NullValueHandling = NullValueHandling.Ignore)]
			public string PfxId { get; set; }

			[JsonProperty("playId", NullValueHandling = NullValueHandling.Ignore)]
			public Guid? PlayId { get; set; }

			[JsonProperty("pitchNumber", NullValueHandling = NullValueHandling.Ignore)]
			public int? PitchNumber { get; set; }

			[JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
			public DateTimeOffset? StartTime { get; set; }

			[JsonProperty("endTime", NullValueHandling = NullValueHandling.Ignore)]
			public DateTimeOffset? EndTime { get; set; }

			[JsonProperty("isPitch")]
			public bool IsPitch { get; set; }

			[JsonProperty("type")]
			public string Type { get; set; }

			[JsonProperty("hitData", NullValueHandling = NullValueHandling.Ignore)]
			public HitData HitData { get; set; }

			[JsonProperty("player", NullValueHandling = NullValueHandling.Ignore)]
			public EntityIdNode Player { get; set; }

			[JsonProperty("battingOrder", NullValueHandling = NullValueHandling.Ignore)]
			public int? BattingOrder { get; set; }

			[JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
			public Position Position { get; set; }
		}

		public class EventDetails
		{
			[JsonProperty("call", NullValueHandling = NullValueHandling.Ignore)]
			public CodedDataNode Call { get; set; }

			[JsonProperty("description")]
			public string Description { get; set; }

			[JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
			public string Code { get; set; }

			[JsonProperty("ballColor", NullValueHandling = NullValueHandling.Ignore)]
			public string BallColor { get; set; }

			[JsonProperty("trailColor", NullValueHandling = NullValueHandling.Ignore)]
			public string TrailColor { get; set; }

			[JsonProperty("isInPlay", NullValueHandling = NullValueHandling.Ignore)]
			public bool? IsInPlay { get; set; }

			[JsonProperty("isStrike", NullValueHandling = NullValueHandling.Ignore)]
			public bool? IsStrike { get; set; }

			[JsonProperty("isBall", NullValueHandling = NullValueHandling.Ignore)]
			public bool? IsBall { get; set; }

			[JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
			public CodedDataNode Type { get; set; }

			[JsonProperty("hasReview")]
			public bool HasReview { get; set; }

			[JsonProperty("fromCatcher", NullValueHandling = NullValueHandling.Ignore)]
			public bool? FromCatcher { get; set; }

			[JsonProperty("event", NullValueHandling = NullValueHandling.Ignore)]
			public string Event { get; set; }

			[JsonProperty("awayScore", NullValueHandling = NullValueHandling.Ignore)]
			public int? AwayScore { get; set; }

			[JsonProperty("homeScore", NullValueHandling = NullValueHandling.Ignore)]
			public int? HomeScore { get; set; }

			[JsonProperty("isScoringPlay", NullValueHandling = NullValueHandling.Ignore)]
			public bool? IsScoringPlay { get; set; }

			[JsonProperty("eventType", NullValueHandling = NullValueHandling.Ignore)]
			public string EventType { get; set; }

			[JsonProperty("runnerGoing", NullValueHandling = NullValueHandling.Ignore)]
			public bool? RunnerGoing { get; set; }
		}

		public class HitData
		{
			[JsonProperty("launchSpeed", NullValueHandling = NullValueHandling.Ignore)]
			public double? LaunchSpeed { get; set; }

			[JsonProperty("launchAngle", NullValueHandling = NullValueHandling.Ignore)]
			public double? LaunchAngle { get; set; }

			[JsonProperty("totalDistance", NullValueHandling = NullValueHandling.Ignore)]
			public double? TotalDistance { get; set; }

			[JsonProperty("trajectory")]
			public string Trajectory { get; set; }

			[JsonProperty("hardness")]
			public string Hardness { get; set; }

			[JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
			public int? Location { get; set; }

			[JsonProperty("coordinates")]
			public HitDataCoordinates Coordinates { get; set; }
		}

		public class HitDataCoordinates
		{
			[JsonProperty("coordX")]
			public double CoordX { get; set; }

			[JsonProperty("coordY")]
			public double CoordY { get; set; }
		}

		public class PitchData
		{
			[JsonProperty("startSpeed", NullValueHandling = NullValueHandling.Ignore)]
			public double? StartSpeed { get; set; }

			[JsonProperty("endSpeed", NullValueHandling = NullValueHandling.Ignore)]
			public double? EndSpeed { get; set; }

			[JsonProperty("nastyFactor", NullValueHandling = NullValueHandling.Ignore)]
			public double? NastyFactor { get; set; }

			[JsonProperty("strikeZoneTop")]
			public double StrikeZoneTop { get; set; }

			[JsonProperty("strikeZoneBottom")]
			public double StrikeZoneBottom { get; set; }

			[JsonProperty("coordinates")]
			public Dictionary<string, double> Coordinates { get; set; }

			[JsonProperty("breaks")]
			public Breaks Breaks { get; set; }

			[JsonProperty("zone", NullValueHandling = NullValueHandling.Ignore)]
			public int? Zone { get; set; }

			[JsonProperty("typeConfidence", NullValueHandling = NullValueHandling.Ignore)]
			public double? TypeConfidence { get; set; }
		}

		public class Breaks
		{
			[JsonProperty("breakAngle", NullValueHandling = NullValueHandling.Ignore)]
			public double? BreakAngle { get; set; }

			[JsonProperty("breakLength", NullValueHandling = NullValueHandling.Ignore)]
			public double? BreakLength { get; set; }

			[JsonProperty("breakY", NullValueHandling = NullValueHandling.Ignore)]
			public double? BreakY { get; set; }

			[JsonProperty("spinRate", NullValueHandling = NullValueHandling.Ignore)]
			public double? SpinRate { get; set; }

			[JsonProperty("spinDirection", NullValueHandling = NullValueHandling.Ignore)]
			public double? SpinDirection { get; set; }
		}

		public class Position
		{
			[JsonProperty("code")]
			public int Code { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("type")]
			public string Type { get; set; }

			[JsonProperty("abbreviation")]
			public string Abbreviation { get; set; }
		}

		public class Result
		{
			[JsonProperty("type")]
			public string Type { get; set; }

			[JsonProperty("event")]
			public string Event { get; set; }

			[JsonProperty("eventType")]
			public string EventType { get; set; }

			[JsonProperty("description")]
			public string Description { get; set; }

			[JsonProperty("rbi")]
			public byte Rbi { get; set; }

			[JsonProperty("awayScore")]
			public byte AwayScore { get; set; }

			[JsonProperty("homeScore")]
			public byte HomeScore { get; set; }
		}

		public class Runner
		{
			[JsonProperty("movement")]
			public Movement Movement { get; set; }

			[JsonProperty("details")]
			public RunnerDetails Details { get; set; }

			[JsonProperty("credits", NullValueHandling = NullValueHandling.Ignore)]
			public List<Credit> Credits { get; set; }
		}

		public class Credit
		{
			[JsonProperty("player")]
			public EntityIdNode Player { get; set; }

			[JsonProperty("position")]
			public Position Position { get; set; }

			[JsonProperty("credit")]
			public string CreditCredit { get; set; }
		}

		public class RunnerDetails
		{
			[JsonProperty("event")]
			public string Event { get; set; }

			[JsonProperty("eventType")]
			public string EventType { get; set; }

			[JsonProperty("movementReason")]
			public string MovementReason { get; set; }

			[JsonProperty("runner")]
			public SimplePlayer Runner { get; set; }

			[JsonProperty("responsiblePitcher")]
			public EntityIdNode ResponsiblePitcher { get; set; }

			[JsonProperty("isScoringEvent")]
			public bool IsScoringEvent { get; set; }

			[JsonProperty("rbi")]
			public bool Rbi { get; set; }

			[JsonProperty("earned")]
			public bool Earned { get; set; }

			[JsonProperty("teamUnearned")]
			public bool TeamUnearned { get; set; }

			[JsonProperty("playIndex")]
			public short PlayIndex { get; set; }
		}

		public class Movement
		{
			[JsonProperty("start")]
			public string Start { get; set; }

			[JsonProperty("end")]
			public string End { get; set; }

			[JsonProperty("outBase")]
			public string OutBase { get; set; }

			[JsonProperty("isOut")]
			public bool? IsOut { get; set; }

			[JsonProperty("outNumber")]
			public byte? OutNumber { get; set; }
		}
	}

	public static partial class Serialize
	{
		public static string ToJson(this PlayByPlayFeed self) => JsonConvert.SerializeObject(self, Converter.Settings);
	}
}
