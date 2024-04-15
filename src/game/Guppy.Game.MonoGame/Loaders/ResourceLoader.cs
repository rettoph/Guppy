using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Loaders;

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
