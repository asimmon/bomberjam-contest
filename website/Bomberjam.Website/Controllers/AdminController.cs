using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Authentication;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    [Route("~/admin")]
    [Authorize(Roles = BomberjamRoles.Admin)]
    public class AdminController : BaseBomberjamController<AdminController>
    {
        public AdminController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<AdminController> logger)
            : base(repository, storage, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = await this.GetAdminIndexViewModel();
            return this.View(viewModel);
        }

        [HttpGet("start-new-season")]
        public async Task<IActionResult> StartNewSeason()
        {
            var actualSeason = await this.Repository.GetCurrentSeason();
            this.Logger.LogInformation("Closing current season {SeasonId} with name {SeasonName}", actualSeason.Id, actualSeason.Name);

            using (var transaction = await this.Repository.CreateTransaction())
            {
                await this.Repository.StartNewSeason();
                await transaction.CommitAsync();
            }

            var newSeason = await this.Repository.GetCurrentSeason();
            this.Logger.LogInformation("Starting new season {SeasonId} with name {SeasonName}", newSeason.Id, newSeason.Name);

            return this.RedirectToAction("Index", "Admin");
        }

        [HttpPost("start-game")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartGame(SelectableUser[] users)
        {
            var selectedUserIds = new HashSet<Guid>(users.Where(u => u.IsSelected).Select(u => u.Id));
            if (selectedUserIds.Count != 4)
            {
                var errorViewModel = await this.GetAdminIndexViewModelWithError("You need to select exactly four users");
                return this.View("Index", errorViewModel);
            }

            var fetchedUsers = new List<User>();

            foreach (var selectedUserId in selectedUserIds)
            {
                var user = await this.Repository.GetUserById(selectedUserId);
                var bots = await this.Repository.GetBots(selectedUserId);

                if (bots.All(b => b.Status != CompilationStatus.CompilationSucceeded))
                {
                    var errorViewModel = await this.GetAdminIndexViewModelWithError($"User {user.UserName} does not have a compiled bot yet");
                    return this.View("Index", errorViewModel);
                }

                fetchedUsers.Add(user);
            }

            await this.Repository.AddGameTask(fetchedUsers, GameOrigin.OnDemand);

            this.Logger.LogInformation("Manually queued one match with users: {UserIds}", string.Join(", ", selectedUserIds));

            var successViewModel = await this.GetAdminIndexViewModelWithSuccess("Game queued for users: " + string.Join(", ", fetchedUsers.Select(u => u.UserName)));
            return this.View("Index", successViewModel);
        }

        private async Task<AdminIndexViewModel> GetAdminIndexViewModel(string errorMessage, string successMessage)
        {
            var workers = await this.Repository.GetWorkers(10).ConfigureAwait(false);
            var users = await this.Repository.GetAllUsers().ConfigureAwait(false);
            var seasons = await this.Repository.GetSeasons().ConfigureAwait(false);

            return new AdminIndexViewModel(workers, seasons, users.OrderBy(u => u.GlobalRank), errorMessage, successMessage);
        }

        private Task<AdminIndexViewModel> GetAdminIndexViewModel()
        {
            return this.GetAdminIndexViewModel(null, null);
        }

        private Task<AdminIndexViewModel> GetAdminIndexViewModelWithError(string errorMessage = null)
        {
            return this.GetAdminIndexViewModel(errorMessage, null);
        }

        private Task<AdminIndexViewModel> GetAdminIndexViewModelWithSuccess(string successMessage = null)
        {
            return this.GetAdminIndexViewModel(null, successMessage);
        }
    }
}