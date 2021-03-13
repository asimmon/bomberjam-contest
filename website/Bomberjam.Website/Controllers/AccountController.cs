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
        private static readonly TimeSpan BotUploadDelay = TimeSpan.FromSeconds(10);

        public AccountController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<AccountController> logger)
            : base(repository, storage, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await this.GetAuthenticatedUser();
            var bots = await this.Repository.GetBots(user.Id);
            return this.View("Index", new AccountReadViewModel(user, bots));
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            var user = await this.GetAuthenticatedUser();
            return this.View("Edit", new AccountEditViewModel(user));
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountEditViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
                return this.View("Edit", viewModel);

            var user = await this.GetAuthenticatedUser();

            if (!string.Equals(viewModel.UserName, user.UserName, StringComparison.OrdinalIgnoreCase))
            {
                var isUserNameAlreadyUsed = await this.Repository.IsUserNameAlreadyUsed(viewModel.UserName);
                if (isUserNameAlreadyUsed)
                {
                    this.ModelState.AddModelError<AccountEditViewModel>(vm => vm.UserName, "This username is already used");
                    return this.View("Edit", viewModel);
                }
            }

            if (!string.Equals(viewModel.Email, user.Email, StringComparison.OrdinalIgnoreCase))
            {
                var isEmailAlreadyUsed = await this.Repository.IsUserEmailAlreadyUsed(viewModel.Email);
                if (isEmailAlreadyUsed)
                {
                    this.ModelState.AddModelError<AccountEditViewModel>(vm => vm.Email, "This email address is already used");
                    return this.View("Edit", viewModel);
                }
            }

            user.UserName = viewModel.UserName;
            user.Email = viewModel.Email;
            await this.Repository.UpdateUser(user);

            return this.RedirectToAction("Index", "Account");
        }

        [HttpGet("submit")]
        public IActionResult Submit()
        {
            return this.View("Submit", new AccountSubmitViewModel());
        }

        [HttpPost("submit")]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(Constants.BotSourceCodeMaxUploadSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = Constants.BotSourceCodeMaxUploadSize)]
        public async Task<IActionResult> Submit(AccountSubmitViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
                return this.View("Submit", viewModel);

            var user = await this.GetAuthenticatedUser();
            var canUploadBot = await this.CanSubmitBot(user.Id);

            if (!canUploadBot)
            {
                this.ModelState.AddModelError<AccountSubmitViewModel>(vm => vm.BotFile, "You must wait longer before uploading a new bot");
                return this.View("Submit", viewModel);
            }

            using (var transaction = await this.Repository.CreateTransaction())
            {
                var newBotId = await this.Repository.AddBot(user.Id);
                await this.Storage.UploadBotSourceCode(newBotId, viewModel.BotFile.OpenReadStream());
                await this.Repository.AddCompilationTask(newBotId);
                await transaction.CommitAsync();
            }

            return this.RedirectToAction("Index", "Account");
        }

        private async Task<bool> CanSubmitBot(Guid userId)
        {
            var bots = await this.Repository.GetBots(userId).ConfigureAwait(false);
            var mostRecentBot = bots.OrderByDescending(b => b.Updated).FirstOrDefault();
            if (mostRecentBot == null)
                return true;

            return (DateTime.UtcNow - mostRecentBot.Updated) > BotUploadDelay;
        }

        [HttpGet("bot/{botId}/download")]
        public async Task<IActionResult> DownloadBot(Guid botId)
        {
            var bot = await this.Repository.GetBot(botId);
            var user = await this.GetAuthenticatedUser();

            if (bot.UserId != user.Id)
                return this.Forbid("This bot does not belong to you");

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
