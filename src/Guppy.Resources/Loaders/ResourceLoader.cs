using Autofac;
using Guppy.Loaders;
using Guppy.Files.Serialization.Json;
using Guppy.Resources.Configuration;
using Guppy.Resources.Providers;
using System.Text.Json.Serialization;
using Guppy.Resources.Serialization.Json.Converters;
using Guppy.Attributes;

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

            services.RegisterType<IFileJsonConverter<ResourcePackConfiguration>>().As<JsonConverter>().SingleInstance();
            services.RegisterType<SettingValueDictionaryConverter>().As<JsonConverter>().SingleInstance();
        }
    }
}
