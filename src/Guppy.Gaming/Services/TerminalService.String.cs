using Minnow.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Guppy.Gaming.Services
{
    internal partial class TerminalService
    {
        private readonly struct Line
        {
            public readonly string Text;
            public readonly XnaColor Color;

            public Line(string text, XnaColor color)
            {
                this.Text = text;
                this.Color = color;
            }
        }
    }
}
