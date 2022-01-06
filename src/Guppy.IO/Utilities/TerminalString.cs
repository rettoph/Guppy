using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Utilities
{
    internal readonly struct TerminalString
    {
        public readonly Color Color;
        public readonly String Text;
        public readonly Boolean NewLine;

        public TerminalString(String text, Color color, Boolean newLine)
        {
            this.Text = text;
            this.Color = color;
            this.NewLine = newLine;
        }
    }
}
