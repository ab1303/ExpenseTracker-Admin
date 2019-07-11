using System;
using PartnerUser.Common.Models;

namespace PartnerUser.Api.Requests
{
    public class PartnerUserFilterRequest : PagedResourceBase
    {
        public bool? IsReadyToDeal { get; set; }
        public Guid? PartnerAppId { get; set; }
        public Guid? OfxUserGuid { get; set; }
        public Guid? PartnerUserId { get; set; }
        public Guid? BeneficiaryId { get; set; }
    }
}
