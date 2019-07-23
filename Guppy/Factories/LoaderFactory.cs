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
        public static LoaderFactory<T> BuildFactory<T>()
            where T : class, ILoader
        {
            return new LoaderFactory<T>();
        }
    }
}
