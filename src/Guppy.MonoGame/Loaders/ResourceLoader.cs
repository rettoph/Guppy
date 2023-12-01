﻿using Autofac;
using Guppy.Attributes;
using Guppy.Loaders;

namespace Guppy.MonoGame.Loaders
{
    [AutoLoad]
    internal sealed class ResourceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            // services.AddTransient<IResourcePackTypeProvider, ResourcePackColorProvider>();
        }
    }
}
