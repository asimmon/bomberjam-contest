using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    public class BaseWebController<T> : Controller
        where T : Controller
    {
        public BaseWebController(IRepository repository, ILogger<T> logger)
        {
            this.Repository = repository;
            this.Logger = logger;
        }

        protected IRepository Repository { get; }

        protected ILogger<T> Logger { get; }

        protected bool TryGetAuthenticatedUserEmail(out string email)
        {
            var emailClaim = this.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim != null)
            {
                email = emailClaim.Value;
                return true;
            }

            email = null;
            return false;
        }

        protected Task<User> GetAuthenticatedUser()
        {
            var emailClaim = this.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                throw new Exception("TODO Not authenticated");
            }

            return this.Repository.GetUserByEmail(emailClaim.Value);
        }
    }
}