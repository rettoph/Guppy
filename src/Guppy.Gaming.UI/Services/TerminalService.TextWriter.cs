using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework;

namespace Guppy.Gaming.UI.Services
{
    internal partial class TerminalService
    {
        public class TextWriter : System.IO.TextWriter
        {
            private const char NewLineChar = '\n';
            private const char CarriageReturnChar = '\r';

            private string _line;
            private XnaColor? _color;
            private TerminalService _terminal;

            public override Encoding Encoding { get; } = Encoding.Default;

            public TextWriter(TerminalService terminal, XnaColor? color)
            {
                _line = string.Empty;
                _color = color;
                _terminal = terminal;
            }

            public override void Write(char value)
            {
                if(value == NewLineChar)
                {
                    _terminal.WriteLine(_line, _color ?? TextWriter.GetColor(Console.ForegroundColor));
                    _line = string.Empty;
                    return;
                }

                if(value == CarriageReturnChar)
                {
                    return;
                }

                _line += value;
            }

            private static XnaColor GetColor(ConsoleColor color)
            {
                switch (color)
                {
                    case ConsoleColor.Black:
                        return XnaColor.Black;
                    case ConsoleColor.DarkBlue:
                        return XnaColor.DarkBlue;
                    case ConsoleColor.DarkGreen:
                        return XnaColor.DarkGreen;
                    case ConsoleColor.DarkCyan:
                        return XnaColor.DarkCyan;
                    case ConsoleColor.DarkRed:
                        return XnaColor.DarkRed;
                    case ConsoleColor.DarkMagenta:
                        return XnaColor.DarkMagenta;
                    case ConsoleColor.DarkYellow:
                        return new XnaColor(204, 119, 34);
                    case ConsoleColor.Gray:
                        return XnaColor.Gray;
                    case ConsoleColor.DarkGray:
                        return XnaColor.DarkGray;
                    case ConsoleColor.Blue:
                        return XnaColor.Blue;
                    case ConsoleColor.Green:
                        return XnaColor.Green;
                    case ConsoleColor.Cyan:
                        return XnaColor.Cyan;
                    case ConsoleColor.Red:
                        return XnaColor.Red;
                    case ConsoleColor.Magenta:
                        return XnaColor.Magenta;
                    case ConsoleColor.Yellow:
                        return XnaColor.Yellow;
                    case ConsoleColor.White:
                        return XnaColor.White;
                    default:
                        throw new ArgumentException(nameof(color));
                }
            }
        }
    }
}
