using System.Collections.Generic;
using Admin.Common.Models;

namespace Admin.Api.Responses
{
    public class PartnerUsersPaginatedResponse : PaginatedListBase
    {
        public PartnerUsersPaginatedResponse(int count, int pageNumber, int pageSize) : base(count, pageNumber, pageSize)
        {
        }

        public IEnumerable<PartnerUserResponse> Users { get; set; }
    }
}
