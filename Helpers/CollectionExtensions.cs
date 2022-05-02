using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public static class CollectionExtensions
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

        public static T RandomItem<T>(this IEnumerable<T> source)
        {
            var rnd = new Random();
            var sourceArray = source.ToArray();
            return sourceArray[rnd.Next(sourceArray.Length)];
        }

        public static IEnumerable<T> RandomItems<T>(this IEnumerable<T> source, int count)
        {
            var rnd = new Random();
            var sourceArray = source.ToArray();
            for (int i = 0; i < count; i++)
            {
                yield return sourceArray[rnd.Next(sourceArray.Length)];
            }
        }

        public static List<T> GetShuffled<T>(this List<T> list)
        {
            var rnd = new Random();
            var result = new List<T>(list);

            int n = result.Count;
            while (n > 1)
            {
                int k = rnd.Next(n--);
                T temp = result[n];
                result[n] = result[k];
                result[k] = temp;
            }

            return result;
        }
    }
}
