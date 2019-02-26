using Guppy.Extensions;
using Guppy.Interfaces;
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

        public static LoaderFactory<T> BuildFactory<T>()
            where T : class, ILoader
        {
            return new LoaderFactory<T>();
        }
    }
}
