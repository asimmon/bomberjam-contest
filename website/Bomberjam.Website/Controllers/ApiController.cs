using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
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
        public async Task<IActionResult> DownloadBot(int userId, [FromQuery(Name = "compiled")] bool isCompiled)
        {
            var fileStream = isCompiled ? this._botStorage.DownloadCompiledBot(userId) : this._botStorage.DownloadBotSourceCode(userId);
            var fileBytes = await StreamToByteArray(fileStream);
            return this.File(fileBytes, "application/octet-stream", $"bot-{userId}.zip");
        }

        [HttpPost("user/{userId}/bot/upload")]
        public async Task<IActionResult> UploadBot(int userId)
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
        public async Task<IActionResult> GetTask(int taskId)
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
        public async Task<IActionResult> MarkTaskAsStarted(int taskId)
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
        public async Task<IActionResult> MarkTaskAsFinished(int taskId)
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

        [HttpGet("user/{userId}/game")]
        public async Task<IActionResult> StartYoloGame(int userId)
        {
            try
            {
                await this.Repository.AddGameTask(new[] { userId, userId, userId, userId });
                return this.Ok();
            }
            catch (QueuedTaskNotFoundException)
            {
                return this.NotFound();
            }
        }
    }

    public sealed class BotCompilationResult
    {
        [Required]
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [Required]
        [JsonPropertyName("didCompile")]
        public bool DidCompile { get; set; }

        [Required]
        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("errors")]
        public string Errors { get; set; }
    }

    public sealed class QueuedTask
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("type")]
        public QueuedTaskType Type { get; set; }

        [JsonPropertyName("status")]
        public QueuedTaskStatus Status { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }
    }
}