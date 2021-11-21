using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
using Bomberjam.Website.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    public abstract class BaseBomberjamController : Controller
    {
        protected static readonly SemaphoreSlim GetNextTaskMutex = new(1);
        protected static readonly SemaphoreSlim ComputeAllUserGlobalRanksMutex = new(1);
    }

    public abstract class BaseBomberjamController<T> : BaseBomberjamController
        where T : Controller
    {
        protected BaseBomberjamController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<T> logger)
        {
            this.Repository = repository;
            this.Storage = storage;
            this.Logger = logger;
        }

        protected IBomberjamRepository Repository { get; }

        protected IBomberjamStorage Storage { get; }

        protected ILogger<T> Logger { get; }

        protected Task<User> GetAuthenticatedUser()
        {
            var githubId = this.User.GetGithubId();
            if (!githubId.HasValue)
                throw new Exception("Could not retrieve GitHub name identifier claim from authentication result");

            return this.Repository.GetUserByGithubId(githubId.Value);
        }
        protected static string[] GetAllErrors(ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray();
        }
    }
}