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
            this._maxPoolSize = maxPoolSize;
            this._poolSize = 0;
            this._pool = new Stack<T>();
        }

        /// <inheritdoc />
        public virtual bool Any()
        {
            return this._pool.Count != 0;
        }

        /// <inheritdoc />
        public virtual bool TryPull([MaybeNullWhen(false)] out T instance)
        {
            if (this._pool.TryPop(out instance))
            {
                this._poolSize--;

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual bool TryReturn(T instance)
        {
            if (this._poolSize < this._maxPoolSize)
            {
                this._pool.Push(instance);
                this._poolSize++;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public virtual int Count()
        {
            return this._poolSize;
        }
    }
}