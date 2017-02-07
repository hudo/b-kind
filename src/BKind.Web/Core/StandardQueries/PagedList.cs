using System;
using System.Collections.Generic;
using System.Linq;

namespace BKind.Web.Core.StandardQueries
{
    public class PagedList<T> : IEnumerable<T>
    {
        private readonly List<T> _innerList;

        public PagedList(IEnumerable<T> source, int currentPage, int pageSize, int totalCount)
        {
            _innerList = source.ToList();
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public int TotalCount { get; private set; }

        public int PageSize { get; private set; }

        public int CurrentPage { get; private set; }

        public int TotalPages { get { return (int)Math.Ceiling((double)TotalCount / PageSize); } }

        public IEnumerator<T> GetEnumerator() { return _innerList.GetEnumerator(); }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return _innerList.GetEnumerator(); }
    }
}