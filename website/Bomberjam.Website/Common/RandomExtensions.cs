using System;

namespace Bomberjam.Website.Common
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}