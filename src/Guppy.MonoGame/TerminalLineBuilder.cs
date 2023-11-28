using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    internal class TerminalLineBuilder
    {
        private static readonly Color DarkYellow = new Color(139, 128, 0);

        private TerminalLine _line;

        public ConsoleColor ConsoleForegroundColor;
        public Color XnaForegroundColor;

        public ConsoleColor ConsoleBackgroundColor;
        public Color XnaBackgroundColor;

        public StringBuilder Text;

        public TerminalLineBuilder()
        {
            _line = TerminalLine.Factory.GetInstance();
            this.Text = new StringBuilder();
        }

        public bool TryAppend(char value, ConsoleColor consoleForegroundColor, ConsoleColor consoleBackgroundColor, [MaybeNullWhen(true)] out TerminalLine previousLine)
        {
            if(value == '\n')
            {
                this.AddSegment();

                previousLine = _line;

                _line = TerminalLine.Factory.GetInstance();
                _line.Segments.Clear();

                return false;
            }

            previousLine = null;

            if (this.ConsoleForegroundColor == consoleForegroundColor && this.ConsoleBackgroundColor == consoleBackgroundColor)
            {
                this.Text.Append(value);

                return true;
            }

            this.AddSegment();

            this.ConsoleForegroundColor = consoleForegroundColor;
            this.XnaForegroundColor = ToXnaColor(this.ConsoleForegroundColor);

            this.ConsoleBackgroundColor = consoleBackgroundColor;
            this.XnaBackgroundColor = ToXnaColor(this.ConsoleBackgroundColor);

            this.Text.Append(value);

            return true;
        }

        private void AddSegment()
        {
            TerminalSegment segment = new TerminalSegment(this.XnaForegroundColor, this.XnaBackgroundColor, this.Text.ToString());
            _line.Segments.Add(segment);

            this.Text.Clear();
        }
        private Color ToXnaColor(ConsoleColor consoleColor)
        {
            switch (consoleColor)
            {
                case ConsoleColor.Black:
                    return Color.Black;
                case ConsoleColor.DarkBlue:
                    return Color.DarkBlue;
                case ConsoleColor.DarkGreen:
                    return Color.DarkGreen;
                case ConsoleColor.DarkCyan:
                    return Color.DarkCyan;
                case ConsoleColor.DarkRed:
                    return Color.DarkRed;
                case ConsoleColor.DarkMagenta:
                    return Color.DarkMagenta;
                case ConsoleColor.DarkYellow:
                    return TerminalLineBuilder.DarkYellow;
                case ConsoleColor.Gray:
                    return Color.Gray;
                case ConsoleColor.DarkGray:
                    return Color.DarkGray;
                case ConsoleColor.Blue:
                    return Color.Blue;
                case ConsoleColor.Green:
                    return Color.Green;
                case ConsoleColor.Cyan:
                    return Color.Cyan;
                case ConsoleColor.Red:
                    return Color.Red;
                case ConsoleColor.Magenta:
                    return Color.Magenta;
                case ConsoleColor.Yellow:
                    return Color.Yellow;
                case ConsoleColor.White:
                default:
                    return Color.White;
            }
        }
    }
}
