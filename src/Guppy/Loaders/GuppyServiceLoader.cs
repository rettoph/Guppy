using Autofac;
using Guppy.Attributes;
using Guppy.Providers;
using Serilog;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common;
using System.Text.Json.Serialization;
using System.Text.Json;
using Guppy.Serialization;

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

            services.Register<JsonSerializerOptions>(p =>
            {
                var options = new JsonSerializerOptions();

                foreach (JsonConverter converter in p.Resolve<IEnumerable<JsonConverter>>())
                {
                    options.Converters.Add(converter);
                }

                return options;
            }).InstancePerDependency();

            services.RegisterType<Serialization.JsonSerializer>().As<IJsonSerializer>().InstancePerDependency();
        }
    }
}
