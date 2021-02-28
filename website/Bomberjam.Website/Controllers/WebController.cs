using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;

namespace Bomberjam.Website.Controllers
{
    public class WebController : BaseBomberjamController<WebController>
    {
        public WebController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<WebController> logger)
            : base(repository, storage, logger)
        {
        }

        [HttpGet("~/")]
        public IActionResult Index()
        {
            return this.View();
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
            return this.View(rankedUsers);
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
        public async Task<IActionResult> UserDetails(Guid userId, int page = 1)
        {
            var user = await this.Repository.GetUserById(userId);
            var userGames = await this.Repository.GetPagedUserGames(userId, Math.Max(0, page));

            return this.View(new UserDetails(user, userGames));
        }

        [HttpGet("~/learn")]
        public IActionResult Learn()
        {
            return this.View();
        }
    }
}
