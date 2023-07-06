using Autofac;
using Guppy.Attributes;
using Guppy.Providers;
using Serilog;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common;

namespace Guppy.Loaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisteGuppyCommon();

            services.RegisterType<GuppyProvider>().As<IGuppyProvider>().SingleInstance();

            services.Register<ILogger>(p =>
            {
                var configuration = p.Resolve<IOptions<LoggerConfiguration>>().Value;
                var logger = configuration.CreateLogger();

                return logger;
            }).InstancePerLifetimeScope();
        }
    }
}
