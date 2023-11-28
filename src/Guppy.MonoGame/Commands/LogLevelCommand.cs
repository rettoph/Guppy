using Guppy.Commands.Arguments;
using Guppy.Commands.Attributes;
using Guppy.Common;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Commands
{
    [Command]
    internal class LogLevelCommand : Message<LogLevelCommand>
    {
        public static LoggingLevelSwitch LoggingLevelSwitch = new LoggingLevelSwitch();

        [Option(names: new[] { "-value", "-v" }, required: true)]
        public LogEventLevel Value { get; set; }
    }
}
