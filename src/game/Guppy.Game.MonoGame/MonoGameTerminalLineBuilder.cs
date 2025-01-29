using System.Diagnostics.CodeAnalysis;
using System.Text;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame
{
    public class MonoGameTerminalLineBuilder
    {
        private int _lineNumber;
        private MonoGameTerminalLine _line;
        private readonly StringBuilder _text;

        public IRef<Color> Color;

        public MonoGameTerminalLineBuilder(IRef<Color> color)
        {
            this._lineNumber = 1;
            this._line = MonoGameTerminalLine.Factory.GetOrCreate();
            this._text = new StringBuilder();
            this._text.Append($"{this._lineNumber++}: ");

            this.Color = color;
        }

        public bool TryAppend(char value, [MaybeNullWhen(true)] out MonoGameTerminalLine previousLine)
        {
            if (value == '\n')
            {
                previousLine = this.Flush();
                return false;
            }

            previousLine = null;
            this._text.Append(value);

            return true;
        }

        public void SetColor(IRef<Color> color)
        {
            if (this.Color.Value == color.Value)
            {
                return;
            }

            this._line.Segments.Add(new MonoGameTerminalSegment()
            {
                Text = this._text.Flush(),
                Color = this.Color.Value.ToVector4()
            });

            this.Color = color;
        }

        public MonoGameTerminalLine Flush()
        {
            this._line.Segments.Add(new MonoGameTerminalSegment()
            {
                Text = this._text.Flush(),
                Color = this.Color.Value.ToVector4()
            });

            MonoGameTerminalLine result = this._line;
            result.Clean();

            this._line = MonoGameTerminalLine.Factory.GetOrCreate();
            this._text.Append($"{this._lineNumber++}: ");

            return result;
        }

        public void Append(string text)
        {
            this._text.Append(text);
        }
    }
}