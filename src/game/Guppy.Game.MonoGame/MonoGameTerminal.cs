using System.CommandLine;
using System.CommandLine.IO;
using Guppy.Core.Common;
using Guppy.Core.Common.Collections;
using Guppy.Core.Logging.Common.Enums;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame
{
    public class MonoGameTerminal : ITerminal, IConsole
    {
        private readonly MonoGameTerminalTextWriter _out;
        private readonly MonoGameTerminalLineBuilder _currentLine;

        public const int BufferSize = 1 << 11;

        public readonly Buffer<MonoGameTerminalLine> Lines = new(BufferSize);
        public int FirstLineNumber { get; private set; } = 1;

        public IStandardStreamWriter Out => this._out;
        public IStandardStreamWriter Error { get; }
        public bool IsOutputRedirected { get; }
        public bool IsErrorRedirected { get; }
        public bool IsInputRedirected { get; }

        TextWriter ITerminal.Out => this._out;

        public IRef<Color> Color
        {
            get => this._currentLine.Color;
            set => this._currentLine.SetColor(value);
        }

        public ITerminalTheme Theme { get; private set; }

        public MonoGameTerminal(ITerminalTheme theme)
        {
            this._out = new MonoGameTerminalTextWriter(this);
            this._currentLine = new MonoGameTerminalLineBuilder(theme.Get(default!));
            this.Error = new MonoGameTerminalErrorTextWriter(this, theme.Get(LogLevelEnum.Error));
            this.Theme = theme;

            this.IsOutputRedirected = true;
            this.IsErrorRedirected = true;
            this.IsInputRedirected = true;

            this.Color = theme.Get(default!);
        }

        public void WriteLine(string value)
        {
            this.Write(value);
            this.AddLine(this._currentLine.NewLine());
        }

        public void WriteLine(string value, IRef<Color> color)
        {
            var oldColor = this.Color;
            this._currentLine.SetColor(color);
            this.WriteLine(value);

            this.Color = oldColor;
        }

        public void Write(string value)
        {
            this._currentLine.Text.Append(value);
        }

        public void Write(string value, IRef<Color> color)
        {
            var oldColor = this.Color;
            this._currentLine.SetColor(color);
            this.Write(value);

            this.Color = oldColor;
        }

        public void NewLine()
        {
            this.AddLine(this._currentLine.NewLine());
        }

        public void Write(char value)
        {
            if (this._currentLine.TryAppend(value, out MonoGameTerminalLine? line) == false)
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