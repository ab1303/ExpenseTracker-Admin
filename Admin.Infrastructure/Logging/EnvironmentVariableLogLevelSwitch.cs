using System;
using Admin.Common;
using Serilog.Core;
using Serilog.Events;

namespace Admin.Infrastructure.Logging
{
    public class EnvironmentVariableLoggingLevelSwitch : LoggingLevelSwitch
    {
        public EnvironmentVariableLoggingLevelSwitch()
        {
            if (Enum.TryParse<LogEventLevel>(StackVariables.LoggingLevelSwitchMinimumLevel, true, out var level))
            {
                MinimumLevel = level;
            }
        }
    }
}
