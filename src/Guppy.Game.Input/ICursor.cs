using Guppy.Game.Input.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.Input
{
    public interface ICursor
    {
        Guid Id { get; }
        Vector2 Position { get; }

        bool this[CursorButtons button] { get; }

        int Scroll { get; }
    }
}
