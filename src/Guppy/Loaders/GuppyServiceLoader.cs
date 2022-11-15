using Guppy.Attributes;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Loaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterGuppyCommon().AddSingleton<IGuppyProvider, GuppyProvider>();
        }
    }
}
