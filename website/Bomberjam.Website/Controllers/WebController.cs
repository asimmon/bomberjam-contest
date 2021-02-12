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
            var games = await this.Repository.GetGames();
            return this.View(new HomeModel(users.ToList(), games.ToList()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("~/visualizer")]
        public IActionResult Visualizer()
        {
            return this.View("Visualizer");
        }

        [HttpGet("~/leaderboard")]
        public async Task<IActionResult> Leaderboard()
        {
            var rankedUsers = await this.Repository.GetRankedUsers();
            return this.View(rankedUsers);
        }

        [HttpGet("~/user/{userId}")]
        public IActionResult UserDetails(Guid userId)
        {
            return this.View(new UserDetail());
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
