using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Extensions
{
    public static class CollectionExtension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
      this IEnumerable<TSource> source,
      Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> hash = new HashSet<TKey>();
            return source.Where<TSource>((Func<TSource, bool>)(p => hash.Add(keySelector(p))));
        }
    }
}
