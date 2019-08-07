using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    public class ServicePool<T> : Pool<T>
    {
        protected Type targetType { get; private set; }

        public ServicePool(Type targetType = null) : base()
        {
            this.targetType = targetType;
        }

        protected override T Create(IServiceProvider provider)
        {
            if (targetType == null)
                return ActivatorUtilities.CreateInstance<T>(provider);
            else
                return (T)ActivatorUtilities.CreateInstance(provider, targetType);
        }
    }
}
