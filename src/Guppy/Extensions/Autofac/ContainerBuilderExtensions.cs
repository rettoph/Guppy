using Autofac;
using Guppy.Common.Autofac;
using Guppy.Common.Implementations;
using Guppy.Common.Providers;
using Guppy.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void Configure<T>(this ContainerBuilder services, Action<ILifetimeScope, T> builder)
            where T : new()
        {
            services.RegisterInstance(new ConfigurationBuilder<T>(builder));
        }
    }
}
