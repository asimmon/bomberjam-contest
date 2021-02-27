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
        private const string TablePrefix = "App_";

        private static readonly DbUser[] InitialTestUsers =
        {
            CreateInitialUser(Constants.UserAskaiserId, 14242083, "Askaiser", "simmon.anthony@gmail.com"),
            CreateInitialUser(Constants.UserFalgarId, 36072624, "Falgar", "falgar@gmail.com"),
            CreateInitialUser(Constants.UserXenureId, 9208753, "Xenure", "xenure@gmail.com"),
            CreateInitialUser(Constants.UserMintyId, 26142591, "Minty", "minty@gmail.com"),
            CreateInitialUser(Constants.UserKalmeraId, 5122918, "Kalmera", "kalmera@gmail.com"),
            CreateInitialUser(Constants.UserPandarfId, 1035273, "Pandarf", "pandarf@gmail.com"),
            CreateInitialUser(Constants.UserMireId, 5489330, "Mire", "mire@gmail.com")
        };

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(TablePrefix + entityType.GetTableName());
            }

            // Indexes
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

            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Status);
            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Type);
            modelBuilder.Entity<DbQueuedTask>().HasIndex(x => x.Created);
            modelBuilder.Entity<DbQueuedTask>().Property(x => x.Type).HasConversion<int>();
            modelBuilder.Entity<DbQueuedTask>().Property(x => x.Status).HasConversion<int>();

            // Foreign keys with specific behavior
            modelBuilder.Entity<DbGameUser>().HasOne(gu => gu.Bot).WithMany().OnDelete(DeleteBehavior.NoAction);

            // TEST DATA
            modelBuilder.Entity<DbUser>().HasData(InitialTestUsers);

            var initialBots = InitialTestUsers.Select(u => CreateInitialBot(u.Id)).ToList();
            modelBuilder.Entity<DbBot>().HasData(initialBots);

            var initialCompileTasks = initialBots.Select(b => CreateInitialCompileTask(Guid.NewGuid(), b.Id)).ToList();
            modelBuilder.Entity<DbQueuedTask>().HasData(initialCompileTasks);
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

        private static DbUser CreateInitialUser(Guid id, int githubId, string username, string email) => new DbUser
        {
            Id = id,
            GithubId = githubId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            UserName = username,
            Email = email,
            Points = Constants.InitialPoints
        };

        private static DbBot CreateInitialBot(Guid userId) => new DbBot
        {
            Id = userId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            UserId = userId,
            Status = CompilationStatus.NotCompiled,
            Language = string.Empty,
            Errors = string.Empty
        };

        private static DbQueuedTask CreateInitialCompileTask(Guid taskId, Guid botId) => new DbQueuedTask
        {
            Id = taskId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Type = QueuedTaskType.Compile,
            Data = botId.ToString("D"),
            Status = QueuedTaskStatus.Created,
        };
    }
}