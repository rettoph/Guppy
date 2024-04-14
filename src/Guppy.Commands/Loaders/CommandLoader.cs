using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Commands.Services;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Loaders;
using Guppy.Messaging;
using System.CommandLine;
using System.CommandLine.IO;

namespace Guppy.Commands.Loaders
{
    [AutoLoad]
    internal sealed class CommandLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<CommandService>().As<ICommandService>().As<IMagicBroker>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);
            services.RegisterType<SystemConsole>().As<IConsole>().SingleInstance();
        }
    }
}
