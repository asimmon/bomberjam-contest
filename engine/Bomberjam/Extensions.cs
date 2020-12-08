using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomberjam
{
    internal static class Extensions
    {
        private static readonly Random Rng = new Random();

        internal static T PickRandom<T>(this IReadOnlyList<T> immutableItems)
        {
            return immutableItems[Rng.Next(immutableItems.Count)];
        }

        internal static IEnumerable<T> Shuffle<T>(this IEnumerable<T> immutableItems)
        {
            var mutableItems = immutableItems.ToList();
            ShuffleInPlace(mutableItems);
            return mutableItems;
        }

        internal static void ShuffleInPlace<T>(this IList<T> mutableItems)
        {
            var n = mutableItems.Count;

            while (n > 1) {
                n--;
                var k = Rng.Next(n + 1);
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