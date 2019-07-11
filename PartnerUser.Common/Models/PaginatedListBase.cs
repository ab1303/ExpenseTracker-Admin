using System;

namespace PartnerUser.Common.Models
{
    public abstract class PaginatedListBase
    {
        protected PaginatedListBase(int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Page = pageNumber;
            PageCount = (int)Math.Ceiling(count / (double)pageSize);
        }

        public int Page { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public int TotalCount { get; }
    }
}
