using System;

namespace PartnerUser.Api.Responses
{
    public class PartnerUserResponse
    {
        public Guid PartnerUserId { get; set; }
        public Guid PartnerAppId { get; set; }
        public Guid OfxUserGuid { get; set; }
        public Guid? BeneficiaryId { get; set; }
        public bool IsReadyToDeal { get; set; }
    }
}
