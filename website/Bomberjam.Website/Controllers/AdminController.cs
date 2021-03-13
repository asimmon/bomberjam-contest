using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
using Bomberjam.Website.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    [Authorize]
    [Route("~/admin")]
    public class AdminController : BaseBomberjamController<AdminController>
    {
        private readonly GitHubConfiguration _gitHubConfiguration;

        public AdminController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<AdminController> logger, GitHubConfiguration gitHubConfiguration)
            : base(repository, storage, logger)
        {
            this._gitHubConfiguration = gitHubConfiguration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!this.IsAdministrator())
                return this.Forbid();

            var workers = await this.Repository.GetWorkers(20);
            var users = await this.Repository.GetAllUsers();

            return this.View(new AdminIndexViewModel(workers, users));
        }

        private bool IsAdministrator()
        {
            return this.User.Claims.TryGetGitHubId(out string githubId) && this._gitHubConfiguration.AllowedGitHubIds.Contains(githubId);
        }
    }
}