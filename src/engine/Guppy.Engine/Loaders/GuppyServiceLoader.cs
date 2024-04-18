using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Engine.Common.Loaders;
using Guppy.Engine.Common.Services;
using Guppy.Engine.Services;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guppy.Engine.Loaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<ObjectTextFilterService>().As<IObjectTextFilterService>().SingleInstance();

            services.Register<ILogger>(p =>
            {
                var configuration = p.Resolve<IConfiguration<LoggerConfiguration>>().Value;
                var logger = configuration.CreateLogger();

                return logger;
            }).InstancePerLifetimeScope();

            services.Configure<JsonSerializerOptions>((p, options) =>
            {
                options.PropertyNameCaseInsensitive = true;
                options.WriteIndented = true;

                foreach (JsonConverter converter in p.Resolve<IEnumerable<JsonConverter>>())
                {
                    options.Converters.Add(converter);
                }
            });

            services.RegisterInstance(new JsonStringEnumConverter()).As<JsonConverter>().SingleInstance();
        }
    }
}
