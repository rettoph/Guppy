using Guppy.Core.Common;
using Guppy.Core.Common.Collections;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;
using Serilog.Events;
using System.CommandLine;
using System.CommandLine.IO;

namespace Guppy.Game.MonoGame
{
    internal class MonoGameTerminal : ITerminal, IConsole
    {
        private readonly MonoGameTerminalTextWriter _out;

        public const int BufferSize = 1 << 11;

        public readonly Buffer<MonoGameTerminalLine> Lines = new Buffer<MonoGameTerminalLine>(BufferSize);
        public MonoGameTerminalLineBuilder _currentLine;
        public int FirstLineNumber { get; private set; } = 1;

        public IStandardStreamWriter Out => _out;
        public IStandardStreamWriter Error { get; }
        public bool IsOutputRedirected { get; }
        public bool IsErrorRedirected { get; }
        public bool IsInputRedirected { get; }

        TextWriter ITerminal.Out => _out;

        public IRef<Color> Color
        {
            get => _currentLine.Color;
            set => _currentLine.SetColor(value);
        }

        public ITerminalTheme Theme { get; private set; }

        public MonoGameTerminal(ITerminalTheme theme)
        {
            _out = new MonoGameTerminalTextWriter(this);
            _currentLine = new MonoGameTerminalLineBuilder(theme.Get(default!));
            this.Error = new MonoGameTerminalErrorTextWriter(this, theme.Get(LogEventLevel.Error));
            this.Theme = theme;

            this.IsOutputRedirected = true;
            this.IsErrorRedirected = true;
            this.IsInputRedirected = true;

            this.Color = theme.Get(default!);
        }

        public void WriteLine(string value)
        {
            this.Write(value);
            this.AddLine(_currentLine.NewLine());
        }

        public void WriteLine(string value, IRef<Color> color)
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

        public void Write(string value, IRef<Color> color)
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
