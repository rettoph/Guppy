using Guppy.Input.Providers;
using Guppy.Loaders;
using Autofac;
using Guppy.Input.Services;
using Guppy.Input.Components;
using Guppy.Attributes;

namespace Guppy.Input.Loaders
{
    [AutoLoad]
    internal sealed class InputLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<KeyboardButtonProvider>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<MouseButtonProvider>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<CursorProvider>().AsImplementedInterfaces().SingleInstance();

            services.RegisterType<InputService>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
