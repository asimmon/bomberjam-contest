using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Bomberjam.Common;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.SupportedAuthenticationSchemes)]
    [ApiController]
    [Route("~/api")]
    public class ApiController : BaseApiController<ApiController>
    {
        private static readonly SemaphoreSlim GetNextTaskMutex = new(1);

        private readonly IBotStorage _botStorage;

        public ApiController(IRepository repository, IBotStorage botStorage, ILogger<ApiController> logger)
            : base(repository, logger)
        {
            this._botStorage = botStorage;
        }

        [HttpGet("user/{userId}/bot/download")]
        public async Task<IActionResult> DownloadBot(Guid userId, [FromQuery(Name = "compiled")] bool isCompiled)
        {
            var fileStream = isCompiled ? this._botStorage.DownloadCompiledBot(userId) : this._botStorage.DownloadBotSourceCode(userId);
            var fileBytes = await StreamToByteArray(fileStream);
            return this.File(fileBytes, "application/octet-stream", $"bot-{userId:D}.zip");
        }

        [HttpPost("user/{userId}/bot/upload")]
        public async Task<IActionResult> UploadBot(Guid userId)
        {
            await this._botStorage.UploadCompiledBot(userId, this.Request.Body);
            return this.Ok();
        }

        private static async Task<byte[]> StreamToByteArray(Stream stream)
        {
            await using (stream)
            {
                await using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        [HttpPost("user/{userId}/bot/compilation-result")]
        public async Task<IActionResult> SetCompilationResult([FromBody] BotCompilationResult compilationResult)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(GetAllErrors(this.ModelState));

            User user;
            try
            {
                user = await this.Repository.GetUserById(compilationResult.UserId);
            }
            catch (UserNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

            user.IsCompiled = compilationResult.DidCompile;
            user.BotLanguage = compilationResult.Language ?? string.Empty;
            user.CompilationErrors = compilationResult.Errors ?? string.Empty;

            await this.Repository.UpdateUser(user);

            return this.Ok();
        }

        private static string[] GetAllErrors(ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray();
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTask(Guid taskId)
        {
            try
            {
                var task = await this.Repository.GetTask(taskId);
                return this.Ok(task);
            }
            catch (QueuedTaskNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpGet("task/next")]
        public async Task<IActionResult> GetNextTask()
        {
            await GetNextTaskMutex.WaitAsync();

            try
            {
                var task = await this.Repository.PopNextTask();
                return this.Ok(task);
            }
            catch (QueuedTaskNotFoundException)
            {
                return this.NotFound();
            }
            finally
            {
                GetNextTaskMutex.Release();
            }
        }

        [HttpGet("task/{taskId}/started")]
        public async Task<IActionResult> MarkTaskAsStarted(Guid taskId)
        {
            try
            {
                await this.Repository.MarkTaskAsStarted(taskId);
                var task = await this.Repository.GetTask(taskId);
                return this.Ok(task);
            }
            catch (QueuedTaskNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpGet("task/{taskId}/finished")]
        public async Task<IActionResult> MarkTaskAsFinished(Guid taskId)
        {
            try
            {
                await this.Repository.MarkTaskAsFinished(taskId);
                var task = await this.Repository.GetTask(taskId);
                return this.Ok(task);
            }
            catch (QueuedTaskNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpGet("game/yolo")]
        public async Task<IActionResult> StartYoloGame()
        {
            var users = await this.Repository.GetUsers();
            await this.Repository.AddGameTask(users.Take(4).ToList());
            return this.Ok();
        }

        [HttpPost("game")]
        public async Task<IActionResult> AddGameResult([FromBody] GameResult gameResult)
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

            var gameId = await this.Repository.AddGame(gameHistory.Summary);

            var jsonGameHistoryStream = SerializeGameHistoryToJsonStream(RemoveWebsiteIds(gameHistory));

            await this._botStorage.UploadGameResult(gameId, jsonGameHistoryStream);

            return this.Ok();
        }

        private static GameHistory RemoveWebsiteIds(GameHistory gh)
        {
            if (gh.Summary is { Players: var players })
            {
                foreach (var (_, playerSummary) in players)
                {
                    playerSummary.WebsiteId = null;
                }
            }

            return gh;
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