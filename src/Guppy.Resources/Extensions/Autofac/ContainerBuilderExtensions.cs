using Autofac;
using Guppy.Extensions.Autofac;
using Guppy.Resources.Configuration;

namespace Guppy.Resources.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void ConfigureResourcePacks(this ContainerBuilder services, Action<ILifetimeScope, ResourcePacksConfiguration> builder)
        {
            services.Configure<ResourcePacksConfiguration>(builder);
        }
    }
}
