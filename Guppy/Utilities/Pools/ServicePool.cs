using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Utilities.Pools
{
    /// <summary>
    /// Simple pooling class, used for object creation.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class ServicePool<TService>
    {
        protected IServiceProvider provider { get; private set; }
        private Queue<TService> _pool;

        public ServicePool(IServiceProvider provider)
        {
            this.provider = provider;
        }

        protected virtual TService Create()
        {
            return this.provider.GetService<TService>();
        }

        public void Put(TService service)
        {
            _pool.Enqueue(service);
        }

        public TService Pull()
        {
            if (_pool.Count == 0)
                this.Put(this.Create());

            return _pool.Dequeue();
        }
    }
}
