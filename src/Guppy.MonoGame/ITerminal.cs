﻿using Microsoft.Xna.Framework;
using System.CommandLine;

namespace Guppy.MonoGame
{
    public interface ITerminal : IConsole
    {
        TerminalTheme Theme { get; }

        internal new TextWriter Out { get; }

        Color Color { get; set; }

        void Write(string value);
        
        void Write(string value, Color color);

        void WriteLine(string value);

        void WriteLine(string value, Color color);

        void NewLine();
    }
}
