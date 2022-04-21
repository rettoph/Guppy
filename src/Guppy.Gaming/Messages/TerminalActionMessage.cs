using Guppy.Gaming.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Messages
{
    internal struct TerminalActionMessage
    {
        public readonly TerminalAction Action;

        public TerminalActionMessage(TerminalAction action)
        {
            this.Action = action;
        }
    }
}
