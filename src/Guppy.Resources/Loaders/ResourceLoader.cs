using Autofac;
using Guppy.Attributes;
using Guppy.Files.Serialization.Json;
using Guppy.Loaders;
using Guppy.Resources.Configuration;
using Guppy.Resources.Providers;
using Guppy.Resources.Serialization.Json;
using Guppy.Resources.Serialization.Json.Converters;
using Serilog.Events;
using System.Text.Json.Serialization;

namespace Guppy.Resources.Loaders
{
    [AutoLoad]
    internal sealed class ResourceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {

            services.RegisterType<SettingProvider>().As<ISettingProvider>().SingleInstance();
            services.RegisterType<ResourcePackProvider>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<ResourceProvider>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<ResourceTypeProvider>().As<IResourceTypeProvider>().SingleInstance();
            services.RegisterGeneric(typeof(PolymorphicJsonSerializer<>)).As(typeof(IPolymorphicJsonSerializer<>)).InstancePerDependency();

            services.RegisterType<ResourcePacksConfigurationConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<IFileJsonConverter<ResourcePackConfiguration>>().As<JsonConverter>().SingleInstance();
            services.RegisterType<SettingValueDictionaryConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<SettingValueConverter>().As<JsonConverter>().SingleInstance();

            services.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<Boolean, object>(nameof(Boolean))).SingleInstance();
            services.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<Int32, object>(nameof(Int32))).SingleInstance();
            services.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<String, object>(nameof(String))).SingleInstance();
            services.RegisterInstance<PolymorphicJsonType>(new PolymorphicJsonType<LogEventLevel, object>(nameof(LogEventLevel))).SingleInstance();
        }
    }
}
