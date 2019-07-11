using System;

namespace PartnerUser.Common
{
    public static class StackVariables
    {
        public static string LoggingLevelSwitchMinimumLevel => Environment.GetEnvironmentVariable("LoggingLevelSwitchMinimumLevel");
        public static string BslBaseAddress => Environment.GetEnvironmentVariable("BslBaseAddress");
        public static string PartnerUserDbConnString => Environment.GetEnvironmentVariable("PartnerUserDB__ConnectionString");
    }
}
