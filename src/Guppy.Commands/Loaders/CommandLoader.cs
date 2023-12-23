using Autofac;
using Guppy.Attributes;
using Guppy.Commands.Services;
using Guppy.Common.Autofac;
using Guppy.Loaders;
using Guppy.Providers;
using System.CommandLine;
using System.CommandLine.IO;

namespace Guppy.Commands.Loaders
{
    [AutoLoad]
    internal sealed class CommandLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<CommandService>().As<ICommandService>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);
            services.RegisterType<SystemConsole>().As<IConsole>().SingleInstance();

            services.RegisterType<BulkGuppyBrokerSubscriptionProvider<ICommandService, ICommand>>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
