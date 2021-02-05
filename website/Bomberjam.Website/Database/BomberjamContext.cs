using System;
using System.Threading.Tasks;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bomberjam.Website.Database
{
    public sealed class BomberjamContext : DbContext
    {
        private static readonly DbUser UserAskaiser = CreateInitialUser(new Guid("00000000-0000-0000-0000-000000000001"), "Askaiser", "simmon.anthony@gmail.com");
        private static readonly DbUser UserFalgar = CreateInitialUser(new Guid("00000000-0000-0000-0000-000000000002"), "Falgar", "falgar@gmail.com");
        private static readonly DbUser UserXenure = CreateInitialUser(new Guid("00000000-0000-0000-0000-000000000003"), "Xenure", "xenure@gmail.com");
        private static readonly DbUser UserMinty = CreateInitialUser(new Guid("00000000-0000-0000-0000-000000000004"), "Minty", "minty@gmail.com");

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

            modelBuilder.Entity<DbUser>().HasData(UserAskaiser, UserFalgar, UserXenure, UserMinty);

            modelBuilder.Entity<DbQueuedTask>().HasData(
                CreateInitialCompileTask(Guid.NewGuid(), UserAskaiser.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserFalgar.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserXenure.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserMinty.Id));
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

        private static DbUser CreateInitialUser(Guid id, string username, string email) => new DbUser
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

        private static DbQueuedTask CreateInitialCompileTask(Guid taskId, Guid userId) => new DbQueuedTask
        {
            Id = taskId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Type = QueuedTaskType.Compile,
            Data = userId.ToString("D"),
            Status = QueuedTaskStatus.Created,
        };
    }
}