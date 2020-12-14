using System;
using System.Threading.Tasks;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bomberjam.Website.Database
{
    public sealed class BomberjamContext : DbContext
    {
        public BomberjamContext (DbContextOptions<BomberjamContext> options)
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
    }

    public class DbUser : ITimestampable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int SubmitCount { get; set; }
        public int GameCount { get; set; }
        public bool IsCompiling { get; set; }
        public bool IsCompiled { get; set; }
        public string CompilationErrors { get; set; }
        public string BotLanguage { get; set; }
    }

    public class DbGame : ITimestampable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsFinished { get; set; }
        public int? WinnerId { get; set; }
        public string Errors { get; set; }
    }

    public class DbGameUser : ITimestampable
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public DbGame Game { get; set; }
        public DbUser User { get; set; }
        public string Errors { get; set; }
    }

    public class DbQueuedTask : ITimestampable
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public QueuedTaskType Type { get; set; }
        public QueuedTaskStatus Status { get; set; }
        public string Data { get; set; }
    }

    public interface ITimestampable
    {
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}