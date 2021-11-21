using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.RegularExpressions;
using AspNet.Security.OAuth.GitHub;
using Bomberjam.Website.Authentication;
using Bomberjam.Website.Common;
using Bomberjam.Website.Configuration;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
using Bomberjam.Website.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bomberjam.Website.Controllers
{
    public class AuthenticationController : BaseBomberjamController<AuthenticationController>
    {
        private readonly IOptions<GitHubOptions> _githubOptions;

        public AuthenticationController(
            IBomberjamRepository repository,
            IBomberjamStorage storage,
            IOptions<GitHubOptions> githubOptions,
            ILogger<AuthenticationController> logger)
            : base(repository, storage, logger)
        {
            this._githubOptions = githubOptions;
        }

        [HttpGet("~/signin")]
        public IActionResult SignIn()
        {
            if (this.User.IsAuthenticated())
                return this.RedirectToAction("Index", "Account");

            this.Logger.LogDebug("Redirecting to github for signin");
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

            var githubUserName = authResult.Ticket.Principal.FindFirstValue(ClaimTypes.Name);
            var githubId = authResult.Ticket.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (githubUserName == null || githubId == null)
                return this.SignOut(errorUri);

            User user;

            try
            {
                user = await this.Repository.GetUserByGithubId(githubId);
                this.Logger.LogInformation("User {UserId} signed in", user.Id);
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

                    user = await this.Repository.GetUserByGithubId(githubId);
                    this.Logger.LogInformation("Created new user {UserId}", user.Id);
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

            var role = this._githubOptions.Value.Administrators.Contains(githubId, StringComparer.Ordinal) ? BomberjamRoles.Admin : BomberjamRoles.User;

            var newClaims = new List<Claim>
            {
                new Claim(BomberjamClaimTypes.UserId, user.Id.ToString("D")),
                new Claim(BomberjamClaimTypes.GithubId, githubId),
                new Claim(BomberjamClaimTypes.GithubUserName, githubUserName),
                new Claim(BomberjamClaimTypes.Role, role),
            };

            var newIdentity = new ClaimsIdentity(newClaims, authResult.Ticket.AuthenticationScheme, BomberjamClaimTypes.UserId, BomberjamClaimTypes.Role);
            var newPrincipal = new ClaimsPrincipal(newIdentity);

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newPrincipal);
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
            if (this.User.IsAuthenticated())
            {
                var userId = this.User.GetUserId();
                this.Logger.LogInformation("Signing out user {UserId}", userId);
                return base.SignOut(new AuthenticationProperties { RedirectUri = redirectUri }, CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return this.Redirect(redirectUri);
        }
    }
}