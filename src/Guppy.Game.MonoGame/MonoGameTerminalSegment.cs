using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.MonoGame
{
    internal class MonoGameTerminalSegment
    {
        public readonly Vector4 Color;
        public readonly string Text;

        public MonoGameTerminalSegment(Color color, string text)
        {
            this.Color = color.ToVector4();
            this.Text = text;
        }
    }
}
