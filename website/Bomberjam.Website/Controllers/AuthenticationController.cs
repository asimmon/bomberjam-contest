using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.RegularExpressions;
using AspNet.Security.OAuth.GitHub;
using Bomberjam.Website.Common;
using Bomberjam.Website.Database;
using Bomberjam.Website.Storage;
using Bomberjam.Website.Utils;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    public class AuthenticationController : BaseBomberjamController<AuthenticationController>
    {
        public AuthenticationController(IBomberjamRepository repository, IBomberjamStorage storage, ILogger<AuthenticationController> logger)
            : base(repository, storage, logger)
        {
        }

        [HttpGet("~/signin")]
        public IActionResult SignIn()
        {
            if (this.User.IsAuthenticated())
                return this.RedirectToAction("Index", "Account");

            var redirectUri = this.Url.Action("SignInGithub", "Authentication");
            return this.Challenge(new AuthenticationProperties { RedirectUri = redirectUri }, GitHubAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("~/signin-github")]
        public async Task<IActionResult> SignInGithub()
        {
            var errorUri = this.Url.Action("Error", "Web");

            var authResult = await this.HttpContext.AuthenticateAsync(GitHubAuthenticationDefaults.AuthenticationScheme);
            if (authResult.Ticket == null)
                return this.SignOut();

            if (!authResult.Ticket.Principal.TryGetGitHubUserName(out var githubUserName) ||
                !authResult.Ticket.Principal.TryGetGitHubId(out int githubId))
            {
                return this.SignOut(errorUri);
            }

            try
            {
                await this.Repository.GetUserByGithubId(githubId);
            }
            catch (EntityNotFound)
            {
                try
                {
                    await ComputeAllUserGlobalRanksMutex.WaitAsync();

                    using (var transaction = await this.Repository.CreateTransaction())
                    {
                        var userName = await this.GetUniqueUserName(githubUserName);
                        await this.Repository.AddUser(githubId, userName);
                        await this.Repository.UpdateAllUserGlobalRanks();
                        await transaction.CommitAsync();
                    }
                }
                catch (Exception)
                {
                    return this.SignOut(errorUri);
                }
                finally
                {
                    ComputeAllUserGlobalRanksMutex.Release();
                }
            }

            return this.RedirectToAction("Index", "Account");
        }

        private static readonly Regex UserNameRegex = new Regex("^[a-zA-Z0-9]{2,32}$", RegexOptions.Compiled);

        private async Task<string> GetUniqueUserName(string githubUserName)
        {
            if (UserNameRegex.IsMatch(githubUserName))
            {
                var isAlreadyUsed = await this.Repository.IsUserNameAlreadyUsed(githubUserName).ConfigureAwait(false);
                if (!isAlreadyUsed)
                    return githubUserName;
            }

            while (true)
            {
                var randomUserName = "Player" + Constants.Rng.Next(100000, 999999).ToString(CultureInfo.InvariantCulture);
                var isAlreadyUsed = await this.Repository.IsUserNameAlreadyUsed(randomUserName).ConfigureAwait(false);
                if (!isAlreadyUsed)
                    return randomUserName;
            }
        }

        [HttpGet("~/signout")]
        [HttpPost("~/signout")]
        public new IActionResult SignOut()
        {
            var redirectUri = this.Url.Action("Index", "Web");
            return this.SignOut(redirectUri);
        }

        private IActionResult SignOut(string redirectUri)
        {
            return this.User.IsAuthenticated()
                ? base.SignOut(new AuthenticationProperties { RedirectUri = redirectUri }, CookieAuthenticationDefaults.AuthenticationScheme)
                : this.Redirect(redirectUri);
        }
    }
}