﻿using Guppy.Core.Common.Collections;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;
using Serilog.Events;
using System.CommandLine.IO;

namespace Guppy.Game.MonoGame
{
    internal class MonoGameTerminal : ITerminal
    {
        private readonly MonoGameTerminalTextWriter _out;

        public const int BufferSize = 1 << 11;

        public readonly Buffer<MonoGameTerminalLine> Lines = new Buffer<MonoGameTerminalLine>(BufferSize);
        public MonoGameTerminalLineBuilder _currentLine = new MonoGameTerminalLineBuilder();
        public int FirstLineNumber { get; private set; } = 1;

        public IStandardStreamWriter Out => _out;
        public IStandardStreamWriter Error { get; }
        public bool IsOutputRedirected { get; }
        public bool IsErrorRedirected { get; }
        public bool IsInputRedirected { get; }

        TextWriter ITerminal.Out => _out;

        public Color Color
        {
            get => _currentLine.Color;
            set => _currentLine.SetColor(value);
        }

        public ITerminalTheme Theme { get; private set; }

        public MonoGameTerminal(ITerminalTheme theme)
        {
            _out = new MonoGameTerminalTextWriter(this);
            this.Error = new MonoGameTerminalErrorTextWriter(this, theme.Get(LogEventLevel.Error));
            this.Theme = theme;

            this.IsOutputRedirected = true;
            this.IsErrorRedirected = true;
            this.IsInputRedirected = true;

            this.Color = theme.Get(default!).Value;
        }

        public void WriteLine(string value)
        {
            this.Write(value);
            this.AddLine(_currentLine.NewLine());
        }

        public void WriteLine(string value, Color color)
        {
            var oldColor = this.Color;
            _currentLine.SetColor(color);
            this.WriteLine(value);

            this.Color = oldColor;
        }

        public void Write(string value)
        {
            _currentLine.Text.Append(value);
        }

        public void Write(string value, Color color)
        {
            var oldColor = this.Color;
            _currentLine.SetColor(color);
            this.Write(value);

            this.Color = oldColor;
        }

        public void NewLine()
        {
            this.AddLine(_currentLine.NewLine());
        }

        public void Write(char value)
        {
            if (_currentLine.TryAppend(value, out MonoGameTerminalLine? line) == false)
            {
                this.AddLine(line);
            }
        }

        private void AddLine(MonoGameTerminalLine line)
        {
            this.Lines.Add(line, out MonoGameTerminalLine oldLine);

            if (oldLine is not null)
            {
                MonoGameTerminalLine.Factory.TryReturn(ref oldLine);
                this.FirstLineNumber++;
            }
        }
    }
}
