using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable MethodHasAsyncOverload
namespace Bomberjam.Website.Database
{
    public partial class DatabaseRepository : IBomberjamRepository
    {
        private readonly BomberjamContext _dbContext;
        private readonly ILogger<DatabaseRepository> _logger;

        public DatabaseRepository(BomberjamContext dbContext, ILogger<DatabaseRepository> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

        public async Task<IEnumerable<User>> GetUsersWithCompiledBot()
        {
            return await this._dbContext.Users
                .Where(u => this._dbContext.Bots.Any(b => b.UserId == u.Id && b.Status == CompilationStatus.CompilationSucceeded))
                .Select(u => MapUser(u))
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<User> GetUserByGithubId(int githubId)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(u => u.GithubId == githubId).ConfigureAwait(false);
            if (dbUser == null)
                throw new EntityNotFound(EntityType.User, githubId);

            return MapUser(dbUser);
        }

        public async Task<User> GetUserById(Guid id)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
            if (dbUser == null)
                throw new EntityNotFound(EntityType.User, id);

            return MapUser(dbUser);
        }

        private static TUser MapUser<TUser>(DbUser dbUser) where TUser : User, new() => new()
        {
            Id = dbUser.Id,
            Created = dbUser.Created,
            Updated = dbUser.Updated,
            GithubId = dbUser.GithubId,
            Email = dbUser.Email,
            UserName = dbUser.UserName,
            Points = dbUser.Points,
            GlobalRank = dbUser.GlobalRank
        };

        private static User MapUser(DbUser dbUser) => MapUser<User>(dbUser);

        public async Task AddUser(int githubId, string email, string username)
        {
            this._dbContext.Users.Add(new DbUser
            {
                GithubId = githubId,
                Email = email,
                UserName = username ?? string.Empty,
                Points = Constants.InitialPoints
            });

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateUser(User changedUser)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(e => e.Id == changedUser.Id).ConfigureAwait(false);
            if (dbUser == null)
                throw new EntityNotFound(EntityType.User, changedUser.Id);

            if (!string.IsNullOrWhiteSpace(changedUser.UserName))
                dbUser.UserName = changedUser.UserName;

            if (!string.IsNullOrWhiteSpace(changedUser.Email))
                dbUser.Email = changedUser.Email;

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<ICollection<RankedUser>> GetRankedUsers()
        {
            return await this._dbContext.Users
                .Where(u => this._dbContext.GameUsers.Any(gu => gu.UserId == u.Id))
                .OrderByDescending(u => u.Points)
                .ThenBy(u => u.Created)
                .Select(u => MapRankedUser(u))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        private static RankedUser MapRankedUser(DbUser u) => new RankedUser
        {
            Id = u.Id,
            UserName = u.UserName,
            Points = u.Points,
            GlobalRank = u.GlobalRank
        };

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var selectQuery =
                from u in this._dbContext.Users
                join b in this._dbContext.Bots on u.Id equals b.UserId into innerJoin
                from leftJoin in innerJoin.DefaultIfEmpty()
                group leftJoin by new { u.Id, u.UserName, u.GithubId, u.Points, u.Email, u.Created, u.Updated }
                into grouped
                orderby grouped.Key.Created descending
                select new User
                {
                    Id = grouped.Key.Id,
                    UserName = grouped.Key.UserName,
                    GithubId = grouped.Key.GithubId,
                    Points = grouped.Key.Points,
                    Email = grouped.Key.Email,
                    Created = grouped.Key.Created,
                    Updated = grouped.Key.Updated,
                    AllBotCount = grouped.Count(b => b != null),
                    CompiledBotCount = grouped.Count(b => b != null && b.Status == CompilationStatus.CompilationSucceeded)
                };

            return await selectQuery.ToListAsync().ConfigureAwait(false);
        }

        public Task<bool> IsUserNameAlreadyUsed(string username)
        {
            return this._dbContext.Users.AnyAsync(u => u.UserName == username);
        }

        public Task<bool> IsUserEmailAlreadyUsed(string email)
        {
            return this._dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task UpdateAllUserGlobalRanks()
        {
            var usersBefore = await this._dbContext.Users
                .Select(u => new
                {
                    u.Id,
                    u.Created,
                    u.Points,
                    u.GlobalRank
                })
                .ToListAsync()
                .ConfigureAwait(false);

            var sortedUsers = usersBefore.OrderByDescending(u => u.Points).ThenBy(u => u.Created).ToList();
            var updatedUserCount = 0;

            for (var i = 0; i < sortedUsers.Count; i++)
            {
                var sortedUser = sortedUsers[i];
                var newGlobalRank = i + 1;

                if (sortedUser.GlobalRank != newGlobalRank)
                {
                    var dbUser = await this._dbContext.Users.FindAsync(sortedUser.Id).ConfigureAwait(false);
                    dbUser.GlobalRank = newGlobalRank;
                    this._logger.LogDebug($" - Updating user {sortedUser.Id} rank from {sortedUser.GlobalRank} to {newGlobalRank}");
                    updatedUserCount++;
                }
            }

            if (updatedUserCount > 0)
            {
                await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}