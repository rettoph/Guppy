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
        public readonly Vector4 ForegroundColor;
        public readonly Vector4 BackgroundColor;
        public readonly string Text;
        public readonly bool NewLine;

        public TerminalSegment(Color foregroundColor, Color backgroundColor, string text)
        {
            this.ForegroundColor = foregroundColor.ToVector4();
            this.BackgroundColor = backgroundColor.ToVector4();

            this.NewLine = text.StartsWith(Environment.NewLine);

            if(this.NewLine)
            {
                this.Text = text[2..];
            }
            else
            {
                this.Text = text;
            }
        }
    }
}
