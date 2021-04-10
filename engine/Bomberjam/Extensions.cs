using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomberjam
{
    internal static class Extensions
    {
        internal static T PickRandom<T>(this IReadOnlyList<T> immutableItems, Random rng)
        {
            return immutableItems[rng.Next(immutableItems.Count)];
        }

        internal static IEnumerable<T> Shuffle<T>(this IEnumerable<T> immutableItems, Random rng)
        {
            var mutableItems = immutableItems.ToList();
            ShuffleInPlace(mutableItems, rng);
            return mutableItems;
        }

        internal static void ShuffleInPlace<T>(this IList<T> mutableItems, Random rng)
        {
            var n = mutableItems.Count;

            while (n > 1) {
                n--;
                var k = rng.Next(n + 1);
                var value = mutableItems[k];
                mutableItems[k] = mutableItems[n];
                mutableItems[n] = value;
            }
        }

        internal static bool IsValidAsciiMap(IReadOnlyCollection<string> asciiMap)
        {
            if (asciiMap.Count <= 1) return false;
            if (asciiMap.Any(string.IsNullOrEmpty)) return false;
            return asciiMap.SelectMany(l => l).All(c => Translator.CharacterToTileKindMappings.ContainsKey(c));
        }

        internal static TVal GetOrAdd<TKey, TVal>(this IDictionary<TKey, TVal> items, TKey key, Func<TVal> itemFactory)
            where TKey : notnull
        {
            return items.TryGetValue(key, out var item) ? item : items[key] = itemFactory();
        }
    }
}