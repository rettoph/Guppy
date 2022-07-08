using Guppy.Loaders;
using Guppy.MonoGame.Providers.ResourcePackTypeProviders;
using Guppy.Resources.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class ResourceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IResourcePackTypeProvider, ResourcePackColorProvider>();
        }
    }
}
