using Guppy.Interfaces;
using Guppy.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Pools
{
    public class ScopedInitializablePool<T> : InitializablePool<T>
        where T : class, IInitializable
    {
        public ScopedInitializablePool(Type targetType = null) : base(targetType)
        {
        }

        protected override T Create(IServiceProvider provider)
        {
            return base.Create(provider.CreateScopeWithConfiguration());
        }
    }
}
