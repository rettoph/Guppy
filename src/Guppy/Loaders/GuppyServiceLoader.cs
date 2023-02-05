using Guppy.Attributes;
using Guppy.Common.DependencyInjection;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace Guppy.Loaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterGuppyCommon().AddSingleton<IGuppyProvider, GuppyProvider>();
            services.AddSingleton<ILogger>(p =>
            {
                var configuration = p.GetRequiredService<IOptions<LoggerConfiguration>>().Value;
                var logger = configuration.CreateLogger();

                return logger;
            });
        }
    }
}
