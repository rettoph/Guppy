using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Common.Collections
{
    public class Pool<T>
    {
        private readonly Stack<T> _pool;
        private ushort _poolSize;
        private readonly ushort _maxPoolSize;

        public Pool(ushort maxPoolSize) : this(ref maxPoolSize)
        {

        }
        public Pool(ref ushort maxPoolSize)
        {
            _maxPoolSize = maxPoolSize;
            _poolSize = 0;
            _pool = new Stack<T>();
        }

        /// <inheritdoc />
        public virtual bool Any()
        {
            return _pool.Any();
        }

        /// <inheritdoc />
        public virtual bool TryPull([MaybeNullWhen(false)] out T instance)
        {
            if (_pool.TryPop(out instance))
            {
                _poolSize--;

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual bool TryReturn(ref T instance)
        {
            if (_poolSize < _maxPoolSize)
            {
                _pool.Push(instance);
                _poolSize++;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual int Count()
        {
            return _poolSize;
        }
    }
}
