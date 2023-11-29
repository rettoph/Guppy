using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    internal class TerminalSegment
    {
        public readonly Vector4 Color;
        public readonly string Text;

        public TerminalSegment(Color color, string text)
        {
            this.Color = color.ToVector4();
            this.Text = text;
        }
    }
}
