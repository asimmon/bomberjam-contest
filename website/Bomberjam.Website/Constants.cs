using System;
using Bomberjam.Website.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Bomberjam.Website
{
    public static class Constants
    {
        public const int InitialPoints = 1500;

        public const string ApiPrincipalEmail = "api@bomberjam.com";

        public const string SupportedAuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + SecretAuthenticationDefaults.AuthenticationScheme;

        public const int GamesPageSize = 25;

        public static readonly Random Rng = new Random();

        public static readonly Guid UserAskaiserId = new Guid("00000000-0000-0000-0000-000000000001");
        public static readonly Guid UserFalgarId = new Guid("00000000-0000-0000-0000-000000000002");
        public static readonly Guid UserXenureId = new Guid("00000000-0000-0000-0000-000000000003");
        public static readonly Guid UserMintyId = new Guid("00000000-0000-0000-0000-000000000004");
        public static readonly Guid UserKalmeraId = new Guid("00000000-0000-0000-0000-000000000005");
        public static readonly Guid UserPandarfId = new Guid("00000000-0000-0000-0000-000000000006");
        public static readonly Guid UserMireId = new Guid("00000000-0000-0000-0000-000000000007");
    }
}