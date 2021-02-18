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

        public static IEnumerable<T> GetNeighbors<T>(this IReadOnlyList<T> items, int fromIndex, int neighborCount)
        {
            if (neighborCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(neighborCount), $"{nameof(neighborCount)} must be lesser than the items element count");

            var itemsYieldedCount = 0;

            for (var increment = 1; itemsYieldedCount < neighborCount; increment++)
            {
                var negIndex = fromIndex - increment;
                var posIndex = fromIndex + increment;

                var isNegIndexOutOfBound = false;
                var isPosIndexOutOfBound = false;

                if (negIndex >= 0)
                {
                    yield return items[negIndex];
                    if (++itemsYieldedCount >= neighborCount) yield break;
                }
                else
                {
                    isNegIndexOutOfBound = true;
                }

                if (posIndex < items.Count)
                {
                    yield return items[posIndex];
                    if (++itemsYieldedCount >= neighborCount) yield break;
                }
                else
                {
                    isPosIndexOutOfBound = true;
                }

                if (isNegIndexOutOfBound && isPosIndexOutOfBound)
                {
                    yield break;
                }
            }
        }
    }
}