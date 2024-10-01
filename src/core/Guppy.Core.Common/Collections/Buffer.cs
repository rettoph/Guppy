using System.Collections;

namespace Guppy.Core.Common.Collections
{
    public class Buffer<T>(int length) : IEnumerable<T>
    {
        private T[] _buffer = new T[length];
        private int _position = 0;

        public int Length => _buffer.Length;
        public int Position
        {
            get => _position;
            set => _position = value;
        }

        public int Count => Math.Min(this.Position, this.Length);

        public T this[int index] => _buffer[index];

        public T[] Items => _buffer;

        public void Add(T value)
        {
            _buffer[_position++ % this.Length] = value;
        }

        public void Add(T value, out T old)
        {
            int index = _position++ % this.Length;

            old = _buffer[index];
            _buffer[index] = value;
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        public void Resize(int length)
        {
            Array.Resize(ref _buffer, length);
        }

        public void Reset(int length)
        {
            this.Reset();

            if (_buffer.Length == length)
            {
                return;
            }

            this.Resize(length);
        }
        public void Reset()
        {
            _position = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_position < this.Length)
            {
                for (int i = 0; i < _position; i++)
                {
                    yield return _buffer[i];
                }
            }
            else
            {
                for (int i = 0; i < this.Length; i++)
                {
                    yield return _buffer[(_position + i) % this.Length];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}