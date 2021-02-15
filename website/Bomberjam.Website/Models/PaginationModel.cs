using System;
using System.Collections.Generic;
using System.Linq;

namespace Bomberjam.Website.Models
{
    public class PaginationModel<T> where T : class
    {
        public PaginationModel(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
        {
            this.Items = items.ToList();
            this.TotalCount = totalCount;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.PageCount = (int)Math.Ceiling(1d * TotalCount / PageSize);
        }

        public IReadOnlyCollection<T> Items { get; }

        public int TotalCount { get; }

        public int CurrentPage { get; }

        public int PageSize { get; }

        public int PageCount { get; }
    }
}