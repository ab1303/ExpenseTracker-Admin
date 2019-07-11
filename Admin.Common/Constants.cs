namespace Admin.Common
{
    public static class Constants
    {
        public const string CorrelationIdHeaderKey = "X-OFX-CorrelationId";
        public const string ApplicationName = "PartnerUser Service";
        public const string BslUserApiBaseUrl = "/User.ApiService";
        public const string AuthorizationHeaderName = "Authorization";
        public const string VerifiedVerificationStatus = "Verified";

        public static class ApiVersion
        {
            public const string V1 = "v1";
        }
    }
}
