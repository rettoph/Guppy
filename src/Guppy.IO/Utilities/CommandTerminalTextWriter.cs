﻿using Guppy.IO.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Guppy.IO.Utilities
{
    internal class CommandTerminalTextWriter : TextWriter
    {
        private Terminal _terminal;
        private Color _color;
        private String _string;

        public override Encoding Encoding => Encoding.Default;

        internal CommandTerminalTextWriter(Terminal terminal, ref Color color)
        {
            _terminal = terminal;
            _color = color;
            _string = String.Empty;
        }

        public override void Write(Char value)
        {
            _string += value;

            if (value == '\n')
            {
                _terminal.Write(_string, _color);
                _string = String.Empty;
            }
        }

        private Color FromConsoleColor(ConsoleColor consoleColor)
        {
            switch (consoleColor)
            {
                case ConsoleColor.Black:
                    return Color.Black;
                case ConsoleColor.Blue:
                    return Color.Blue;
                case ConsoleColor.Cyan:
                    return Color.Cyan;
                case ConsoleColor.DarkBlue:
                    return Color.DarkBlue;
                case ConsoleColor.DarkCyan:
                    return Color.DarkCyan;
                case ConsoleColor.DarkGray:
                    return Color.DarkGray;
                case ConsoleColor.DarkGreen:
                    return Color.DarkGreen;
                case ConsoleColor.DarkMagenta:
                    return Color.DarkMagenta;
                case ConsoleColor.DarkRed:
                    return Color.DarkRed;
                case ConsoleColor.DarkYellow:
                    return new Color(204, 204, 0);
                case ConsoleColor.Gray:
                    return Color.Gray;
                case ConsoleColor.Green:
                    return Color.Green;
                case ConsoleColor.Magenta:
                    return Color.Magenta;
                case ConsoleColor.Red:
                    return Color.Red;
                case ConsoleColor.White:
                    return Color.White;
                case ConsoleColor.Yellow:
                    return Color.Yellow;
            }

            return Color.White;
        }
    }
}
