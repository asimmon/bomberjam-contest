using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bomberjam.Website.Common
{
    public interface IObjectCache
    {
        void Remove(string cacheKey);

        Task<IEnumerable<T>> GetOrSetAsync<T>(string cacheKey, Func<Task<IEnumerable<T>>> asyncValueFactory);

        Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> asyncValueFactory);
    }
}