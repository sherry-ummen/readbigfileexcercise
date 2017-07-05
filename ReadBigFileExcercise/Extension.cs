using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Linq;

namespace ReadBigFileExcercise
{
    public static class Extension
    {
        public static Dictionary<K, int> ToDictionary<K, V>(this IEnumerable<K> array)
        {
            return array
                .Select((v, i) => new { Key = v, Value = i })
                .ToDictionary(o => o.Key, o => o.Value);
        }

    }
}
