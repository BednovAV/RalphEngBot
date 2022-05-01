using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public static class CollectionHelpers
    {
        public static T[][] Smash<T>(this IEnumerable<T> source, int chunkSize)
        {
            var result = new List<T[]>();
            while (source.Any())
            {
                result.Add(source.Take(chunkSize).ToArray());
                source = source.Skip(chunkSize);
            }

            return result.ToArray();
        }
    }
}
