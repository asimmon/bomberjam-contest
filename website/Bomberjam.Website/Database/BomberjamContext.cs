using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bomberjam.Website.Database
{
    public sealed class BomberjamContext : DbContext
    {
        private const int FirstSeasonId = 0;
        private const string TablePrefix = "App_";

        public BomberjamContext(DbContextOptions<BomberjamContext> options)
            : base(options)
        {
            this.ChangeTracker.Tracked += ChangeTrackerOnTracked;
            this.ChangeTracker.StateChanged += ChangeTrackerOnStateChanged;
        }

        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbBot> Bots { get; set; }
        public DbSet<DbGame> Games { get; set; }
        public DbSet<DbGameUser> GameUsers { get; set; }
        public DbSet<DbQueuedTask> Tasks { get; set; }
        public DbSet<DbWorker> Workers { get; set; }
        public DbSet<DbSeason> Seasons { get; set; }
        public DbSet<DbSeasonSummary> SeasonSummaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(TablePrefix + entityType.GetTableName());
            }

            modelBuilder.Entity<DbGameUser>().HasKey(x => new { GameID = x.GameId, UserID = x.UserId });

            modelBuilder.Entity<DbUser>().HasIndex(x => x.GithubId).IsUnique();
            modelBuilder.Entity<DbUser>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<DbUser>().HasIndex(x => x.UserName).IsUnique();
            modelBuilder.Entity<DbUser>().HasIndex(x => x.Points);

            modelBuilder.Entity<DbBot>().HasIndex(x => x.Created);
            modelBuilder.Entity<DbBot>().HasIndex(x => x.Updated);
            modelBuilder.Entity<DbBot>().HasIndex(x => x.Status);
            modelBuilder.Entity<DbBot>().Property(x => x.Status).HasConversion<int>();

            modelBuilder.Entity<DbGame>().HasIndex(x => x.Created);
            modelBuilder.Entity<DbGame>().HasIndex(x => x.Origin);
            modelBuilder.Entity<DbGame>().Property(x => x.Origin).HasConversion<int>();
            modelBuilder.Entity<DbGame>().Property(x => x.SeasonId).HasDefaultValue(FirstSeasonId);

            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Status);
            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Type);
            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Created);
            modelBuilder.Entity<DbQueuedTask>().Property(x => x.Type).HasConversion<int>();
            modelBuilder.Entity<DbQueuedTask>().Property(x => x.Status).HasConversion<int>();

            modelBuilder.Entity<DbWorker>().HasIndex(x => x.Created);

            modelBuilder.Entity<DbSeasonSummary>().HasKey(x => new { UserId = x.UserId, SeasonId = x.SeasonId });

            // Foreign keys with specific behavior
            modelBuilder.Entity<DbGameUser>().HasOne(gu => gu.Bot).WithMany().OnDelete(DeleteBehavior.NoAction);

            // Initial season
            modelBuilder.Entity<DbSeason>().HasData(new DbSeason
            {
                Id = FirstSeasonId,
                Created = DateTime.Parse("2021-03-01T00:00:00.0000000Z"),
                Updated = DateTime.Parse("2021-03-01T00:00:00.0000000Z"),
                Title = "S" + FirstSeasonId.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')
            });
        }

        private static void ChangeTrackerOnTracked(object sender, EntityTrackedEventArgs e)
        {
            if (e.FromQuery)
                return;

            UpdateTimestampableObjects(e.Entry);
        }

        private static void ChangeTrackerOnStateChanged(object sender, EntityStateChangedEventArgs e)
        {
            UpdateTimestampableObjects(e.Entry);
        }

        private static void UpdateTimestampableObjects(EntityEntry entry)
        {
            if (entry.Entity is ITimestampable timestampable)
                UpdateTimestampableObjects(entry.State, timestampable);
        }

        private static void UpdateTimestampableObjects(EntityState state, ITimestampable timestampable)
        {
            if (state == EntityState.Added)
            {
                var utcNow = DateTime.UtcNow;
                timestampable.Created = utcNow;
                timestampable.Updated = utcNow;
            }
            else if (state == EntityState.Modified)
            {
                timestampable.Updated = DateTime.UtcNow;
            }
        }

        public override void Dispose()
        {
            this.ChangeTracker.Tracked -= ChangeTrackerOnTracked;
            this.ChangeTracker.StateChanged -= ChangeTrackerOnStateChanged;
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            this.ChangeTracker.Tracked -= ChangeTrackerOnTracked;
            this.ChangeTracker.StateChanged -= ChangeTrackerOnStateChanged;
            return base.DisposeAsync();
        }
    }
}