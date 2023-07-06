using Autofac;
using Guppy.Loaders;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class ResourceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            // services.AddTransient<IResourcePackTypeProvider, ResourcePackColorProvider>();
        }
    }
}
