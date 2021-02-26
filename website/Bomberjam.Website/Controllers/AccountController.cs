using System;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
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
        public async Task<IActionResult> Submit(AccountSubmitViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
                return this.View("Submit", viewModel);

            var user = await this.GetAuthenticatedUser();
            var canUploadBot = await this.CanUploadBot(user.Id);

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

        private async Task<bool> CanUploadBot(Guid userId)
        {
            var bots = await this.Repository.GetBots(userId).ConfigureAwait(false);
            var mostRecentBot = bots.OrderByDescending(b => b.Updated).FirstOrDefault();
            if (mostRecentBot == null)
                return true;

            return (DateTime.UtcNow - mostRecentBot.Updated) > BotUploadDelay;
        }
    }
}
