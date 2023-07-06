using Autofac;
using Guppy.Commands.Services;
using Guppy.Loaders;

namespace Guppy.Commands.Loaders
{
    internal sealed class CommandLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<CommandService>().As<ICommandService>().InstancePerLifetimeScope();
        }
    }
}
