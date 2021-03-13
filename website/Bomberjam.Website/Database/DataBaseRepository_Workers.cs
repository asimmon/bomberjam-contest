using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MethodHasAsyncOverload
namespace Bomberjam.Website.Database
{
    public partial class DatabaseRepository
    {
        public async Task<IEnumerable<Worker>> GetWorkers(int max)
        {
            var dbWorkers = await this._dbContext.Workers.OrderByDescending(w => w.Created).Take(max).ToListAsync().ConfigureAwait(false);
            return dbWorkers.Select(MapWorker).ToList();
        }

        public async Task<Worker> AddOrUpdateWorker(Guid id)
        {
            var utcNow = DateTime.UtcNow;

            var dbWorker = await this._dbContext.Workers.FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);
            if (dbWorker == null)
            {
                dbWorker = new DbWorker
                {
                    Id = id,
                    Created = utcNow,
                    Updated = utcNow,
                };

                this._dbContext.Add(dbWorker);
            }

            dbWorker.Updated = utcNow;

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);

            return MapWorker(dbWorker);
        }

        private static Worker MapWorker(DbWorker dbWorker) => new Worker
        {
            Id = dbWorker.Id,
            Created = dbWorker.Created,
            Updated = dbWorker.Updated
        };
    }
}