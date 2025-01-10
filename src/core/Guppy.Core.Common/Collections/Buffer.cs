using System.Collections;

namespace Guppy.Core.Common.Collections
{
    public class Buffer<T>(int length) : IEnumerable<T>
    {
        private T[] _buffer = new T[length];

        public int Length => this._buffer.Length;
        public int Position { get; set; } = 0;

        public int Count => Math.Min(this.Position, this.Length);

        public T this[int index] => this._buffer[index];

        public T[] Items => this._buffer;

        public void Add(T value)
        {
            this._buffer[this.Position++ % this.Length] = value;
        }

        public void Add(T value, out T old)
        {
            int index = this.Position++ % this.Length;

            old = this._buffer[index];
            this._buffer[index] = value;
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
            Array.Resize(ref this._buffer, length);
        }

        public void Reset(int length)
        {
            this.Reset();

            if (this._buffer.Length == length)
            {
                return;
            }

            this.Resize(length);
        }
        public void Reset()
        {
            this.Position = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.Position < this.Length)
            {
                for (int i = 0; i < this.Position; i++)
                {
                    yield return this._buffer[i];
                }
            }
            else
            {
                for (int i = 0; i < this.Length; i++)
                {
                    yield return this._buffer[(this.Position + i) % this.Length];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}