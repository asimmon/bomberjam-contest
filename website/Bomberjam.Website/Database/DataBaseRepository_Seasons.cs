using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MethodHasAsyncOverload
namespace Bomberjam.Website.Database
{
    public partial class DatabaseRepository
    {
        public async Task<Season> GetCurrentSeason()
        {
            var dbSeason = await this._dbContext.Seasons.OrderByDescending(s => s.Created).FirstOrDefaultAsync().ConfigureAwait(false);
            if (dbSeason == null)
                throw new EntityNotFound(EntityType.Season, 0);

            return MapSeason(dbSeason);
        }

        public async Task<Season> GetSeason(int id)
        {
            var dbSeason = await this._dbContext.Seasons.FirstOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);
            if (dbSeason == null)
                throw new EntityNotFound(EntityType.Season, id);

            return MapSeason(dbSeason);
        }

        public async Task<IEnumerable<Season>> GetSeasons()
        {
            return await this._dbContext.Seasons
                .OrderBy(s => s.Id)
                .Select(s => MapSeason(s))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        private static Season MapSeason(DbSeason dbSeason) => new Season
        {
            Id = dbSeason.Id,
            Created = dbSeason.Created,
            Updated = dbSeason.Updated,
            Name = dbSeason.Name,
            UserCount = dbSeason.UserCount
        };
    }
}