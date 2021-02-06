using System;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Bomberjam.Website.Database
{
    public sealed class BomberjamContext : DbContext
    {
        private static readonly DbUser UserAskaiser = CreateInitialUser(Constants.UserAskaiserId, "Askaiser", "simmon.anthony@gmail.com");
        private static readonly DbUser UserFalgar = CreateInitialUser(Constants.UserFalgarId, "Falgar", "falgar@gmail.com");
        private static readonly DbUser UserXenure = CreateInitialUser(Constants.UserXenureId, "Xenure", "xenure@gmail.com");
        private static readonly DbUser UserMinty = CreateInitialUser(Constants.UserMintyId, "Minty", "minty@gmail.com");
        private static readonly DbUser UserKalmera = CreateInitialUser(Constants.UserKalmeraId, "Kalmera", "kalmera@gmail.com");
        private static readonly DbUser UserPandarf = CreateInitialUser(Constants.UserPandarfId, "Pandarf", "pandarf@gmail.com");
        private static readonly DbUser UserMire = CreateInitialUser(Constants.UserMireId, "Mire", "mire@gmail.com");

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

            modelBuilder.Entity<DbUser>().HasData(UserAskaiser, UserFalgar, UserXenure, UserMinty, UserKalmera, UserPandarf, UserMire);

            modelBuilder.Entity<DbQueuedTask>().HasData(
                CreateInitialCompileTask(Guid.NewGuid(), UserAskaiser.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserFalgar.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserXenure.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserMinty.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserKalmera.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserPandarf.Id),
                CreateInitialCompileTask(Guid.NewGuid(), UserMire.Id));

            modelBuilder.Entity<DbQueuedTask>().HasData(
                CreateInitialGameTask(Guid.NewGuid(), UserAskaiser, UserPandarf, UserXenure, UserFalgar),
                CreateInitialGameTask(Guid.NewGuid(), UserMire, UserKalmera, UserXenure, UserFalgar),
                CreateInitialGameTask(Guid.NewGuid(), UserMinty, UserPandarf, UserKalmera, UserAskaiser),
                CreateInitialGameTask(Guid.NewGuid(), UserFalgar, UserAskaiser, UserXenure, UserKalmera));
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

        private static DbQueuedTask CreateInitialGameTask(Guid taskId, params DbUser[] users) => new DbQueuedTask
        {
            Id = taskId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Type = QueuedTaskType.Game,
            Data = string.Join(",", users.Select(u => $"{u.Id:D}:{u.Username}")),
            Status = QueuedTaskStatus.Created,
        };
    }
}