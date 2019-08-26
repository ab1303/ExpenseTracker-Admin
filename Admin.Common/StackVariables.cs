using System;

namespace Admin.Common
{
    public static class StackVariables
    {
        public static string LoggingLevelSwitchMinimumLevel => Environment.GetEnvironmentVariable("LoggingLevelSwitchMinimumLevel");
        public static string BslBaseAddress => Environment.GetEnvironmentVariable("BslBaseAddress");
        public static string UserDbConnString => Environment.GetEnvironmentVariable("AdminDB__ConnectionString");
    }
}
