using System.Collections.Generic;

namespace Bomberjam.Website.Utils
{
    public static class EnumerableExtensions
    {
        public static string JoinStrings(this IEnumerable<string> texts, string separator)
        {
            return string.Join(separator, texts);
        }
    }
}