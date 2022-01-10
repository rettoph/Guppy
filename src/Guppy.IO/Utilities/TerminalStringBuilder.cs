using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.IO.Utilities
{
    internal sealed class TerminalStringBuilder : IEnumerable<TerminalString>
    {
        private TerminalString[] _buffer;
        private Int32 _bufferIndex;

        private StringBuilder _currentString;
        private Color _currentColor;
        private Boolean _currentNewLine;
        private Guid _currentSourceId;

        public TerminalStringBuilder(Int32 size)
        {
            _currentString = new StringBuilder();
            _buffer = new TerminalString[size];
        }

        public void Append(Char character, Color color, Guid sourceId)
        {
            if(character == '\n' || character == '\r')
            {
                this.FlushCurrentStringBuilder(color, true, sourceId);
                return;
            }

            if (_currentSourceId != sourceId)
            {
                this.FlushCurrentStringBuilder(character, color, true, sourceId);
                return;
            }

            if (_currentColor != color)
            {
                this.FlushCurrentStringBuilder(character, color, false, sourceId);
                return;
            }

            _currentString.Append(character);
        }

        public void Append(String text, Color color, Guid sourceId)
        {
            foreach(Char character in text)
            {
                this.Append(character, color, sourceId);
            }
        }

        private void FlushCurrentStringBuilder(Color newCurrentColor, Boolean newCurrentNewLine, Guid sourceId)
        {
            if (_currentString.Length > 0)
            {
                _buffer[_bufferIndex++ % _buffer.Length] = new TerminalString(_currentString.ToString(), _currentColor, _currentNewLine);
            }

            _currentString.Clear();
            _currentColor = newCurrentColor;
            _currentNewLine = newCurrentNewLine;
            _currentSourceId = sourceId;
        }

        private void FlushCurrentStringBuilder(Char character, Color newCurrentColor, Boolean newCurrentNewLine, Guid sourceId)
        {

            if(_currentString.Length == 0)
            {
                _currentSourceId = sourceId;
                _currentColor = newCurrentColor;
                _currentString.Append(character);
                return;
            }

            this.FlushCurrentStringBuilder(newCurrentColor, newCurrentNewLine, sourceId);
            _currentString.Append(character);
        }

        public IEnumerator<TerminalString> GetEnumerator()
        {
            for(Int32 i=0; i<_buffer.Length; i++)
            {
                yield return _buffer[(_bufferIndex + i) % _buffer.Length];
            }

            String built = _currentString.ToString();
            yield return new TerminalString(built, _currentColor, _currentNewLine);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
