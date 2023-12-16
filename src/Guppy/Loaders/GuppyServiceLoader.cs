using Autofac;
using Guppy.Attributes;
using Guppy.Providers;
using Serilog;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common;
using System.Text.Json.Serialization;
using System.Text.Json;
using Guppy.Serialization;
using Guppy.Common.Autofac;
using Guppy.Messaging;
using Guppy.Common.Providers;
using Guppy.Extensions.Autofac;

namespace Guppy.Loaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisteGuppyCommon();

            services.RegisterType<GuppyProvider>().As<IGuppyProvider>().SingleInstance();

            services.RegisterType<Tags>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();

            services.RegisterType<Broker<IMessage>>().As<IBroker<IMessage>>().InstancePerDependency();
            services.RegisterType<Bus>().As<IBus>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);

            services.RegisterGeneric(typeof(Lazier<>)).As(typeof(Lazier<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Scoped<>)).As(typeof(IScoped<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Filtered<>)).As(typeof(IFiltered<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Configuration<>)).As(typeof(IConfiguration<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Optional<>)).As(typeof(IOptional<>)).InstancePerDependency();

            services.RegisterType<FilteredProvider>().As<IFilteredProvider>().InstancePerLifetimeScope();
            services.RegisterType<BulkGuppyBrokerSubscriptionProvider<IBus, IMessage>>().AsImplementedInterfaces().InstancePerLifetimeScope();

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
            services.RegisterType<Serialization.JsonSerializer>().As<IJsonSerializer>().InstancePerDependency();
        }
    }
}
