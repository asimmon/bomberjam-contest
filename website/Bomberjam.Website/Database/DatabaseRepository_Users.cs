using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MethodHasAsyncOverload
namespace Bomberjam.Website.Database
{
    public partial class DatabaseRepository : IBomberjamRepository
    {
        private readonly BomberjamContext _dbContext;

        public DatabaseRepository(BomberjamContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await this._dbContext.Users.Select(u => MapUser(u)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<User> GetUserByGithubId(int githubId)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(u => u.GithubId == githubId).ConfigureAwait(false);
            if (dbUser == null)
                throw new EntityNotFound(ModelType.User, githubId);

            return MapUser(dbUser);
        }

        public async Task<User> GetUserById(Guid id)
        {
            var dbUser = await this._dbContext.Users.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
            if (dbUser == null)
                throw new EntityNotFound(ModelType.User, id);

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
                throw new EntityNotFound(ModelType.User, changedUser.Id);

            if (!string.IsNullOrWhiteSpace(changedUser.UserName))
                dbUser.UserName = changedUser.UserName;

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<ICollection<RankedUser>> GetRankedUsers()
        {
            return await this._dbContext.Users
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
            Points = u.Points
        };
    }
}