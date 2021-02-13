using System;
using System.Collections.Generic;

namespace Bomberjam.Website.Models
{
    public class PaginationModel<T> where T : class
    {
        public PaginationModel(IReadOnlyCollection<T> items, int currentPage, int pageSize, int totalCount)
        {
            this.Items = items;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.PageCount = (int)Math.Ceiling(1d * TotalCount / PageSize);
        }

        public IReadOnlyCollection<T> Items { get; }

        public int CurrentPage { get; }

        public int PageSize { get; }

        public int TotalCount { get; }

        public int PageCount { get; }
    }
}