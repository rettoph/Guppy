using Guppy.Game.Commands;
using Guppy.Game.Commands.Arguments;
using Guppy.Game.Commands.Attributes;
using Guppy.Core.Messaging.Common;
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
