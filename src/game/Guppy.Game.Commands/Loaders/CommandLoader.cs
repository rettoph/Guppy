using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Game.Commands.Services;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Loaders;
using Guppy.Core.Messaging;
using System.CommandLine;
using System.CommandLine.IO;

namespace Guppy.Game.Commands.Loaders
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
