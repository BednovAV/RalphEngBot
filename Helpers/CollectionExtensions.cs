using Entities.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public static class CollectionExtensions
    {
        public static Random Random { get; } = new Random();

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
            var sourceArray = source.ToArray();
            return sourceArray[Random.Next(sourceArray.Length)];
        }

        public static IEnumerable<T> RandomItems<T>(this IEnumerable<T> source, int count)
        {
            var sourceArray = source.ToArray();
            for (int i = 0; i < count; i++)
            {
                yield return sourceArray[Random.Next(sourceArray.Length)];
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static List<T> GetShuffled<T>(this List<T> list)
        {
            var result = new List<T>(list);

            int n = result.Count;
            while (n > 1)
            {
                int k = Random.Next(n--);
                T temp = result[n];
                result[n] = result[k];
                result[k] = temp;
            }

            return result;
        }

        public static Page<T> GetPaged<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var data = source.ToList();
            return new Page<T>
            {
                PageSize = pageSize,
                Number = pageNumber,
                TotalCount = data.Count,
                TotalPages = (data.Count / pageSize) + 1,
                Data = data.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList()
            };
        }
    }
}
