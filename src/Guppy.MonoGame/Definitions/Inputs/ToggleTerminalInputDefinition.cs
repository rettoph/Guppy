using Guppy.Attributes;
using Guppy.MonoGame.Commands;
using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Definitions.Inputs
{
    [AutoLoad]
    internal sealed class ToggleTerminalInputDefinition : InputDefinition<ToggleTerminal>
    {
        public override string Key => "toggle_terminal";

        public override InputSource DefaultSource => Keys.OemTilde;

        public override (ButtonState, ToggleTerminal)[] Data => new[]
        {
            (ButtonState.Pressed, new ToggleTerminal())
        };
    }
}
