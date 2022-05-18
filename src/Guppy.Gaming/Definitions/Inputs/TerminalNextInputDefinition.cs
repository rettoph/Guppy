using Guppy.Attributes;
using Guppy.Gaming.Constants;
using Guppy.Gaming.Enums;
using Guppy.Gaming.Messages;
using Guppy.Gaming.Structs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Definitions.Inputs
{
    [AutoLoad]
    internal sealed class TerminalNextInputDefinition : InputDefinition<TerminalActionMessage>
    {
        public override string Key { get; } = InputConstants.TerminalNext;

        public override InputSource DefaultSource { get; } = Keys.Down;

        public override bool GetOnPress(IServiceProvider provider, [MaybeNullWhen(false)] out TerminalActionMessage command)
        {
            command = new TerminalActionMessage(TerminalAction.Next);
            return true;
        }

        public override bool GetOnRelease(IServiceProvider provider, [MaybeNullWhen(false)] out TerminalActionMessage command)
        {
            command = default;
            return false;
        }
    }
}
