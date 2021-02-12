using System;
using System.Collections.Generic;

namespace Bomberjam.Website.Common
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list, Random rng)
        {
            var n = list.Count;
            while (n > 1) {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}