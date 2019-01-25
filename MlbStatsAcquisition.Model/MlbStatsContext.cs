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
		public DbSet<StatType> StatTypes { get; set; }
		public DbSet<Venue> Venues { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Venue>().HasKey(v => v.VenueId).Property(v=>v.VenueId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			modelBuilder.Entity<StatType>().HasKey(st => st.StatTypeID).Property(st=>st.StatTypeID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
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
