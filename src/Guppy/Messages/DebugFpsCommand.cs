using Guppy.CommandLine.Interfaces;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Messages
{
    public sealed class DebugFpsCommand : ICommandData
    {
        public Boolean ResetFps { get; init; }
    }
}
