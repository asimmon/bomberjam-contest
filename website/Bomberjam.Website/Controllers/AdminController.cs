using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Bomberjam.Website.Jobs;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    [Authorize]
    [Route("~/admin")]
    [AdministrationFilter]
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

        [HttpPost]
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

            this.Logger.Log(LogLevel.Information, "Manually queued one match with users: " + string.Join(", ", selectedUserIds));

            var successViewModel = await this.GetAdminIndexViewModelWithSuccess("Game queued for users: " + string.Join(", ", fetchedUsers.Select(u => u.UserName)));
            return this.View("Index", successViewModel);
        }

        private async Task<AdminIndexViewModel> GetAdminIndexViewModel(string errorMessage, string successMessage)
        {
            var workers = await this.Repository.GetWorkers(10).ConfigureAwait(false);
            var users = await this.Repository.GetAllUsers().ConfigureAwait(false);

            return new AdminIndexViewModel(workers, users, errorMessage, successMessage);
        }

        private Task<AdminIndexViewModel> GetAdminIndexViewModel()
        {
            return GetAdminIndexViewModel(null, null);
        }

        private Task<AdminIndexViewModel> GetAdminIndexViewModelWithError(string errorMessage = null)
        {
            return GetAdminIndexViewModel(errorMessage, null);
        }

        private Task<AdminIndexViewModel> GetAdminIndexViewModelWithSuccess(string successMessage = null)
        {
            return GetAdminIndexViewModel(null, successMessage);
        }
    }
}