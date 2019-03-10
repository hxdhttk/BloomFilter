using System.Collections.Generic;
using System.Linq;

namespace BloomFilter
{
    public static class StringExtensions
    {
        private const int _cap = 1 << 29;

        private static readonly int[] _seeds = { 3, 5, 7, 11, 13, 31, 37, 61 };

        public static HashSet<int> Hash(this string value)
        {
            return _seeds.Select(seed => Hash(value, seed)).ToHashSet();
        }

        public static int Hash(this string value, int seed)
        {
            var result = 0;

            foreach (var character in value)
            {
                result = seed * result + character;
            }

            return (_cap - 1) & result;
        }
    }
}
