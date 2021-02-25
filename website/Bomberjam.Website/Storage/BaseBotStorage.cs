using System;
using System.Globalization;

namespace Bomberjam.Website.Storage
{
    public abstract class BaseBotStorage
    {
        protected static string MakeBotFileName(Guid userId, bool isCompiled)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}-{1}.zip", userId.ToString("D"), isCompiled ? 1 : 0);
        }

        protected static string MakeGameFileName(Guid gameId)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.json", gameId.ToString("D"));
        }
    }
}