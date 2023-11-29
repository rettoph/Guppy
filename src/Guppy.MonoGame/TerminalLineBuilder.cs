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
        private int _lineNumber = 1;

        private TerminalLine _line;

        public Color Color;

        public StringBuilder Text;

        public TerminalLineBuilder()
        {
            _line = TerminalLine.Factory.GetInstance();
            this.Text = new StringBuilder();

            this.AppendLineNumber();
        }

        public bool TryAppend(char value, [MaybeNullWhen(true)] out TerminalLine previousLine)
        {
            if(value == '\n')
            {
                previousLine = this.NewLine();
                return false;
            }

            previousLine = null;
            this.Text.Append(value);

            return true;
        }

        public void SetColor(Color color)
        {
            this.AddSegment();
            this.Color = color;
        }

        private void AddSegment()
        {
            if(this.Text.Length == 0)
            {
                return;
            }

            TerminalSegment segment = new TerminalSegment(this.Color, this.Text.ToString());
            _line.Segments.Add(segment);

            this.Text.Clear();
        }

        public TerminalLine NewLine()
        {
            this.AddSegment();

            var result = _line;
            _line.CleanText();

            _line = TerminalLine.Factory.GetInstance();
            _line.Segments.Clear();

            this.AppendLineNumber();

            return result;
        }

        private void AppendLineNumber()
        {
            TerminalSegment segment = new TerminalSegment(Color.White, $"{_lineNumber++}: ");
            _line.Segments.Add(segment);
        }
    }
}
