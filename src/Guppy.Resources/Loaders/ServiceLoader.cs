using Autofac;
using Guppy.Loaders;
using Guppy.Files.Serialization.Json;
using Guppy.Resources.Configuration;
using Guppy.Resources.Providers;
using System.Text.Json.Serialization;
using Guppy.Resources.Serialization.Json.Converters;

namespace Guppy.Resources.Loaders
{
    internal sealed class ServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<SettingProvider>().As<ISettingProvider>().SingleInstance();
            services.RegisterType<ResourcePackProvider>().As<IResourcePackProvider>().SingleInstance();
            services.RegisterType<ResourceProvider>().As<IResourceProvider>().SingleInstance();
            services.RegisterType<ResourceTypeProvider>().As<IResourceTypeProvider>().SingleInstance();

            services.RegisterType<IFileJsonConverter<ResourcePackConfiguration>>().As<JsonConverter>().SingleInstance();
            services.RegisterType<SettingValueDictionaryConverter>().As<JsonConverter>().SingleInstance();
        }
    }
}
