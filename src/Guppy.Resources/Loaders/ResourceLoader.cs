using Autofac;
using Guppy.Loaders;
using Guppy.Resources.Providers;

namespace Guppy.Resources.Loaders
{
    internal sealed class ResourceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<ResourcePackProvider>().As<IResourcePackProvider>().SingleInstance();
            services.RegisterType<ResourceProvider>().As<IResourceProvider>().SingleInstance();
        }
    }
}
