using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.UI.Extensions
{
    public static class Extensions
    {
        public static Task<List<TSource>> ToListAsyncSafe<TSource>(
            this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!(source is IAsyncEnumerable<TSource>))
                return Task.FromResult(source.ToList());
            return source.ToListAsync();
        }

        public static Task<int> CountSafe<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!(source is IAsyncEnumerable<TSource>))
                return Task.FromResult(source.Count());
            return source.CountAsync();
        }
    }
}