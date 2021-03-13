using System;

namespace Bomberjam.Website
{
    public static class Constants
    {
        public const int InitialPoints = 1500;
        public const int GamesPageSize = 25;

        public const int GeneralMaxUploadSize = 5 * 1024 * 1024;
        public const int BotSourceCodeMaxUploadSize = 25 * 1024 * 1024;
        public const int CompiledBotMaxUploadSize = 100 * 1024 * 1024;

        public const string ApiPrincipalFakeEmail = "api@bomberjam.com";
        public const string EmailRegexPattern = "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";

        public static readonly Random Rng = new Random();
    }
}