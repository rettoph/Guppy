using Guppy.Common;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.Input
{
    public interface IButton
    {
        string Key { get; }
        ButtonSource DefaultSource { get; }
        ButtonSource Source { get; set; }
        bool Pressed { get; }

        bool SetPressed(bool pressed, [MaybeNullWhen(false)] out IInput message);
    }
}
