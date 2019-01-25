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
		public DbSet<GameStatusType> GameStatusTypes { get; set; }
		public DbSet<GameType> GameTypes { get; set; }
		public DbSet<HitTrajectoryType> HitTrajectoryTypes { get; set; }
		public DbSet<JobType> JobTypes { get; set; }
		public DbSet<PitchResultType> PitchResultTypes { get; set; }
		public DbSet<Venue> Venues { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<StatType>().HasKey(st => st.StatTypeID).Property(st => st.StatTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<GameEventType>().HasKey(t => t.GameEventTypeID).Property(t => t.GameEventTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<GameStatusType>().HasKey(t => t.GameStatusTypeID).Property(t => t.GameStatusTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<HitTrajectoryType>().HasKey(t => t.HitTrajectoryTypeID).Property(t => t.HitTrajectoryTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<PitchResultType>().HasKey(t => t.PitchResultTypeID).Property(t => t.PitchResultTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<JobType>().HasKey(t => t.JobTypeID).Property(t => t.JobTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<Position>().HasKey(p => p.PositionAbbr).Property(p => p.PositionAbbr).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			modelBuilder.Entity<GameType>().HasKey(t => t.GameTypeID).Property(t => t.GameTypeID).HasMaxLength(1).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			modelBuilder.Entity<Venue>().HasKey(v => v.VenueId).Property(v => v.VenueId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

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
