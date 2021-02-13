using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Controllers
{
    public class WebController : BaseWebController<WebController>
    {
        public WebController(IRepository repository, ILogger<WebController> logger)
            : base(repository, logger)
        {
        }

        [HttpGet("~/")]
        public async Task<IActionResult> Index()
        {
            var users = await this.Repository.GetUsers();
            return this.View(new HomeModel(users.ToList(), new List<GameInfo>()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("~/visualizer")]
        public IActionResult Visualizer()
        {
            return this.View();
        }

        [HttpGet("~/game/{gameId}")]
        public IActionResult GameDetails(Guid gameId)
        {
            return this.View(gameId);
        }

        [HttpGet("~/leaderboard")]
        public async Task<IActionResult> Leaderboard()
        {
            var rankedUsers = await this.Repository.GetRankedUsers();
            return this.View(rankedUsers);
        }

        [HttpGet("~/user/{userId}")]
        public async Task<IActionResult> UserDetails(Guid userId, int page = 1)
        {
            page = Math.Max(0, page);

            var user = await this.Repository.GetUserById(userId);
            var userGames = await this.Repository.GetPagedUserGames(userId, page);

            return this.View(new UserDetails(user, userGames));
        }
    }

    public class HomeModel
    {
        public HomeModel(IReadOnlyList<User> users, IReadOnlyList<GameInfo> games)
        {
            this.Users = users;
            this.Games = games;
        }

        public IReadOnlyList<User> Users { get; }
        public IReadOnlyList<GameInfo> Games { get; }
    }
}
