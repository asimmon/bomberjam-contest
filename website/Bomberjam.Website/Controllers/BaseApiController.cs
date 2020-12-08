using Bomberjam.Website.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    public class BaseApiController<T> : ControllerBase
        where T : ControllerBase
    {
        public BaseApiController(IRepository repository, ILogger<T> logger)
        {
            this.Repository = repository;
            this.Logger = logger;
        }

        protected IRepository Repository { get; }

        protected ILogger<T> Logger { get; }
    }
}