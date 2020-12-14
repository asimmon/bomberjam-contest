using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Controllers;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Database
{
    public class CachedRepository : IRepository
    {
        private const string GetUsersCacheKeyFormat = "GetUsers";
        private const string GetUserByEmailCacheKeyFormat = "GetUserByEmail_{0}";
        private const string GetUserByIdCacheKeyFormat = "GetUserById_{0}";

        private readonly IRepository _underlyingRepository;
        private readonly IObjectCache _objectCache;

        public CachedRepository(IRepository underlyingRepository, IObjectCache objectCache)
        {
            this._underlyingRepository = underlyingRepository;
            this._objectCache = objectCache;
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return this._objectCache.GetOrSetAsync(GetUsersCacheKeyFormat, () =>
            {
                return this._underlyingRepository.GetUsers();
            });
        }

        public Task<User> GetUserByEmail(string email)
        {
            return this._objectCache.GetOrSetAsync(FormatKey(GetUserByEmailCacheKeyFormat, email), () =>
            {
                return this._underlyingRepository.GetUserByEmail(email);
            });
        }

        public Task<User> GetUserById(int id)
        {
            return this._objectCache.GetOrSetAsync(FormatKey(GetUserByIdCacheKeyFormat, id), () =>
            {
                return this._underlyingRepository.GetUserById(id);
            });
        }

        public async Task AddUser(string email, string username)
        {
            await this._underlyingRepository.AddUser(email, username);
            this._objectCache.Remove(GetUsersCacheKeyFormat);
        }

        public async Task UpdateUser(User changedUser)
        {
            await this._underlyingRepository.UpdateUser(changedUser);
            this._objectCache.Remove(GetUsersCacheKeyFormat);
            this._objectCache.Remove(FormatKey(GetUserByIdCacheKeyFormat, changedUser.Id));
            this._objectCache.Remove(FormatKey(GetUserByEmailCacheKeyFormat, changedUser.Email));
        }

        public Task<QueuedTask> PopNextTask()
        {
            return this._underlyingRepository.PopNextTask();
        }

        public Task<QueuedTask> GetTask(int taskId)
        {
            return this._underlyingRepository.GetTask(taskId);
        }

        public Task AddCompilationTask(int userId)
        {
            return this._underlyingRepository.AddCompilationTask(userId);
        }

        public Task AddGameTask(int[] userIds)
        {
            return this._underlyingRepository.AddGameTask(userIds);
        }

        public Task MarkTaskAsStarted(int taskId)
        {
            return this._underlyingRepository.MarkTaskAsStarted(taskId);
        }

        public Task MarkTaskAsFinished(int taskId)
        {
            return this._underlyingRepository.MarkTaskAsFinished(taskId);
        }

        public Task<bool> DoesUserHaveActiveCompileTask(int userId)
        {
            return this._underlyingRepository.DoesUserHaveActiveCompileTask(userId);
        }

        public Task<int> AddGame(ICollection<int> userIds)
        {
            return this._underlyingRepository.AddGame(userIds);
        }

        public Task<Game> GetGame(int id)
        {
            return this._underlyingRepository.GetGame(id);
        }

        private static string FormatKey(string keyFormat, params object[] objects)
        {
            return string.Format(CultureInfo.InvariantCulture, keyFormat, objects);
        }
    }
}