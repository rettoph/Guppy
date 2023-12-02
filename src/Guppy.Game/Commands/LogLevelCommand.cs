using Guppy.Commands;
using Guppy.Commands.Arguments;
using Guppy.Commands.Attributes;
using Guppy.Common;
using Guppy.Messaging;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
