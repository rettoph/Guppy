using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Common.Collections;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Configuration;
using System.CommandLine.IO;
using Guppy.Resources.Providers;

namespace Guppy.MonoGame
{
    internal class Terminal : ITerminal
    {
        private readonly TerminalTextWriter _out;

        public const int BufferSize = 1 << 11;

        public readonly Buffer<TerminalLine> Lines = new Buffer<TerminalLine>(BufferSize);
        public TerminalLineBuilder _currentLine = new TerminalLineBuilder();
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

        public TerminalTheme Theme { get; private set; }

        public Terminal(TerminalTheme theme)
        {
            _out = new TerminalTextWriter(this);
            this.Error = new TerminalErrorTextWriter(this, theme.Error);
            this.Theme = theme;

            this.IsOutputRedirected = true;
            this.IsErrorRedirected = true;
            this.IsInputRedirected = true;

            this.Color = theme.Default;

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
            if(_currentLine.TryAppend(value, out TerminalLine? line) == false)
            {
                this.AddLine(line);
            }
        }

        private void AddLine(TerminalLine line)
        {
            this.Lines.Add(line, out TerminalLine oldLine);

            if (oldLine is not null)
            {
                TerminalLine.Factory.TryReturn(ref oldLine);
                this.FirstLineNumber++;
            }
        }
    }
}
