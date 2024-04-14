using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Engine.Loaders;

namespace Guppy.Game.MonoGame.Loaders
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
