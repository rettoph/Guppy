using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Commands
{
    public struct ToggleWindow : ICommandData
    {
        public enum Windows
        {
            Debugger,
            Terminal
        }

        public Windows Window { get; init; }
    }
}
