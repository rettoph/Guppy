using Autofac;
using Guppy.Attributes;
using Guppy.Files.Serialization.Json;
using Guppy.Loaders;
using Guppy.Resources.Configuration;
using Guppy.Resources.Services;
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

            services.RegisterType<SettingService>().As<ISettingService>().SingleInstance();
            services.RegisterType<ResourcePackService>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<ResourceService>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<ResourceTypeService>().As<IResourceTypeService>().SingleInstance();
            services.RegisterGeneric(typeof(PolymorphicJsonSerializerService<>)).As(typeof(IPolymorphicJsonSerializerService<>)).InstancePerDependency();

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
