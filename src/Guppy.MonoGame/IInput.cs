using Guppy.Common;
using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public interface IInput
    {
        string Key { get; }
        InputSource DefaultSource { get; }
        InputSource Source { get; set; }

        bool Message(bool pressed, [MaybeNullWhen(false)] out IMessage message);
    }
}
