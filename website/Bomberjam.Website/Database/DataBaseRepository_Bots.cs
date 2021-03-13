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
    public partial class DatabaseRepository
    {
        public async Task<IEnumerable<Bot>> GetBots(Guid userId, int? max)
        {
            var selectQuery =
                from b in this._dbContext.Bots where b.UserId == userId
                join gu in this._dbContext.GameUsers on b.Id equals gu.BotId into innerJoin
                from leftJoin in innerJoin.DefaultIfEmpty()
                group leftJoin by new { b.Id, b.Iteration, b.UserId, b.Created, b.Updated, b.Status, b.Language, b.Errors }
                into grouped
                orderby grouped.Key.Created descending
                select new Bot
                {
                    Id = grouped.Key.Id,
                    Iteration = grouped.Key.Iteration,
                    UserId = grouped.Key.UserId,
                    Created = grouped.Key.Created,
                    Updated = grouped.Key.Updated,
                    Status = grouped.Key.Status,
                    Language = grouped.Key.Language,
                    Errors = grouped.Key.Errors,
                    GameCount = grouped.Count(gu => gu != null)
                };

            return max.HasValue && max.Value > 0
                ? await selectQuery.Take(max.Value).ToListAsync().ConfigureAwait(false)
                : await selectQuery.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Bot> GetBot(Guid botId)
        {
            return await this._dbContext.Bots
                .Where(b => b.Id == botId)
                .Select(b => new Bot
                {
                    Id = b.Id,
                    Iteration = b.Iteration,
                    UserId = b.UserId,
                    Created = b.Created,
                    Updated = b.Updated,
                    Status = b.Status,
                    Language = b.Language,
                    Errors = b.Errors
                })
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<Guid> AddBot(Guid userId)
        {
            var latestbot = await this._dbContext.Bots.OrderByDescending(b => b.Created).FirstOrDefaultAsync();

            var dbBot = new DbBot
            {
                UserId = userId,
                Iteration = latestbot == null ? 1 : latestbot.Iteration + 1,
                Status = CompilationStatus.NotCompiled,
                Language = string.Empty,
                Errors = string.Empty
            };

            this._dbContext.Bots.Add(dbBot);

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dbBot.Id;
        }

        public async Task UpdateBot(Bot bot)
        {
            var dbBot = await this._dbContext.Bots.FirstOrDefaultAsync(b => b.Id == bot.Id).ConfigureAwait(false);
            if (dbBot == null)
                throw new EntityNotFound(EntityType.Bot, bot.Id);

            if (bot.Status != CompilationStatus.Unknown)
                dbBot.Status = bot.Status;

            if (!string.IsNullOrWhiteSpace(bot.Language))
                dbBot.Language = bot.Language;

            if (!string.IsNullOrWhiteSpace(bot.Errors))
                dbBot.Errors = bot.Errors;

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}