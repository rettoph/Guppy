using Autofac;
using Guppy.Attributes;
using Guppy.Game.Input.Providers;
using Guppy.Game.Input.Services;
using Guppy.Loaders;

namespace Guppy.Game.Input.Loaders
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
