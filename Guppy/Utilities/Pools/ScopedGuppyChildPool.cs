using Guppy.Interfaces;
using Guppy.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    public class ScopedGuppyChildPool<T> : ReusablePool<T>
        where T : class, IReusable
    {
        public ScopedGuppyChildPool(Type targetType = null) : base(targetType)
        {
        }

        protected override T Create(IServiceProvider provider)
        {
            return base.Create(provider.CreateScopeWithConfiguration());
        }
    }
}
