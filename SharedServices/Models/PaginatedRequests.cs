using SharedServices.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.UI.Models
{
    public class PaginatedRequests<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedRequests(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        public static async Task<PaginatedRequests<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountSafe();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsyncSafe();
            return new PaginatedRequests<T>(items, count, pageIndex, pageSize);
        }
    }
}