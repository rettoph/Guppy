using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Common.Extensions.Autofac;
using Guppy.Engine.Common.Services;
using Guppy.Engine.Extensions.Autofac;
using Guppy.Engine.Providers;
using Guppy.Engine.Serialization;
using Guppy.Engine.Services;
using Guppy.Core.Messaging;
using Guppy.Core.Messaging.Services;
using Guppy.Core.StateMachine.Services;
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
            services.RegisteGuppyCommon();

            services.RegisterType<GuppyProvider>().As<IGuppyProvider>().SingleInstance();
            services.RegisterType<ObjectTextFilterService>().As<IObjectTextFilterService>().SingleInstance();

            services.RegisterType<Tags>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();

            services.RegisterType<Broker<IMessage>>().As<IBroker<IMessage>>().InstancePerDependency();
            services.RegisterType<Bus>().As<IBus>().As<IMagicBroker>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);

            services.RegisterGeneric(typeof(Lazier<>)).As(typeof(Lazier<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Scoped<>)).As(typeof(IScoped<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Filtered<>)).As(typeof(IFiltered<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Configuration<>)).As(typeof(IConfiguration<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Optional<>)).As(typeof(IOptional<>)).InstancePerDependency();

            services.RegisterType<FilteredService>().As<IFilteredService>().InstancePerLifetimeScope();
            services.RegisterType<StateService>().As<IStateService>().InstancePerLifetimeScope();
            services.RegisterType<MagicBrokerService>().As<IMagicBrokerService>().InstancePerLifetimeScope();
            services.RegisterType<DefaultInstanceService>().As<IDefaultInstanceService>().InstancePerDependency();

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
