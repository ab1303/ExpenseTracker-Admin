using System;
using PartnerUser.Api.Validation;

namespace PartnerUser.Api.Requests
{
    public class CreatePartnerUserRequest
    {
        [NotNullOrEmptyGuid]
        public Guid PartnerAppId { get; set; }
        [NotNullOrEmptyGuid]
        public Guid OfxUserGuid { get; set; }
    }
}
