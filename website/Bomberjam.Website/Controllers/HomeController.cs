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

    public class HomeController : BaseWebController<HomeController>
    {
        public HomeController(IRepository repository, ILogger<HomeController> logger)
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
    }
}
