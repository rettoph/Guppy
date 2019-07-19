using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    class LoaderFactory<TLoader> : Factory<TLoader>
        where TLoader : class, ILoader
    {
        public override TLoader Create(IServiceProvider provider)
        {
            return provider.GetLoader<TLoader>();
        }
        public override TLoader CreateCustom(IServiceProvider provider, params object[] args)
        {
            return ActivatorUtilities.CreateInstance<TLoader>(provider, this.targetType, args);
        }

        public static LoaderFactory<T> BuildFactory<T>()
            where T : class, ILoader
        {
            return new LoaderFactory<T>();
        }
    }
}
