using Guppy.Core.Messaging.Common;
using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Serilog.Core;
using Serilog.Events;

namespace Guppy.Game
{
    [Command]
    internal class LogLevelCommand : Message<LogLevelCommand>, ICommand
    {
        public static LoggingLevelSwitch LoggingLevelSwitch = new LoggingLevelSwitch();

        [Argument]
        public LogEventLevel? Value { get; set; }
    }
}
