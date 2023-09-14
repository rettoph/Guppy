using Autofac;
using Guppy.Files.Providers;
using Guppy.Files.Services;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files.Loaders
{
    internal class FileLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<DefaultFilePathProvider>().AsImplementedInterfaces().SingleInstance();

            services.RegisterType<FileService>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
