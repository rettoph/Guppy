using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Messages.Inputs
{
    public class ToggleWindowInput : Message
    {
        public enum Windows
        {
            Debugger,
            Terminal
        }

        public Windows Window { get; init; }
    }
}
