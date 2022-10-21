using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Commands
{
    public class ToggleWindow : Message, ICommandData
    {
        public enum Windows
        {
            Debugger,
            Terminal
        }

        public Windows Window { get; init; }
    }
}
