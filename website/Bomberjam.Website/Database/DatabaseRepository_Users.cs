using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<User> GetUserByGithubId(string githubId)
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
            UserName = dbUser.UserName,
            Organization = dbUser.Organization,
            Points = dbUser.Points,
            GlobalRank = dbUser.GlobalRank
        };

        private static User MapUser(DbUser dbUser) => MapUser<User>(dbUser);

        public async Task<User> AddUser(string githubId, string username)
        {
            this._dbContext.Users.Add(new DbUser
            {
                GithubId = githubId,
                UserName = username ?? string.Empty,
                Organization = string.Empty,
                Points = Constants.InitialPoints
            });

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);

            return await this.GetUserByGithubId(githubId);
        }

        public async Task UpdateUser(User changedUser)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(e => e.Id == changedUser.Id).ConfigureAwait(false);
            if (dbUser == null)
                throw new EntityNotFound(EntityType.User, changedUser.Id);

            if (!string.IsNullOrWhiteSpace(changedUser.UserName))
                dbUser.UserName = changedUser.UserName;

            dbUser.Organization = changedUser.Organization ?? string.Empty;

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<ICollection<RankedUser>> GetRankedUsers()
        {
            return await this._dbContext.Users
                .OrderBy(u => u.GlobalRank)
                .ThenBy(u => u.Created)
                .Select(u => new RankedUser
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Organization = u.Organization,
                    Points = u.Points,
                    GlobalRank = u.GlobalRank,
                    GithubId = u.GithubId,
                    HasCompiledBot = this._dbContext.Bots.Any(b => b.UserId == u.Id && b.Status == CompilationStatus.CompilationSucceeded)
                })
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var selectQuery =
                from u in this._dbContext.Users
                join b in this._dbContext.Bots on u.Id equals b.UserId into innerJoin
                from leftJoin in innerJoin.DefaultIfEmpty()
                group leftJoin by new { u.Id, u.UserName, u.Organization, u.GithubId, u.Points, u.Created, u.Updated, u.GlobalRank }
                into grouped
                orderby grouped.Key.Created descending
                select new User
                {
                    Id = grouped.Key.Id,
                    UserName = grouped.Key.UserName,
                    Organization = grouped.Key.Organization,
                    GithubId = grouped.Key.GithubId,
                    Points = grouped.Key.Points,
                    GlobalRank = grouped.Key.GlobalRank,
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

        public async Task UpdateAllUserGlobalRanks()
        {
            var currentSeason = await this.GetCurrentSeason().ConfigureAwait(false);
            await this.UpdateAllUserGlobalRanks(currentSeason.Id).ConfigureAwait(false);
        }

        public async Task UpdateAllUserGlobalRanks(int seasonId)
        {
            // There's no batch update in EF Core so this will be faster and more efficient
            const string updateAllUserRankFormat =
@"WITH [TmpGameUsers] AS
(
    SELECT gu.[UserId], gu.[GameId]
    FROM [dbo].[App_GameUsers] gu
    INNER JOIN [dbo].[App_Games] g ON g.[Id] = gu.[GameId]
    WHERE g.[SeasonId] = {0}
),
[TmpUsers] AS
(
    SELECT
        [Id], [GlobalRank], [Points], [Created], CASE
            WHEN EXISTS (SELECT 1 FROM [TmpGameUsers] gu WHERE gu.[UserId] = u.Id) THEN 1
            ELSE 0
        END as [HasGames]
    FROM [dbo].[App_Users] u
),
[RankedUsers] AS
(
    SELECT [Id], [GlobalRank], [HasGames], ROW_NUMBER() OVER (ORDER BY [HasGames] DESC, [Points] DESC, [Created] ASC) AS [NewGlobalRank]
    FROM [TmpUsers]
)
UPDATE [RankedUsers]
SET [GlobalRank] = [NewGlobalRank]";

            var updateAllUserRank = string.Format(updateAllUserRankFormat, seasonId.ToString(CultureInfo.InvariantCulture));
            var modifiedUserCount = await this._dbContext.Database.ExecuteSqlRawAsync(updateAllUserRank).ConfigureAwait(false);
            this._logger.LogInformation($"Updated the global rank of {modifiedUserCount} users");
        }
    }
}