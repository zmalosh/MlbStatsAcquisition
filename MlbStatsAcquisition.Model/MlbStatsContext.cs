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

		public DbSet<Venue> Venues { get; set; }
		public DbSet<Association> Associations { get; set; }
		public DbSet<League> Leagues { get; set; }
		public DbSet<Division> Divisions { get; set; }
		public DbSet<Team> Teams { get; set; }

		public DbSet<VenueSeason> VenueSeasons { get; set; }
		public DbSet<AssociationSeason> AssociationSeasons { get; set; }
		public DbSet<LeagueSeason> LeagueSeasons { get; set; }
		public DbSet<DivisionSeason> DivisionSeasons { get; set; }
		public DbSet<TeamSeason> TeamSeasons { get; set; }

		public DbSet<Game> Games { get; set; }

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
			modelBuilder.Entity<Position>().HasKey(p => p.PositionAbbr).Property(p => p.PositionAbbr).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<GameType>().HasKey(t => t.GameTypeID).Property(t => t.GameTypeID).HasMaxLength(1).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			modelBuilder.Entity<Venue>().HasKey(v => v.VenueID).Property(v => v.VenueID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			modelBuilder.Entity<Association>().HasKey(a => a.AssociationID).Property(a => a.AssociationID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			modelBuilder.Entity<League>().HasKey(lg => lg.LeagueID).Property(lg => lg.LeagueID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<League>().HasRequired(lg => lg.Association).WithMany(a => a.Leagues).HasForeignKey(lg => lg.AssociationID).WillCascadeOnDelete(false);

			modelBuilder.Entity<Division>().HasKey(d => d.DivisionID).Property(d => d.DivisionID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<Division>().HasRequired(d => d.League).WithMany(lg => lg.Divisions).HasForeignKey(d => d.LeagueID).WillCascadeOnDelete(false);

			modelBuilder.Entity<Team>().HasKey(t => t.TeamID).Property(t => t.TeamID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<Team>().HasOptional(t => t.Association).WithMany(a => a.Teams).HasForeignKey(t => t.AssociationID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Team>().HasOptional(t => t.League).WithMany(lg => lg.Teams).HasForeignKey(t => t.LeagueID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Team>().HasOptional(t => t.Division).WithMany(lg => lg.Teams).HasForeignKey(t => t.DivisionID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Team>().HasOptional(t => t.Venue).WithMany(lg => lg.Teams).HasForeignKey(t => t.VenueID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Team>().HasOptional(t => t.SpringLeague).WithMany(t => t.SpringTeams).HasForeignKey(t => t.SpringLeagueID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Team>().HasOptional(t => t.ParentOrg).WithMany(t => t.ChildOrgTeams).HasForeignKey(t => t.ParentOrgID).WillCascadeOnDelete(false);

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

			modelBuilder.Entity<Game>().HasKey(g => g.GameID).Property(g => g.GameID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<Game>().HasRequired(g => g.Association).WithMany(a => a.Games).HasForeignKey(g => g.AssociationID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Game>().HasOptional(g => g.HomeTeam).WithMany(t => t.HomeGames).HasForeignKey(g => g.HomeTeamID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Game>().HasOptional(g => g.AwayTeam).WithMany(t => t.AwayGames).HasForeignKey(g => g.AwayTeamID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Game>().HasOptional(g => g.Venue).WithMany(v => v.Games).HasForeignKey(g => g.VenueID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Game>().HasRequired(g => g.AssociationSeason).WithMany(a => a.Games).HasForeignKey(g => new { g.AssociationID, g.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<Game>().HasOptional(g => g.HomeTeamSeason).WithMany(t => t.HomeGames).HasForeignKey(g => new { g.HomeTeamID, g.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<Game>().HasOptional(g => g.AwayTeamSeason).WithMany(t => t.AwayGames).HasForeignKey(g => new { g.AwayTeamID, g.Season }).WillCascadeOnDelete(false);
			modelBuilder.Entity<Game>().HasOptional(g => g.VenueSeason).WithMany(v => v.Games).HasForeignKey(g => new { g.VenueID, g.Season }).WillCascadeOnDelete(false);
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
