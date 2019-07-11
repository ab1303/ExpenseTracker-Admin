namespace PartnerUser.Common.Models
{
    public abstract class PagedResourceBase
    {
        public const int MaxPageSize = 25;
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 10;
        
        public int PageNumber { get; set; } = DefaultPageNumber;

        private int _pageSize = DefaultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
