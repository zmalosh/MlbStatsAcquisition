using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class Game : MlbStatsEntity
	{
		public int GameID { get; set; }
		public int Season { get; set; }
		public int AssociationID { get; set; }
		public string GameTypeID { get; set; }
		public int? AwayTeamID { get; set; }
		public int? HomeTeamID { get; set; }
		public int? VenueID { get; set; }
		public DateTime GameTime { get; set; }
		public int GameStatus { get; set; }
		public byte? AwayScore { get; set; }
		public byte? HomeScore { get; set; }
		public bool? IsTie { get; set; }
		public bool IsDoubleHeader { get; set; }
		public byte DayGameNum { get; set; }
		public bool? IsDayGame { get; set; }
		public bool? IsTBD { get; set; }
		public bool IsIfNecessary { get; set; }
		public string IfNecessaryDescription { get; set; }
		public byte ScheduledLength { get; set; }
		public byte? SeriesLength { get; set; }
		public byte? SeriesGameNum { get; set; }
		public string SeriesDescription { get; set; }
		public string RecordSource { get; set; }
		public byte? AwaySeriesNum { get; set; }
		public byte? AwayWins { get; set; }
		public byte? AwayLosses { get; set; }
		public bool? IsAwaySplitSquad { get; set; }
		public byte? HomeSeriesNum { get; set; }
		public byte? HomeWins { get; set; }
		public byte? HomeLosses { get; set; }
		public bool? IsHomeSplitSquad { get; set; }
		public int? AltAssociationID { get; set; }
		public string RawSeason { get; set; }
		public DateTime? RescheduledDate { get; set; }
		public DateTime? RescheduledFromDate { get; set; }
		public DateTime? ResumeDate { get; set; }
		public DateTime? ResumedFrom { get; set; }

		public virtual AssociationSeason AssociationSeason { get; set; }
		public virtual TeamSeason AwayTeamSeason { get; set; }
		public virtual TeamSeason HomeTeamSeason { get; set; }
		public virtual VenueSeason VenueSeason { get; set; }

		public virtual ICollection<UmpireAssignment> UmpireAssignments { get; set; }

		//public virtual ICollection<GamePlay> Plays { get; set; }
		public virtual ICollection<PlayerHittingBoxscore> PlayerHittingBoxscores { get; set; }
	}
}
