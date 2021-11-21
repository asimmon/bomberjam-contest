using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Bomberjam.Common;
using Bomberjam.Website.Authentication;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Bomberjam.Website.Jobs;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
using Bomberjam.Website.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    [Authorize(AuthenticationSchemes = SecretAuthenticationDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("~/api")]
    public class ApiController : BaseBomberjamController<ApiController>
    {
        public ApiController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<ApiController> logger)
            : base(repository, storage, logger)
        {
        }

        [HttpGet("bot/{botId}/download")]
        public IActionResult DownloadBot(Guid botId, [FromQuery(Name = "compiled")] bool isCompiled)
        {
            this.Logger.LogInformation("Downloading bot {BotId}, is compiled: {IsCompiled}", botId, isCompiled);

            return this.PushFileStream(MediaTypeNames.Application.Zip, $"bot-{botId:D}.zip", async responseStream =>
            {
                await using (responseStream)
                {
                    if (isCompiled)
                    {
                        await this.Storage.DownloadCompiledBotTo(botId, responseStream);
                    }
                    else
                    {
                        await this.Storage.DownloadBotSourceCodeTo(botId, responseStream);
                    }
                }
            });
        }

        [HttpPost("bot/{botId}/upload")]
        [RequestSizeLimit(Constants.CompiledBotMaxUploadSize)]
        public async Task<IActionResult> UploadCompiledBot(Guid botId)
        {
            this.Logger.LogInformation("Uploading bot {BotId}, is compiled: {IsCompiled}", botId, true);
            await this.Storage.UploadCompiledBot(botId, this.Request.Body);
            return this.Ok();
        }

        private static readonly string[] EolSeparators = { "\r\n", "\r", "\n" };

        [HttpPost("bot/{botId}/compilation-result")]
        public async Task<IActionResult> SetCompilationResult([FromBody] BotCompilationResult compilationResult)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(GetAllErrors(this.ModelState));

            var bot = await this.Repository.GetBot(compilationResult.BotId);

            bot.Status = compilationResult.DidCompile ? CompilationStatus.CompilationSucceeded : CompilationStatus.CompilationFailed;
            bot.Language = compilationResult.Language ?? string.Empty;
            bot.Errors = RemoveDuplicateLines(compilationResult.Errors ?? string.Empty);

            await this.Repository.UpdateBot(bot);
            this.Logger.LogInformation("Changing bot {BotId} status to {CompilationStatus}", bot.Id, bot.Status);

            // Immediately enqueue a testing game task on success
            if (bot.Status == CompilationStatus.CompilationSucceeded)
            {
                // TODO optimize the users query so we don't return all users
                var users = await this.Repository.GetUsersWithCompiledBot();
                var match = MatchMaker.Execute(users).FirstOrDefault(m => m.Users.Any(u => u.Id == bot.UserId));
                if (match != null)
                {
                    await this.Repository.AddGameTask(match.Users, GameOrigin.TestingPurpose);
                    this.Logger.LogInformation("Queuing a new {GameOrigin} game", GameOrigin.TestingPurpose);
                }
            }

            return this.Ok();
        }

        private static string RemoveDuplicateLines(string text) => text
            .Split(EolSeparators, StringSplitOptions.RemoveEmptyEntries)
            .Distinct(StringComparer.Ordinal)
            .JoinStrings(Environment.NewLine);

        [HttpGet("user/{userId}/bot")]
        public async Task<IActionResult> GetUserCompiledBotId(Guid userId)
        {
            var bots = await this.Repository.GetBots(userId);
            var compiledBots = bots.Where(b => b.Status == CompilationStatus.CompilationSucceeded);
            return compiledBots.OrderByDescending(b => b.Created).FirstOrDefault() is { } latestCompiledBot
                ? this.Ok(latestCompiledBot.Id.ToString("D"))
                : this.NotFound();
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTask(Guid taskId)
        {
            this.Logger.LogDebug("Retrieving task {TaskId}", taskId);

            try
            {
                var task = await this.Repository.GetTask(taskId);
                return this.Ok(task);
            }
            catch (EntityNotFound)
            {
                return this.NotFound();
            }
        }

        [HttpGet("task/next")]
        public async Task<IActionResult> GetNextTask()
        {
            this.Logger.LogDebug("Retrieving next task");

            QueuedTask task;

            await GetNextTaskMutex.WaitAsync();

            try
            {
                task = await this.Repository.PopNextTask();
            }
            catch (EntityNotFound)
            {
                task = null;
            }
            finally
            {
                GetNextTaskMutex.Release();
            }

            if (this.Request.Headers.TryGetValue("X-Worker-ID", out var workerIdStr) && Guid.TryParse(workerIdStr, out var workerId))
            {
                try
                {
                    await this.Repository.AddOrUpdateWorker(workerId);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, ex.Message);
                }
            }

            return task == null ? this.NotFound() : this.Ok(task);
        }

        [HttpGet("task/{taskId}/started")]
        public async Task<IActionResult> MarkTaskAsStarted(Guid taskId)
        {
            this.Logger.LogInformation("Task {TaskId} has started", taskId);

            try
            {
                using (var transaction = await this.Repository.CreateTransaction())
                {
                    await this.Repository.MarkTaskAsStarted(taskId);
                    var task = await this.Repository.GetTask(taskId);
                    await transaction.CommitAsync();
                    return this.Ok(task);
                }
            }
            catch (EntityNotFound)
            {
                return this.NotFound();
            }
        }

        [HttpGet("task/{taskId}/finished")]
        public async Task<IActionResult> MarkTaskAsFinished(Guid taskId)
        {
            this.Logger.LogInformation("Task {TaskId} has finished", taskId);

            try
            {
                using (var transaction = await this.Repository.CreateTransaction())
                {
                    await this.Repository.MarkTaskAsFinished(taskId);
                    var task = await this.Repository.GetTask(taskId);
                    await transaction.CommitAsync();
                    return this.Ok(task);
                }
            }
            catch (EntityNotFound)
            {
                return this.NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet("game/{gameId}")]
        public IActionResult GetGameWithStreaming(Guid gameId)
        {
            this.Logger.LogInformation("Downloading game {GameId}", gameId);

            return this.PushFileStream(MediaTypeNames.Application.Json, $"game-{gameId:D}.json", async responseStream =>
            {
                await using (responseStream)
                {
                    await this.Storage.DownloadGameResultTo(gameId, responseStream);
                }
            });
        }

        [HttpPost("game")]
        public async Task<IActionResult> AddGameResult([FromBody] ApiGameResult gameResult)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(GetAllErrors(this.ModelState));

            GameHistory gameHistory;
            try
            {
                gameHistory = JsonSerializer.Deserialize<GameHistory>(gameResult.SerializedHistory);
                if (gameHistory == null)
                    throw new Exception("JSON game result parsing returned null");
            }
            catch (Exception ex)
            {
                return this.BadRequest("Not a valid JSON game result: " + ex);
            }

            gameHistory.Summary.StandardOutput = gameResult.StandardOutput ?? string.Empty;
            gameHistory.Summary.StandardError = gameResult.StandardError ?? string.Empty;

            if (gameResult.PlayerBotIds != null)
            {
                foreach (var (playerIndex, playerBotIdStr) in gameResult.PlayerBotIds)
                {
                    if (Guid.TryParse(playerBotIdStr, out var playerBotId) && gameHistory.Summary.Players.TryGetValue(playerIndex, out var playerSummary))
                        playerSummary.BotId = playerBotId;
                }
            }

            await ComputeAllUserGlobalRanksMutex.WaitAsync();

            try
            {
                using (var transaction = await this.Repository.CreateTransaction())
                {
                    if (gameResult.Origin == GameOrigin.RankedMatchmaking)
                    {
                        gameHistory = await this.ComputeNewUserPoints(gameHistory);
                    }

                    var currentSeason = await this.Repository.GetCurrentSeason();
                    var gameId = await this.Repository.AddGame(gameHistory.Summary, gameResult.Origin, currentSeason.Id);
                    this.Logger.LogInformation("Added new game result {GameId}", gameId);

                    if (gameResult.Origin == GameOrigin.RankedMatchmaking)
                    {
                        await this.Repository.UpdateAllUserGlobalRanks();
                    }

                    var jsonGameHistoryStream = SerializeGameHistoryToJsonStream(gameHistory);
                    await this.Storage.UploadGameResult(gameId, jsonGameHistoryStream);
                    await transaction.CommitAsync();
                }
            }
            finally
            {
                ComputeAllUserGlobalRanksMutex.Release();
            }

            return this.Ok();
        }

        private async Task<GameHistory> ComputeNewUserPoints(GameHistory gameHistory)
        {
            this.Logger.LogInformation("Computing new user points");

            var users = new Dictionary<string, User>();

            foreach (var (playerId, player) in gameHistory.Summary.Players)
            {
                users[playerId] = await this.Repository.GetUserById(player.WebsiteId!.Value).ConfigureAwait(false);
            }

            var match = new EloMatch();

            var eloPlayers = gameHistory.Summary.Players.Aggregate(new Dictionary<string, IEloPlayer>(), (acc, kvp) =>
            {
                var user = users[kvp.Key];
                acc[kvp.Key] = match.AddPlayer(kvp.Value.Rank, user.Points);
                return acc;
            });

            match.ComputeElos();

            foreach (var (playerId, player) in gameHistory.Summary.Players)
            {
                player.Points = eloPlayers[playerId].NewElo;
                player.DeltaPoints = eloPlayers[playerId].EloChange;
            }

            return gameHistory;
        }

        private static MemoryStream SerializeGameHistoryToJsonStream(GameHistory gh)
        {
            var memoryStream = new MemoryStream();

            using (var jsonWriter = new Utf8JsonWriter(memoryStream, Bomberjam.Common.Constants.DefaultJsonWriterOptions))
            {
                JsonSerializer.Serialize(jsonWriter, gh);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}