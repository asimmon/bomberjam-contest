using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
using Bomberjam.Website.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    [Authorize]
    [Route("~/account")]
    public class AccountController : BaseBomberjamController<AccountController>
    {
        private const int MaxBotUploadIn24h = 8;
        private static readonly TimeSpan BotSubmitWaitDelay = TimeSpan.FromMinutes(5);

        public AccountController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<AccountController> logger)
            : base(repository, storage, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await this.GetAuthenticatedUser();
            var bots = await this.Repository.GetBots(user.Id);
            return this.View("Index", new AccountReadWriteViewModel(user, bots));
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountWriteViewModel viewModel)
        {
            var user = await this.GetAuthenticatedUser();

            if (!this.ModelState.IsValid)
            {
                var bots = await this.Repository.GetBots(user.Id);
                return this.View("Index", new AccountReadWriteViewModel(user, bots, viewModel));
            }

            if (string.Equals(viewModel.UserName, user.UserName, StringComparison.Ordinal) && string.Equals(viewModel.Organization, user.Organization, StringComparison.Ordinal))
                return this.RedirectToAction("Index", "Account");

            if (!string.Equals(viewModel.UserName, user.UserName, StringComparison.Ordinal))
            {
                var isUserNameAlreadyUsed = await this.Repository.IsUserNameAlreadyUsed(viewModel.UserName);
                if (isUserNameAlreadyUsed)
                {
                    var bots = await this.Repository.GetBots(user.Id);
                    this.ModelState.AddModelError<AccountWriteViewModel>(vm => vm.UserName, "This username is already used");
                    return this.View("Index", new AccountReadWriteViewModel(user, bots, viewModel));
                }
            }

            var previousUserName = user.UserName;
            var previousOrganization = user.Organization;

            user.UserName = viewModel.UserName;
            user.Organization = viewModel.Organization;
            await this.Repository.UpdateUser(user);
            this.Logger.LogInformation(
                "User {UserId} changed it username from {PreviousUserName} to {UserName} and organization from {PreviousOrganization} to {Organization}",
                user.Id, previousUserName, user.UserName, previousOrganization, user.Organization);

            return this.RedirectToAction("Index", "Account");
        }

        [HttpGet("submit")]
        public IActionResult Submit()
        {
            return this.View("Submit", new AccountSubmitViewModel
            {
                BotSubmitWaitDelayMinutes = (int)Math.Ceiling(BotSubmitWaitDelay.TotalMinutes),
                MaxBotUploadIn24h = MaxBotUploadIn24h
            });
        }

        [HttpPost("submit")]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(Constants.BotSourceCodeMaxUploadSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = Constants.BotSourceCodeMaxUploadSize)]
        public async Task<IActionResult> Submit(AccountSubmitViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
                return this.View("Submit", viewModel);

            viewModel.BotSubmitWaitDelayMinutes = (int)Math.Ceiling(BotSubmitWaitDelay.TotalMinutes);
            viewModel.MaxBotUploadIn24h = MaxBotUploadIn24h;

            var user = await this.GetAuthenticatedUser();
            var canUploadBotResult = await this.CanSubmitBot(user.Id);

            if (!canUploadBotResult.CanSubmitBot)
            {
                this.ModelState.AddModelError<AccountSubmitViewModel>(vm => vm.BotFile, canUploadBotResult.ErrorMessage);
                return this.View("Submit", viewModel);
            }

            Guid newBotId;
            using (var transaction = await this.Repository.CreateTransaction())
            {
                newBotId = await this.Repository.AddBot(user.Id);
                await this.Storage.UploadBotSourceCode(newBotId, viewModel.BotFile.OpenReadStream());
                await this.Repository.AddCompilationTask(newBotId);
                await transaction.CommitAsync();
            }

            this.Logger.LogInformation("User {UserId} submitted a new bot {BotId}, is compiled: {IsCompiled}", user.Id, newBotId, false);

            return this.RedirectToAction("Index", "Account");
        }

        private async Task<CanSubmitBotResult> CanSubmitBot(Guid userId)
        {
            var bots = await this.Repository.GetBots(userId).ConfigureAwait(false);
            var sortedBots = bots.OrderByDescending(b => b.Created).ToList();

            var mostRecentBot = sortedBots.FirstOrDefault();
            if (mostRecentBot == null)
                return CanSubmitBotResult.Authorized();

            var now = DateTime.UtcNow;
            var timeElapsedSinceLastBotUpdate = now - mostRecentBot.Created;
            if (timeElapsedSinceLastBotUpdate <= BotSubmitWaitDelay)
            {
                var remainingWaitTime = BotSubmitWaitDelay - timeElapsedSinceLastBotUpdate;

                string remainingWaitTimeStr;
                if (remainingWaitTime.TotalMinutes >= 1d)
                {
                    var remainingMinutes = (int)Math.Ceiling(remainingWaitTime.TotalMinutes);
                    remainingWaitTimeStr = remainingMinutes + " " + (remainingMinutes == 1 ? "minute" : "minutes");
                }
                else
                {
                    var remainingSeconds = (int)Math.Ceiling(remainingWaitTime.TotalSeconds);
                    remainingWaitTimeStr = remainingSeconds + " " + (remainingSeconds == 1 ? "second" : "seconds");
                }

                return CanSubmitBotResult.Unauthorized($"You must wait {remainingWaitTimeStr} before uploading a new bot");
            }

            var yesterday = now.AddDays(-1);
            var botSubmittedInPast24hCount = sortedBots.Count(b => b.Created >= yesterday);

            return botSubmittedInPast24hCount >= MaxBotUploadIn24h
                ? CanSubmitBotResult.Unauthorized($"You can only upload a maximum of {MaxBotUploadIn24h} bots in a 24 hours sliding window")
                : CanSubmitBotResult.Authorized();
        }

        private sealed class CanSubmitBotResult
        {
            private CanSubmitBotResult(bool canSubmitBot, string errorMessage)
            {
                this.CanSubmitBot = canSubmitBot;
                this.ErrorMessage = errorMessage;
            }

            public bool CanSubmitBot { get; }
            public string ErrorMessage { get; }

            public static CanSubmitBotResult Authorized() => new CanSubmitBotResult(true, string.Empty);
            public static CanSubmitBotResult Unauthorized(string errorMessage) => new CanSubmitBotResult(false, errorMessage);
        }

        [HttpGet("bot/{botId}/download")]
        public async Task<IActionResult> DownloadBot(Guid botId)
        {
            var bot = await this.Repository.GetBot(botId);
            var user = await this.GetAuthenticatedUser();

            if (bot.UserId != user.Id)
                return this.Forbid("This bot does not belong to you");

            this.Logger.LogInformation("User {UserId} downloads its bot {BotId}", user.Id, botId);

            return this.PushFileStream(MediaTypeNames.Application.Zip, $"bot-{botId:D}.zip", async responseStream =>
            {
                await using (responseStream)
                {
                    if (bot.Status == CompilationStatus.CompilationSucceeded)
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
    }
}
