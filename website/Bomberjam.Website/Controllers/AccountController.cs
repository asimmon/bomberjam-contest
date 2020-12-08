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
    public class AccountController : BaseWebController<AccountController>
    {
        private readonly IBotStorage _botStorage;

        public AccountController(IRepository repository, IBotStorage botStorage, ILogger<AccountController> logger)
            : base(repository, logger)
        {
            this._botStorage = botStorage;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.GetAuthenticatedUser();
            if (user.NeedsToCompleteProfile())
                return this.RedirectToAction("Edit", "Account");

            return this.View("Index", user);
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            var user = await this.GetAuthenticatedUser();
            var viewModel = new AccountEditViewModel
            {
                UserName = user.UserName
            };

            return this.View("Edit", viewModel);
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountEditViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
                return this.View("Edit", viewModel);

            var user = await this.GetAuthenticatedUser();
            user.UserName = viewModel.UserName;
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

            var hasActiveCompileTask = await this.Repository.DoesUserHaveActiveCompileTask(user.Id);
            if (hasActiveCompileTask)
            {
                this.ModelState.AddModelError<AccountSubmitViewModel>(vm => vm.BotFile, "You already have an active bot compilation task");
                return this.View("Submit", viewModel);
            }

            await this._botStorage.UploadBotSourceCode(user.Id, viewModel.BotFile.OpenReadStream());

            user.SubmitCount++;
            user.IsCompiled = false;
            user.IsCompiling = false;
            user.BotLanguage = string.Empty;
            user.CompilationErrors = string.Empty;

            await this.Repository.UpdateUser(user);
            await this.Repository.AddCompilationTask(user.Id);

            return this.RedirectToAction("Index", "Account");
        }
    }
}
