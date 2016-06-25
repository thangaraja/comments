using Comments.Models;
using System.Collections.Generic;
using System.Linq;

namespace Comments.Helpers
{
    public static class QueryableExtension
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int page, int pageSize)
        {
            return new PagedList<T>(source, page, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, bool fetchCountOnly)
        {
            return new PagedList<T>(source, fetchCountOnly);
        }

        public static PagedList<T> ToPagedList<T>(this ICollection<T> source, int page, int pageSize, int totalCount)
        {
            return new PagedList<T>(source, page, pageSize, totalCount);
        }
    }
}