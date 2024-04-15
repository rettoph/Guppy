using Autofac;
using Guppy.Core.Resources.Configuration;

namespace Guppy.Core.Resources.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterResourcePack(this ContainerBuilder services, ResourcePackConfiguration configuration)
        {
            services.RegisterInstance<ResourcePackConfiguration>(configuration);
        }
    }
}
