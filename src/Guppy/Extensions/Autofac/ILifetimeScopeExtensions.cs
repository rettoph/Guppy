﻿using Autofac;
using Guppy.Common;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Extensions.Autofac
{
    public static class ILifetimeScopeExtensions
    {
        public static ILifetimeScope BeginGuppyScope(this ILifetimeScope lifetimeScope, object tag, Action<ContainerBuilder>? build = null)
        {
            return lifetimeScope.BeginLifetimeScope(tag, builder =>
            {
                foreach(IServiceLoader loader in lifetimeScope.Resolve<IEnumerable<IServiceLoader>>())
                {
                    if(loader.LifetimeScopeTag.Equals(tag))
                    {
                        loader.ConfigureServices(builder);
                    }
                }

                build?.Invoke(builder);
            });
        }

        public static bool HasTag(this ILifetimeScope lifetimeScope, object tag)
        {
            return lifetimeScope.Resolve<ITags>().Has(tag);
        }

        public static bool IsTag(this ILifetimeScope lifetimeScope, object tag)
        {
            return lifetimeScope.Tag.Equals(tag);
        }
    }
}
