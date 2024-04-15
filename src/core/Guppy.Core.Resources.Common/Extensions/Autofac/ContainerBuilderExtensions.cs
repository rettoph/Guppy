using Autofac;
using Guppy.Core.Resources.Common.Configuration;

namespace Guppy.Core.Resources.Common.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterResourcePack(this ContainerBuilder services, ResourcePackConfiguration configuration)
        {
            services.RegisterInstance<ResourcePackConfiguration>(configuration);
        }
    }
}
