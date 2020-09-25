using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands
{
    public abstract class CommandContext : CommandGroupContext, ICommandContext
    {
        public abstract ArgContext[] Arguments { get; }

        public abstract Object BuildData(IReadOnlyDictionary<String, Object> args);
    }
}
