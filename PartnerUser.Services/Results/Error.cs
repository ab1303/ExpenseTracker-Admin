namespace PartnerUser.Services.Results
{
    public class Error
    {
        public string ErrorCode { get; set; }
        public string SystemMessage { get; set; }
    }

    public static class ErrorCodes
    {
        public static string DuplicatePartnerUserInsert = "PU:DUPLICATE:0001";
        public static string PartnerUserNotFound = "PU:NOTFOUND:0001";
        public static string UnknownError = "GEN:UNKNOWN";
        public static string RequestInvalidGeneric = "PU:REQUEST:0001";
        public static string RequestPropertyNotSet = "PU:REQUEST:0002";
    }
}
