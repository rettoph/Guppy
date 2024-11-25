using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Messaging.Common;
using Serilog.Core;
using Serilog.Events;

namespace Guppy.Game
{
    [Command]
    internal class LogLevelCommand : Message<LogLevelCommand>, ICommand
    {
        public static LoggingLevelSwitch LoggingLevelSwitch = new(LogEventLevel.Verbose);

        [Argument]
        public LogEventLevel? Value { get; set; }
    }
}
