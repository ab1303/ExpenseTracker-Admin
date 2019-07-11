using System;

namespace PartnerUser.Persistence.Entity
{
    public class PartnerUser
    {
        public Guid PartnerUserId { get; set; }
        public Guid PartnerAppId { get; set; }
        public Guid OfxUserGuid { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? BeneficiaryId { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
