using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Game.Input.Services;
using Guppy.Engine.Common.Loaders;

namespace Guppy.Game.Input.Loaders
{
    [AutoLoad]
    internal sealed class InputLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<KeyboardButtonService>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<MouseButtonService>().AsImplementedInterfaces().SingleInstance();
            services.RegisterType<CursorService>().AsImplementedInterfaces().SingleInstance();

            services.RegisterType<InputService>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
