using System;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Common;
using Bomberjam.Website.Common;
using Bomberjam.Website.Models;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MethodHasAsyncOverload
namespace Bomberjam.Website.Database
{
    public partial class DatabaseRepository
    {
        public async Task<GameInfo> GetGame(Guid gameId)
        {
            // Then join each game to its participants and user details
            var gameAndParticipants = await this._dbContext.Games
                .Where(g => g.Id == gameId)
                .Join(this._dbContext.GameUsers, game => game.Id, gameUser => gameUser.GameId, (game, gameUser) => new
                {
                    GameId = game.Id,
                    GameCreated = game.Created,
                    GameOrigin = game.Origin,
                    gameUser.UserId,
                    UserDeltaPoints = gameUser.DeltaPoints,
                    UserRank = gameUser.Rank,
                    UserErrors = gameUser.Errors
                })
                .Join(this._dbContext.Users, tmp => tmp.UserId, user => user.Id, (tmp, user) => new
                {
                    tmp.GameId,
                    tmp.GameCreated,
                    tmp.GameOrigin,
                    tmp.UserId,
                    tmp.UserDeltaPoints,
                    tmp.UserRank,
                    tmp.UserErrors,
                    user.UserName,
                    UserGithubId = user.GithubId
                })
                .OrderByDescending(x => x.GameCreated)
                .ThenBy(x => x.UserRank)
                .ToListAsync()
                .ConfigureAwait(false);

            // Project the results to actual C# models
            var groupedGame = gameAndParticipants
                .GroupBy(x => new { x.GameId, x.GameCreated, x.GameOrigin })
                .Select(g => new GameInfo(g.Key.GameId, g.Key.GameCreated, g.Key.GameOrigin, g.Select(row => new GameUserInfo
                {
                    Id = row.UserId,
                    GithubId = row.UserGithubId,
                    UserName = row.UserName,
                    DeltaPoints = row.UserDeltaPoints,
                    Rank = row.UserRank,
                    Errors = row.UserErrors
                })))
                .FirstOrDefault();

            if (groupedGame == null)
                throw new EntityNotFound(EntityType.Game, gameId);

            return groupedGame;
        }

        public async Task<PaginationModel<GameInfo>> GetPagedUserGames(Guid userId, int seasonId, int page)
        {
            var pageIndex = page - 1;
            var skipCount = pageIndex * Constants.GamesPageSize;

            var totalGamesCount = await this._dbContext.GameUsers.CountAsync(x => x.UserId == userId).ConfigureAwait(false);

            // First, find the n-queried games of this user, sorted by date
            var innerGamePageQuery = this._dbContext.GameUsers
                .Where(gameUser => gameUser.UserId == userId)
                .Join(this._dbContext.Games, gameUser => gameUser.GameId, game => game.Id, (gameUser, game) => new
                {
                    GameId = game.Id,
                    GameCreated = game.Created,
                    GameOrigin = game.Origin,
                    GameSeasonId = game.SeasonId
                })
                .Where(x => x.GameSeasonId == seasonId)
                .OrderByDescending(x => x.GameCreated)
                .Skip(skipCount)
                .Take(Constants.GamesPageSize);

            // Then join each game to its participants and user details
            var groupedGameAndParticipantsQuery = await innerGamePageQuery
                .Join(this._dbContext.GameUsers, tmp => tmp.GameId, gameUser => gameUser.GameId, (tmp, gameUser) => new
                {
                    tmp.GameId,
                    tmp.GameCreated,
                    tmp.GameOrigin,
                    gameUser.UserId,
                    UserDeltaPoints = gameUser.DeltaPoints,
                    UserRank = gameUser.Rank
                })
                .Join(this._dbContext.Users, tmp => tmp.UserId, user => user.Id, (tmp, user) => new
                {
                    tmp.GameId,
                    tmp.GameCreated,
                    tmp.GameOrigin,
                    tmp.UserId,
                    tmp.UserDeltaPoints,
                    tmp.UserRank,
                    user.UserName,
                    UserGithubId = user.GithubId
                })
                .OrderByDescending(x => x.GameCreated)
                .ThenBy(x => x.UserRank)
                .ToListAsync()
                .ConfigureAwait(false);

            // Project the results to actual C# models
            var gamePage = groupedGameAndParticipantsQuery
                .GroupBy(x => new { x.GameId, x.GameCreated, x.GameOrigin })
                .Select(g => new GameInfo(g.Key.GameId, g.Key.GameCreated, g.Key.GameOrigin, g.Select(row => new GameUserInfo
                {
                    Id = row.UserId,
                    GithubId = row.UserGithubId,
                    UserName = row.UserName,
                    DeltaPoints = row.UserDeltaPoints,
                    Rank = row.UserRank
                })));

            return new PaginationModel<GameInfo>(gamePage, totalGamesCount, page, Constants.GamesPageSize);
        }

        public async Task<Guid> AddGame(GameSummary gameSummary, GameOrigin origin, int seasonId)
        {
            var dbGame = new DbGame();

            dbGame.Errors = gameSummary.Errors;
            dbGame.InitDuration = gameSummary.InitDuration;
            dbGame.GameDuration = gameSummary.GameDuration;
            dbGame.Stdout = gameSummary.StandardOutput;
            dbGame.Stderr = gameSummary.StandardError;
            dbGame.Origin = origin;
            dbGame.SeasonId = seasonId;

            this._dbContext.Games.Add(dbGame);

            foreach (var (_, playerSummary) in gameSummary.Players)
            {
                var userDbId = playerSummary.WebsiteId!.Value;
                var botDbId = playerSummary.BotId!.Value;

                var dbGameUser = new DbGameUser
                {
                    Game = dbGame,
                    UserId = userDbId,
                    Score = playerSummary.Score,
                    DeltaPoints = playerSummary.DeltaPoints ?? 0,
                    Rank = playerSummary.Rank,
                    Errors = playerSummary.Errors,
                    BotId = botDbId
                };

                this._dbContext.GameUsers.Add(dbGameUser);

                if (playerSummary.Points.HasValue && playerSummary.Points.Value > 0)
                {
                    var dbUser = await this._dbContext.Users.SingleAsync(u => u.Id == userDbId).ConfigureAwait(false);
                    dbUser.Points = playerSummary.Points.Value;
                }
            }

            await this._dbContext.SaveChangesAsync().ConfigureAwait(false);

            return dbGame.Id;
        }
    }
}