using Autofac;
using Guppy.Attributes;
using Guppy.Files.Providers;
using Guppy.Files.Serialization.Json;
using Guppy.Files.Services;
using Guppy.Loaders;
using System.Text.Json.Serialization;

namespace Guppy.Files.Loaders
{
    [AutoLoad]
    internal class FileLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<IFileLocationJsonConverter>().As<JsonConverter>().SingleInstance();

            services.RegisterType<FilePathProvider>().AsImplementedInterfaces().SingleInstance();

            services.RegisterType<FileService>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
