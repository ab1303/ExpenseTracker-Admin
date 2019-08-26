using System.Collections.Generic;
using Admin.Common.Models;

namespace Admin.Api.Responses
{
    public class AdminPaginatedResponse : PaginatedListBase
    {
        public AdminPaginatedResponse(int count, int pageNumber, int pageSize) : base(count, pageNumber, pageSize)
        {
        }

        public IEnumerable<UserResponse> Users { get; set; }
    }
}
