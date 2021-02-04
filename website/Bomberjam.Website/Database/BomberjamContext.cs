using System;
using System.Globalization;
using System.Threading.Tasks;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bomberjam.Website.Database
{
    public sealed class BomberjamContext : DbContext
    {
        public BomberjamContext(DbContextOptions<BomberjamContext> options)
            : base(options)
        {
            this.ChangeTracker.Tracked += ChangeTrackerOnTracked;
            this.ChangeTracker.StateChanged += ChangeTrackerOnStateChanged;
        }

        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbGame> Games { get; set; }
        public DbSet<DbGameUser> GameUsers { get; set; }
        public DbSet<DbQueuedTask> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbGameUser>().HasKey(x => new { GameID = x.GameId, UserID = x.UserId });

            modelBuilder.Entity<DbUser>().HasIndex(x => x.Email).IsUnique();

            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Status);
            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Type);
            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Created);
            modelBuilder.Entity<DbQueuedTask>().Property(x => x.Type).HasConversion<int>();
            modelBuilder.Entity<DbQueuedTask>().Property(x => x.Status).HasConversion<int>();

            modelBuilder.Entity<DbUser>().HasData(
                CreateInitialUser(1, "Askaiser", "simmon.anthony@gmail.com"),
                CreateInitialUser(2, "Falgar", "falgar@gmail.com"),
                CreateInitialUser(3, "Xenure", "xenure@gmail.com"),
                CreateInitialUser(4, "Minty", "minty@gmail.com"));

            modelBuilder.Entity<DbQueuedTask>().HasData(
                CreateInitialCompileTask(1, 1),
                CreateInitialCompileTask(2, 2),
                CreateInitialCompileTask(3, 3),
                CreateInitialCompileTask(4, 4));
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
                timestampable.Created = DateTime.UtcNow;
                timestampable.Updated = DateTime.UtcNow;
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

        private static DbUser CreateInitialUser(int id, string username, string email) => new DbUser
        {
            Id = id,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Username = username,
            Email = email,
            SubmitCount = 1,
            GameCount = 0,
            IsCompiling = false,
            IsCompiled = false,
            CompilationErrors = string.Empty,
            BotLanguage = string.Empty
        };

        private static DbQueuedTask CreateInitialCompileTask(int taskId, int userId) => new DbQueuedTask
        {
            Id = taskId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Type = QueuedTaskType.Compile,
            Data = userId.ToString(CultureInfo.InvariantCulture),
            Status = QueuedTaskStatus.Created,

        };
    }
}