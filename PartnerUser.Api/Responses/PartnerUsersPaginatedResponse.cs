using System.Collections.Generic;
using PartnerUser.Common.Models;

namespace PartnerUser.Api.Responses
{
    public class PartnerUsersPaginatedResponse : PaginatedListBase
    {
        public PartnerUsersPaginatedResponse(int count, int pageNumber, int pageSize) : base(count, pageNumber, pageSize)
        {
        }

        public IEnumerable<PartnerUserResponse> Users { get; set; }
    }
}
