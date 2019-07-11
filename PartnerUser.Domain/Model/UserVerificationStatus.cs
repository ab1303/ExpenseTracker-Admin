namespace PartnerUser.Domain.Model
{
    public class UserVerificationStatus
    {
        public string VerificationStatus { get; set; }
        public bool VerificationDocumentsSubmitted { get; set; }
        public bool SocialSecurityNumberRequired { get; set; }
    }
}
