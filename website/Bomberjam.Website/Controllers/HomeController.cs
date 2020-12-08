using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Controllers
{
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
            return this.View(users.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
