namespace Admin.Services.Results
{
    public class Error
    {
        public string ErrorCode { get; set; }
        public string SystemMessage { get; set; }
    }

    public static class ErrorCodes
    {
        public static string DuplicateUserInsert = "U:DUPLICATE:0001";
        public static string UserNotFound = "U:NOTFOUND:0001";
        public static string UnknownError = "GEN:UNKNOWN";
        public static string RequestInvalidGeneric = "U:REQUEST:0001";
        public static string RequestPropertyNotSet = "U:REQUEST:0002";
    }
}
