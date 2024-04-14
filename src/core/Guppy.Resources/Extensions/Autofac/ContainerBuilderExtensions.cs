using Autofac;
using Guppy.Resources.Configuration;

namespace Guppy.Resources.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterResourcePack(this ContainerBuilder services, ResourcePackConfiguration configuration)
        {
            services.RegisterInstance<ResourcePackConfiguration>(configuration);
        }
    }
}
