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
        public async Task<IEnumerable<Bot>> GetBots(Guid userId)
        {
            return await this._dbContext.Bots
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Created)
                .Select(b => MapBot(b))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Bot> GetBot(Guid botId)
        {
            return await this._dbContext.Bots
                .Where(b => b.Id == botId)
                .Select(b => MapBot(b))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }
        public async Task<Guid> AddBot(Guid userId)
        {
            var dbBot = new DbBot
            {
                UserId = userId,
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
                throw new EntityNotFound(ModelType.Bot, bot.Id);

            if (bot.Status != CompilationStatus.Unknown)
                dbBot.Status = bot.Status;

            if (!string.IsNullOrWhiteSpace(bot.Language))
                dbBot.Language = bot.Language;

            if (!string.IsNullOrWhiteSpace(bot.Errors))
                dbBot.Errors = bot.Errors;

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static Bot MapBot(DbBot b) => new Bot
        {
            Id = b.Id,
            Created = b.Created,
            Updated = b.Updated,
            Status = b.Status,
            Language = b.Language,
            Errors = b.Errors
        };
    }
}