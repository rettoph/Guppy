using Guppy.Game;
using Guppy.Game.Common;
using System;
using System.Collections.Generic;
using System.CommandLine.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    internal class MonoGameTerminalTextWriter : TextWriter, IStandardStreamWriter
    {
        private readonly MonoGameTerminal _terminal;

        public MonoGameTerminalTextWriter(MonoGameTerminal terminal)
        {
            _terminal = terminal;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(string? value)
        {
            if(value is null)
            {
                return;
            }

            int newLineIndex;
            int lineStart = 0;

            while(-1 != (newLineIndex = value.IndexOf('\n', lineStart)))
            {
                if(lineStart == newLineIndex)
                {
                    _terminal.NewLine();
                }
                else
                {
                    this.WriteLineToTerminal(_terminal, value[lineStart..newLineIndex]);
                }

                lineStart = newLineIndex + 1;
            } 

            if(lineStart < value.Length)
            {
                this.WriteToTerminal(_terminal, value[lineStart..]);
            }
        }

        public override void Write(char value)
        {
            this.WriteToTerminal(_terminal, value);
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
