using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Common.Collections
{
    public class Factory<T, TPool>
        where T : class
        where TPool : IPool<T>
    {
        private TPool _pool;
        private Func<T> _method;

        public Factory(Func<T> method, TPool pool)
        {
            _method = method;
            _pool = pool;
        }

        public virtual T GetInstance()
        {
            if(!_pool.TryPull(out T? instance))
            {
                instance = this.Build();
            }

            return instance;
        }

        protected virtual T Build()
        {
            return _method();
        }

        public virtual bool TryReturnToPool(T instance)
        {
            return _pool.TryReturn(ref instance);
        }
    }

    public class Factory<T> : Factory<T, Pool<T>>
        where T : class
    {
        public Factory(Func<T> method, ushort poolSize = 50) : base(method, new Pool<T>(ref poolSize))
        {
        }
    }
}
