using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;

namespace Bomberjam.Website.Controllers
{
    public class WebController : BaseBomberjamController<WebController>
    {
        private readonly GitHubConfiguration _github;

        public WebController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<WebController> logger, GitHubConfiguration github)
            : base(repository, storage, logger)
        {
            this._github = github;
        }

        [HttpGet("~/")]
        public IActionResult Index()
        {
            return this.View(new WebHomeViewModel { StarterKitsArtifactsUrl = this._github.StartKitsArtifactsUrl });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("~/leaderboard")]
        public async Task<IActionResult> Leaderboard()
        {
            var rankedUsers = await this.Repository.GetRankedUsers();
            return this.View(rankedUsers.ToList());
        }

        [HttpGet("~/game/{gameId}")]
        public async Task<IActionResult> GameDetails(Guid gameId)
        {
            var game = await this.Repository.GetGame(gameId);
            return this.View(game);
        }

        [HttpGet("~/viewer")]
        public IActionResult Viewer()
        {
            return this.View();
        }

        [HttpGet("~/user/{userId}")]
        public async Task<IActionResult> UserDetails(Guid userId, [FromQuery(Name = "season")] int? seasonId, int page = 1)
        {
            Season season;

            if (seasonId.HasValue)
            {
                try
                {
                    season = await this.Repository.GetSeason(seasonId.Value);
                }
                catch (EntityNotFound)
                {
                    return this.RedirectToAction("UserDetails", new { userId = userId, page = 1 });
                }
            }
            else
            {
                season = await this.Repository.GetCurrentSeason();
            }

            var user = await this.Repository.GetUserById(userId);
            var userGames = await this.Repository.GetPagedUserGames(userId, season.Id, Math.Max(1, page));

            return userGames.IsOutOfRange
                ? this.RedirectToAction("UserDetails", new { userId = userId, page = 1 })
                : this.View(new UserDetails(user, userGames));
        }

        [HttpGet("~/learn")]
        public IActionResult Learn()
        {
            return this.View();
        }
    }
}
