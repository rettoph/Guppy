using Autofac;
using Guppy.Attributes;
using Guppy.Files.Providers;
using Guppy.Files.Services;
using Guppy.Loaders;

namespace Guppy.Files.Loaders
{
    [AutoLoad]
    internal class FileLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<DefaultFilePathProvider>().AsImplementedInterfaces().SingleInstance();

            services.RegisterType<FileService>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
