using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Collections
{
    public class Buffer<T> : IEnumerable<T>
    {
        private T[] _buffer;
        private int _position;

        public int Length => _buffer.Length;
        public int Position
        {
            get => _position;
            set => _position = value;
        }

        public T this[int index] => _buffer[index];

        public Buffer(int length)
        {
            _buffer = new T[length];
            _position = 0;
        }

        public void Add(T item)
        {
            _buffer[_position++ % this.Length] = item;
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