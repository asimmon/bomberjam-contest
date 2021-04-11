using System;
using System.Collections.Generic;
using System.Globalization;
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
                .OrderByDescending(s => s.Created)
                .Select(s => MapSeason(s))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        private static Season MapSeason(DbSeason dbSeason) => new Season
        {
            Id = dbSeason.Id,
            Created = dbSeason.Created,
            Updated = dbSeason.Updated,
            Finished = dbSeason.Finished,
            Name = dbSeason.Name,
            UserCount = dbSeason.UserCount
        };

        public async Task StartNewSeason()
        {
            var oldSeason = await this._dbContext.Seasons
                .OrderByDescending(s => s.Created)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (oldSeason == null)
                throw new InvalidOperationException("There must be a current season to end");

            var seasonSummariesQuery =
                from g in this._dbContext.Games
                where g.SeasonId == oldSeason.Id
                where g.Origin == GameOrigin.RankedMatchmaking
                join gu in this._dbContext.GameUsers on g.Id equals gu.GameId into join1
                from gu in join1
                join u in this._dbContext.Users on gu.UserId equals u.Id into join2
                from u in join2.DefaultIfEmpty()
                group u by new { UserId = u.Id, GlobalRank = u.GlobalRank }
                into grouped
                select new
                {
                    UserId = grouped.Key.UserId,
                    SeasonId = oldSeason.Id,
                    GlobalRank = grouped.Key.GlobalRank,
                    RankedGameCount = grouped.Count(x => x != null)
                };

            var seasonSummaries = await seasonSummariesQuery.ToListAsync().ConfigureAwait(false);

            foreach (var seasonSummary in seasonSummaries)
            {
                this._dbContext.SeasonSummaries.Add(new DbSeasonSummary
                {
                    UserId = seasonSummary.UserId,
                    SeasonId = seasonSummary.SeasonId,
                    GlobalRank = seasonSummary.GlobalRank,
                    RankedGameCount = seasonSummary.RankedGameCount
                });
            }

            oldSeason.UserCount = seasonSummaries.Count;
            oldSeason.Finished = DateTime.UtcNow;

            var newSeasonId = oldSeason.Id + 1;
            this._dbContext.Seasons.Add(new DbSeason
            {
                Id = newSeasonId,
                Name = "S" + newSeasonId.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')
            });

            await foreach (var user in this._dbContext.Users)
            {
                user.Points = Constants.InitialPoints;
            }

            await this.UpdateAllUserGlobalRanks().ConfigureAwait(false);

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<SeasonSummary>> GetSeasonSummaries(Guid userId)
        {
            return await this._dbContext.SeasonSummaries
                .Where(summary => summary.UserId == userId)
                .Join(this._dbContext.Seasons, summary => summary.SeasonId, season => season.Id, (summary, season) => new SeasonSummary
                {
                    SeasonId = season.Id,
                    SeasonName = season.Name,
                    GlobalRank = summary.GlobalRank,
                    UserCount = season.UserCount.Value,
                    RankedGameCount = summary.RankedGameCount
                })
                .OrderByDescending(x => x.SeasonId)
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}