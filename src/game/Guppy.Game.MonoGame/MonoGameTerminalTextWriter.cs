using System.CommandLine.IO;
using System.Text;
using Guppy.Game.Common;

namespace Guppy.Game.MonoGame
{
    public class MonoGameTerminalTextWriter(MonoGameTerminal terminal) : TextWriter, IStandardStreamWriter
    {
        private readonly MonoGameTerminal _terminal = terminal;

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(string? value)
        {
            if (value is null)
            {
                return;
            }

            int newLineIndex;
            int lineStart = 0;

            while (-1 != (newLineIndex = value.IndexOf('\n', lineStart)))
            {
                if (lineStart == newLineIndex)
                {
                    this._terminal.NewLine();
                }
                else
                {
                    this.WriteLineToTerminal(this._terminal, value[lineStart..newLineIndex]);
                }

                lineStart = newLineIndex + 1;
            }

            if (lineStart < value.Length)
            {
                this.WriteToTerminal(this._terminal, value[lineStart..]);
            }
        }

        public override void Write(char value)
        {
            this.WriteToTerminal(this._terminal, value);
        }

        protected virtual void WriteToTerminal(MonoGameTerminal terminal, char value)
        {
            terminal.Write(value);
        }

        protected virtual void WriteToTerminal(ITerminal terminal, string value)
        {
            terminal.Write(value);
        }

        protected virtual void WriteLineToTerminal(ITerminal terminal, string value)
        {
            terminal.WriteLine(value);
        }
    }
}