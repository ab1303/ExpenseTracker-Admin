using System;
using Admin.Api.Validation;

namespace Admin.Api.Requests
{
    public class CreatePartnerUserRequest
    {
        [NotNullOrEmptyGuid]
        public Guid PartnerAppId { get; set; }
        [NotNullOrEmptyGuid]
        public Guid OfxUserGuid { get; set; }
    }
}
