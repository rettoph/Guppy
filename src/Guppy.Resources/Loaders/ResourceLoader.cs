using Autofac;
using Guppy.Loaders;
using Guppy.Files.Serialization.Json;
using Guppy.Resources.Configuration;
using Guppy.Resources.Providers;
using System.Text.Json.Serialization;
using Guppy.Resources.Serialization.Json.Converters;

namespace Guppy.Resources.Loaders
{
    internal sealed class ResourceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<ResourcePackProvider>().As<IResourcePackProvider>().SingleInstance();
            services.RegisterType<ResourceProvider>().As<IResourceProvider>().SingleInstance();

            services.RegisterType<RawResourceValuesConverter>().As<JsonConverter>().SingleInstance();
            services.RegisterType<IOptionsJsonConverter<ResourcePackConfiguration>>().As<JsonConverter>().SingleInstance();
        }
    }
}
