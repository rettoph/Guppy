using Guppy.Input.Providers;
using Guppy.Loaders;
using Guppy.Input.Services;
using Autofac;

namespace Guppy.Input.Loaders
{
    internal sealed class InputLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<ButtonService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            services.RegisterType<KeyboardButtonProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
            services.RegisterType<MouseButtonProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
            services.RegisterType<MouseEventPublishService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            services.RegisterType<CursorProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
