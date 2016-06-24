using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comments.Models
{

    public class PagedList<T> : List<T>
    {
        public int TotalCount { get; private set; }
        public int PageCount { get; private set; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }

        public PagedList()
        {

        }


        public PagedList(ICollection<T> source, int page, int pageSize, int totalCount)
        {
            TotalCount = totalCount;

            PageCount = GetPageCount(pageSize, TotalCount);
            Page = page;
            PageSize = pageSize;

            AddRange(source);
        }

        public PagedList(IQueryable<T> source, bool fetchRecordCountOnly)
        {
            if (fetchRecordCountOnly)
                TotalCount = source.Count();
        }

        public PagedList(IQueryable<T> source, int page, int pageSize)
        {
            TotalCount = source.Count();
            PageCount = GetPageCount(pageSize, TotalCount);
            Page = page < 1 ? int.MaxValue : page - 1;
            PageSize = pageSize;

            if (Page == int.MaxValue)
                AddRange(source);
            else
                AddRange(source.Skip(Page * PageSize).Take(PageSize).ToList());
        }

        private int GetPageCount(int pageSize, int totalCount)
        {
            if (pageSize == 0)
                return 0;

            var remainder = totalCount % pageSize;
            return (totalCount / pageSize) + (remainder == 0 ? 0 : 1);
        }
    }



}
