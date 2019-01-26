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
			modelBuilder.Entity<Team>().HasOptional(t => t.Venue).WithMany(lg => lg.CurrentTeams).HasForeignKey(t => t.VenueID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Team>().HasOptional(t => t.SpringLeague).WithMany(t => t.SpringTeams).HasForeignKey(t => t.SpringLeagueID).WillCascadeOnDelete(false);
			modelBuilder.Entity<Team>().HasOptional(t => t.ParentOrg).WithMany(t => t.ChildTeams).HasForeignKey(t => t.ParentOrgID).WillCascadeOnDelete(false);
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
	}
}
