using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bomberjam.Website.Common
{
    public sealed class ObjectCache : IObjectCache
    {
        private readonly ConcurrentDictionary<string, CachedValue> _values;

        public ObjectCache()
        {
            this._values = new ConcurrentDictionary<string, CachedValue>(StringComparer.OrdinalIgnoreCase);
        }

        public void Remove(string cacheKey)
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));

            this._values.Remove(cacheKey, out _);
        }

        public Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> asyncValueFactory)
        {
            return this.GetOrSetInternalAsync(cacheKey, asyncValueFactory);
        }

        public Task<IEnumerable<T>> GetOrSetAsync<T>(string cacheKey, Func<Task<IEnumerable<T>>> asyncValueFactory)
        {
            async Task<IEnumerable<T>> EnsureEnumeratedValue()
            {
                var enumerable = await asyncValueFactory();
                return enumerable is ICollection<T> enumerated ? enumerated : enumerable?.ToList();
            }

            return this.GetOrSetInternalAsync(cacheKey, EnsureEnumeratedValue);
        }

        private async Task<T> GetOrSetInternalAsync<T>(string cacheKey, Func<Task<T>> asyncValueFactory)
        {
            var value = this.GetOrCreateCachedValue(cacheKey);

            if (!value.HasValue)
            {
                using (await ValueLock.EnterAsync(value))
                {
                    if (!value.HasValue && asyncValueFactory != null)
                    {
                        value.SetValue(await asyncValueFactory());
                    }
                }
            }

            return value.GetValue<T>();
        }

        private CachedValue GetOrCreateCachedValue(string cacheKey)
        {
            if (cacheKey == null)
                throw new ArgumentNullException(nameof(cacheKey));

            return this._values.GetOrAdd(cacheKey, _ => new CachedValue());
        }

        private sealed class CachedValue
        {
            private object _value;

            public bool HasValue { get; private set; }

            public T GetValue<T>()
            {
                return (T)(this._value ?? default(T));
            }

            public void SetValue<T>(T value)
            {
                if (typeof(T).IsSubclassOf(typeof(Task)))
                    throw new NotSupportedException("Caching Task objects is not supported");

                this._value = value;
                this.HasValue = true;
            }
        }

        private sealed class ValueLock : IDisposable
        {
            private static readonly ConcurrentDictionary<CachedValue, SemaphoreSlim> Semaphores = new();

            private readonly CachedValue _value;
            private readonly SemaphoreSlim _semaphore;

            private ValueLock(CachedValue value, SemaphoreSlim semaphore)
            {
                this._value = value;
                this._semaphore = semaphore;
            }

            public static async Task<ValueLock> EnterAsync(CachedValue value)
            {
                var semaphore = Semaphores.GetOrAdd(value, _ => new SemaphoreSlim(1));
                await semaphore.WaitAsync();
                return new ValueLock(value, semaphore);
            }

            public void Dispose()
            {
                this._semaphore.Release();
                Semaphores.Remove(this._value, out _);
            }
        }
    }
}