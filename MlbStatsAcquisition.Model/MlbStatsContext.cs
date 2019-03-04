using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlbStatsAcquisition.Model
{
	public class MlbStatsContext : DbContext
	{
		public DbSet<Position> Positions { get; set; }
		public DbSet<StatType> StatTypes { get; set; }
		public DbSet<GameEventType> GameEventTypes { get; set; }
		public DbSet<GameSituationType> GameSituationTypes { get; set; }
		public DbSet<GameStatusType> GameStatusTypes { get; set; }
		public DbSet<GameType> GameTypes { get; set; }
		public DbSet<HitTrajectoryType> HitTrajectoryTypes { get; set; }
		public DbSet<JobType> JobTypes { get; set; }
		public DbSet<PitchType> PitchTypes { get; set; }
		public DbSet<PitchResultType> PitchResultTypes { get; set; }
		public DbSet<ReviewReasonType> ReviewReasonTypes { get; set; }
		public DbSet<SkyType> SkyTypes { get; set; }
		public DbSet<StandingsType> StandingsTypes { get; set; }
		public DbSet<WindType> WindTypes { get; set; }

		public DbSet<RefUmpireType> UmpireTypes { get; set; }
		public DbSet<RefGamePlayFieldingCreditType> GamePlayFieldingCreditTypes { get; set; }

		public DbSet<Venue> Venues { get; set; }
		public DbSet<Association> Associations { get; set; }
		public DbSet<League> Leagues { get; set; }
		public DbSet<Division> Divisions { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<Umpire> Umpires { get; set; }

		public DbSet<VenueSeason> VenueSeasons { get; set; }
		public DbSet<AssociationSeason> AssociationSeasons { get; set; }
		public DbSet<LeagueSeason> LeagueSeasons { get; set; }
		public DbSet<DivisionSeason> DivisionSeasons { get; set; }
		public DbSet<TeamSeason> TeamSeasons { get; set; }
		public DbSet<PlayerTeamSeason> PlayerTeamSeasons { get; set; }

		public DbSet<Game> Games { get; set; }
		public DbSet<GamePlay> GamePlays { get; set; }
		public DbSet<GamePlayRunner> GamePlayRunners { get; set; }
		public DbSet<GamePlayFieldingCredit> GamePlayFieldingCredits { get; set; }
		public DbSet<GamePlayPitch> GamePlayPitches { get; set; }
		public DbSet<GamePlayAction> GamePlayActions { get; set; }
		public DbSet<GamePlayPickoff> GamePlayPickoffs { get; set; }
		public DbSet<UmpireAssignment> UmpireAssignments { get; set; }
		public DbSet<PlayerHittingBoxscore> PlayerHittingBoxscores { get; set; }
		public DbSet<PlayerPitchingBoxscore> PlayerPitchingBoxscores { get; set; }
		public DbSet<PlayerFieldingBoxscore> PlayerFieldingBoxscores { get; set; }
		public DbSet<PlayerGameBoxscore> PlayerGameBoxscores { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<StatType>().HasKey(st => st.StatTypeID).Property(st => st.StatTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<GameEventType>().HasKey(t => t.GameEventTypeID).Property(t => t.GameEventTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<GameStatusType>().HasKey(t => t.GameStatusTypeID).Property(t => t.GameStatusTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<GameSituationType>().HasKey(t => t.GameSituationTypeID).Property(t => t.GameSituationTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<HitTrajectoryType>().HasKey(t => t.HitTrajectoryTypeID).Property(t => t.HitTrajectoryTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<PitchType>().HasKey(t => t.PitchTypeID).Property(t => t.PitchTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<PitchResultType>().HasKey(t => t.PitchResultTypeID).Property(t => t.PitchResultTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<JobType>().HasKey(t => t.JobTypeID).Property(t => t.JobTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<SkyType>().HasKey(t => t.SkyTypeID).Property(t => t.SkyTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<WindType>().HasKey(t => t.WindTypeID).Property(t => t.WindTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<StandingsType>().HasKey(t => t.StandingsTypeID).Property(t => t.StandingsTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<ReviewReasonType>().HasKey(t => t.ReviewReasonTypeID).Property(t => t.ReviewReasonTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<GameType>().HasKey(t => t.GameTypeID).Property(t => t.GameTypeID).HasMaxLength(1).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			modelBuilder.Entity<Position>().HasKey(p => p.PositionAbbr).Property(p => p.PositionAbbr).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<Position>().Property(p => p.PositionAbbr).HasMaxLength(4);

			modelBuilder.Entity<RefUmpireType>().ToTable("UmpireTypes");
			modelBuilder.Entity<RefUmpireType>().HasKey(ut => ut.UmpireType).Property(ut => ut.UmpireType).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			modelBuilder.Entity<RefGamePlayFieldingCreditType>().ToTable("GamePlayFieldingCreditType");
			modelBuilder.Entity<RefGamePlayFieldingCreditType>().HasKey(x => x.CreditType).Property(x => x.CreditType).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			modelBuilder.Entity<Venue>().HasKey(v => v.VenueID).Property(v => v.VenueID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<Association>().HasKey(a => a.AssociationID).Property(a => a.AssociationID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<League>().HasKey(lg => lg.LeagueID).Property(lg => lg.LeagueID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<Division>().HasKey(d => d.DivisionID).Property(d => d.DivisionID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<Team>().HasKey(t => t.TeamID).Property(t => t.TeamID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<Player>().HasKey(p => p.PlayerID).Property(p => p.PlayerID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<Umpire>().HasKey(u => u.UmpireID).Property(u => u.UmpireID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			modelBuilder.Entity<VenueSeason>().HasKey(vs => new { vs.VenueID, vs.Season });
			modelBuilder.Entity<VenueSeason>().HasRequired(vs => vs.Venue).WithMany(v => v.VenueSeasons).HasForeignKey(vs => vs.VenueID).WillCascadeOnDelete(false);

			modelBuilder.Entity<AssociationSeason>().HasKey(a => new { a.AssociationID, a.Season });
			modelBuilder.Entity<AssociationSeason>().HasRequired(a => a.Association).WithMany(a => a.AssociationSeasons).HasForeignKey(a => a.AssociationID).WillCascadeOnDelete(false);

			modelBuilder.Entity<LeagueSeason>().HasKey(ls => new { ls.LeagueID, ls.Season });
			modelBuilder.Entity<LeagueSeason>().HasRequired(ls => ls.League).WithMany(lg => lg.LeagueSeasons).HasForeignKey(ls => ls.LeagueID).WillCascadeOnDelete(false);
			modelBuilder.Entity<LeagueSeason>().HasRequired(ls => ls.AssociationSeason).WithMany(a => a.LeagueSeasons).HasForeignKey(ls => new { ls.AssociationID, ls.Season }).WillCascadeOnDelete(false);

			modelBuilder.Entity<DivisionSeason>().HasKey(ds => new { ds.DivisionID, ds.Season });
			modelBuilder.Entity<DivisionSeason>().HasRequired(ds => ds.Division).WithMany(d => d.DivisionSeasons).HasForeignKey(ds => ds.DivisionID).WillCascadeOnDelete(false);
			modelBuilder.Entity<DivisionSeason>().HasRequired(ds => ds.LeagueSeason).WithMany(ls => ls.DivisionSeasons).HasForeignKey(ds => new { ds.LeagueID, ds.Season }).WillCascadeOnDelete(false);

			modelBuilder.Entity<TeamSeason>().HasKey(ts => new { ts.TeamID, ts.Season });
			modelBuilder.Entity<TeamSeason>().HasRequired(ts => ts.Team).WithMany(t => t.TeamSeasons).HasForeignKey(ts => ts.TeamID).WillCascadeOnDelete(false);
			modelBuilder.Entity<TeamSeason>().HasOptional(ts => ts.AssociationSeason).WithMany(a => a.TeamSeasons).HasForeignKey(ts => new { ts.AssociationID, ts.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<TeamSeason>().HasOptional(ts => ts.LeagueSeason).WithMany(lg => lg.TeamSeasons).HasForeignKey(ts => new { ts.LeagueID, ts.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<TeamSeason>().HasOptional(ts => ts.DivisionSeason).WithMany(lg => lg.TeamSeasons).HasForeignKey(ts => new { ts.DivisionID, ts.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<TeamSeason>().HasOptional(ts => ts.SpringLeagueSeason).WithMany(t => t.SpringTeamSeasons).HasForeignKey(ts => new { ts.SpringLeagueID, ts.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<TeamSeason>().HasOptional(ts => ts.ParentOrgSeason).WithMany(t => t.ChildOrgSeasons).HasForeignKey(ts => new { ts.ParentOrgID, ts.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<TeamSeason>().HasOptional(ts => ts.VenueSeason).WithMany(v => v.TeamSeasons).HasForeignKey(ts => new { ts.VenueID, ts.Season }).WillCascadeOnDelete(false);

			modelBuilder.Entity<PlayerTeamSeason>().HasKey(pts => new { pts.PlayerID, pts.TeamID, pts.Season });
			modelBuilder.Entity<PlayerTeamSeason>().HasRequired(pts => pts.Player).WithMany(p => p.PlayerTeamSeasons).HasForeignKey(pts => pts.PlayerID).WillCascadeOnDelete(false);
			modelBuilder.Entity<PlayerTeamSeason>().HasRequired(pts => pts.TeamSeason).WithMany(ts => ts.PlayerTeamSeasons).HasForeignKey(pts => new { pts.TeamID, pts.Season }).WillCascadeOnDelete(false);

			modelBuilder.Entity<UmpireAssignment>().HasKey(us => new { us.GameID, us.UmpireID });
			modelBuilder.Entity<UmpireAssignment>().HasRequired(ua => ua.Umpire).WithMany(u => u.UmpireAssignments).HasForeignKey(ua => ua.UmpireID).WillCascadeOnDelete(false);
			modelBuilder.Entity<UmpireAssignment>().HasRequired(ua => ua.Game).WithMany(g => g.UmpireAssignments).HasForeignKey(ua => ua.GameID).WillCascadeOnDelete(false);
			modelBuilder.Entity<UmpireAssignment>().HasRequired(ua => ua.RefUmpireType).WithMany(rut => rut.UmpireAssignments).HasForeignKey(ua => ua.UmpireType).WillCascadeOnDelete(false);

			modelBuilder.Entity<Game>().HasKey(g => g.GameID).Property(g => g.GameID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<Game>().HasRequired(g => g.AssociationSeason).WithMany(a => a.Games).HasForeignKey(g => new { g.AssociationID, g.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<Game>().HasOptional(g => g.HomeTeamSeason).WithMany(t => t.HomeGames).HasForeignKey(g => new { g.HomeTeamID, g.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<Game>().HasOptional(g => g.AwayTeamSeason).WithMany(t => t.AwayGames).HasForeignKey(g => new { g.AwayTeamID, g.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<Game>().HasOptional(g => g.VenueSeason).WithMany(v => v.Games).HasForeignKey(g => new { g.VenueID, g.Season }).WillCascadeOnDelete(false);

			modelBuilder.Entity<GamePlay>().HasKey(g => g.GamePlayID).Property(g => g.GamePlayID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<GamePlay>().HasRequired(g => g.Game).WithMany(g => g.Plays).HasForeignKey(g => g.GameID).WillCascadeOnDelete(false);
			modelBuilder.Entity<GamePlay>().HasRequired(g => g.Batter).WithMany(p => p.BatterPlays).HasForeignKey(g => g.BatterID).WillCascadeOnDelete(false);
			modelBuilder.Entity<GamePlay>().HasRequired(g => g.Pitcher).WithMany(p => p.PitcherPlays).HasForeignKey(g => g.PitcherID).WillCascadeOnDelete(false);

			modelBuilder.Entity<GamePlayRunner>().HasKey(g => g.GamePlayRunnerID).Property(r => r.GamePlayRunnerID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<GamePlayRunner>().HasRequired(g => g.GamePlay).WithMany(g => g.Runners).HasForeignKey(g => g.GamePlayID).WillCascadeOnDelete(false);
			modelBuilder.Entity<GamePlayRunner>().HasRequired(g => g.Runner).WithMany(p => p.PlaysOnBase).HasForeignKey(g => g.RunnerID).WillCascadeOnDelete(false);
			modelBuilder.Entity<GamePlayRunner>().HasOptional(g => g.PitcherResponsible).WithMany(p => p.PitcherResponsibleRunners).HasForeignKey(g => g.PitcherResponsibleID).WillCascadeOnDelete(false);

			modelBuilder.Entity<GamePlayFieldingCredit>().HasKey(x => x.FieldingCreditID).Property(x => x.FieldingCreditID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<GamePlayFieldingCredit>().HasRequired(x => x.PlayRunner).WithMany(x => x.FieldingCredits).HasForeignKey(x => x.PlayRunnerID).WillCascadeOnDelete(false);
			modelBuilder.Entity<GamePlayFieldingCredit>().HasRequired(x => x.Fielder).WithMany(x => x.FieldingCredits).HasForeignKey(x => x.FielderID).WillCascadeOnDelete(false);
			modelBuilder.Entity<GamePlayFieldingCredit>().HasRequired(x => x.RefCreditType).WithMany(x => x.FieldingCredits).HasForeignKey(x => x.CreditType).WillCascadeOnDelete(false);

			modelBuilder.Entity<GamePlayPitch>().HasKey(p => new { p.GamePlayID, p.GamePlayEventIndex });
			modelBuilder.Entity<GamePlayPitch>().HasRequired(p => p.GamePlay).WithMany(p => p.Pitches).HasForeignKey(p => p.GamePlayID).WillCascadeOnDelete(false);
			modelBuilder.Entity<GamePlayPitch>().HasRequired(p => p.Batter).WithMany(p => p.PitchesFaced).HasForeignKey(p => p.BatterID).WillCascadeOnDelete(false);
			modelBuilder.Entity<GamePlayPitch>().HasRequired(p => p.Pitcher).WithMany(p => p.PitchesThrown).HasForeignKey(p => p.PitcherID).WillCascadeOnDelete(false);

			modelBuilder.Entity<GamePlayAction>().HasKey(p => new { p.GamePlayID, p.GamePlayEventIndex });
			modelBuilder.Entity<GamePlayAction>().HasRequired(p => p.GamePlay).WithMany(p => p.Actions).HasForeignKey(p => p.GamePlayID).WillCascadeOnDelete(false);
			modelBuilder.Entity<GamePlayAction>().HasOptional(p => p.ActionTaker).WithMany(p => p.ActionsTaken).HasForeignKey(p => p.ActionTakerID).WillCascadeOnDelete(false);

			modelBuilder.Entity<GamePlayPickoff>().HasKey(p => new { p.GamePlayID, p.GamePlayEventIndex });
			modelBuilder.Entity<GamePlayPickoff>().HasRequired(p => p.GamePlay).WithMany(p => p.Pickoffs).HasForeignKey(p => p.GamePlayID).WillCascadeOnDelete(false);

			modelBuilder.Entity<PlayerHittingBoxscore>().HasKey(box => new { box.GameID, box.PlayerID });
			modelBuilder.Entity<PlayerHittingBoxscore>().HasRequired(box => box.Game).WithMany(g => g.PlayerHittingBoxscores).HasForeignKey(box => box.GameID).WillCascadeOnDelete(false);
			modelBuilder.Entity<PlayerHittingBoxscore>().HasRequired(box => box.PlayerTeamSeason).WithMany(g => g.PlayerHittingBoxscores).HasForeignKey(box => new { box.PlayerID, box.TeamID, box.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<PlayerHittingBoxscore>().HasOptional(box => box.Position).WithMany(p => p.PlayerHittingBoxscores).HasForeignKey(box => box.PosAbbr).WillCascadeOnDelete(false);

			modelBuilder.Entity<PlayerPitchingBoxscore>().HasKey(box => new { box.GameID, box.PlayerID });
			modelBuilder.Entity<PlayerPitchingBoxscore>().HasRequired(box => box.Game).WithMany(g => g.PlayerPitchingBoxscores).HasForeignKey(box => box.GameID).WillCascadeOnDelete(false);
			modelBuilder.Entity<PlayerPitchingBoxscore>().HasRequired(box => box.PlayerTeamSeason).WithMany(g => g.PlayerPitchingBoxscores).HasForeignKey(box => new { box.PlayerID, box.TeamID, box.Season }).WillCascadeOnDelete(false);

			modelBuilder.Entity<PlayerFieldingBoxscore>().HasKey(box => new { box.GameID, box.PlayerID });
			modelBuilder.Entity<PlayerFieldingBoxscore>().HasRequired(box => box.Game).WithMany(g => g.PlayerFieldingBoxscores).HasForeignKey(box => box.GameID).WillCascadeOnDelete(false);
			modelBuilder.Entity<PlayerFieldingBoxscore>().HasRequired(box => box.PlayerTeamSeason).WithMany(g => g.PlayerFieldingBoxscores).HasForeignKey(box => new { box.PlayerID, box.TeamID, box.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<PlayerFieldingBoxscore>().HasOptional(box => box.Position).WithMany(p => p.PlayerFieldingBoxscores).HasForeignKey(box => box.PosAbbr).WillCascadeOnDelete(false);

			modelBuilder.Entity<PlayerGameBoxscore>().HasKey(box => new { box.GameID, box.PlayerID });
			modelBuilder.Entity<PlayerGameBoxscore>().HasRequired(box => box.Game).WithMany(g => g.PlayerGameBoxscores).HasForeignKey(box => box.GameID).WillCascadeOnDelete(false);
			modelBuilder.Entity<PlayerGameBoxscore>().HasRequired(box => box.PlayerTeamSeason).WithMany(g => g.PlayerGameBoxscores).HasForeignKey(box => new { box.PlayerID, box.TeamID, box.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<PlayerGameBoxscore>().HasOptional(box => box.Position).WithMany(p => p.PlayerGameBoxscores).HasForeignKey(box => box.PosAbbr).WillCascadeOnDelete(false);
		}

		public override int SaveChanges()
		{
			var now = DateTime.Now;
			var addedEntities = ChangeTracker.Entries<MlbStatsEntity>().Where(e => e.State == EntityState.Added).ToList();

			addedEntities.ForEach(e =>
			{
				e.Entity.DateCreated = now;
				e.Entity.DateModified = now;
			});

			var modifiedEntities = ChangeTracker.Entries<MlbStatsEntity>().Where(e => e.State == EntityState.Modified).ToList();

			modifiedEntities.ForEach(e =>
			{
				e.Entity.DateModified = now;
			});

			return base.SaveChanges();
		}

		public void DetachAllEntities()
		{
			var changedEntriesCopy = this.ChangeTracker.Entries()
				.Where(e => e.State == EntityState.Added ||
							e.State == EntityState.Modified ||
							e.State == EntityState.Deleted)
				.ToList();

			foreach (var entry in changedEntriesCopy)
				entry.State = EntityState.Detached;
		}
	}
}
